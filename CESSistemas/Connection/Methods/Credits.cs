using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using Promig.Exceptions;
using Promig.Model;
using Promig.Model.CbModel;

namespace Promig.Connection.Methods {
    class Credits {

        /*______________________________________________________________________
         |
         |                      CLASSE DE ACESSO A DADOS
         |
         |      Classe para inserir editar, excluir, inativar e recuperar todos
         |      os dados relacionados a contas a pagar do sistema do 
         |      banco de dados.
         |
        */


        #region Header
        private MySqlConnection conn;
        private Client customer;

        public Credits() {
            this.conn = ConnectionFactory.GetConnection();
            this.customer = new Client();
        }
        #endregion

        #region Data-Access

        public void AddCredit(Credit credit) {

            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando de inserção
                string command = $"INSERT INTO {Refs.TABLE_CREDITS}" +
                                 $"(id_cliente, descricao, data_recebimento, " +
                                 $"data_vencimento, valor_total, valor_inicial) " +
                                 $"VALUES(@id_cliente, @descricao, @data_recebimento, @data_vencimento, " +
                                 $"@valor_total, @valor_inicial);";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos valores dos parametros
                cmd.Parameters.Add(new MySqlParameter("@id_cliente", credit.IdCustomer));
                cmd.Parameters.Add(new MySqlParameter("@descricao", credit.Description));
                cmd.Parameters.Add(new MySqlParameter("@data_recebimento", credit.ReceiptDate));
                cmd.Parameters.Add(new MySqlParameter("@data_vencimento", credit.DueDate));
                cmd.Parameters.Add(new MySqlParameter("@valor_total", credit.TotalAmount));
                cmd.Parameters.Add(new MySqlParameter("@valor_inicial", credit.StartAmount));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

                // Mensagem de sucesso
                MessageBox.Show(
                    "Contas a Receber Inserido!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

            }
            catch (MySqlException ex) {

                // Fechando conexão com banco e disparando exceção
                MessageBox.Show(ex.Message);
                conn.Close();
                throw new DatabaseInsertException();
            }
            finally {

                // Sempre fecha conexão com banco
                conn.Close();
            }

        }

        public void EditCredit(Credit credit) {

            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando de edição
                string command = $"UPDATE {Refs.TABLE_CREDITS} SET " +
                                 $"id_cliente = @id_cliente, descricao = @descricao, " +
                                 $"data_recebimento = @data_recebimento, data_vencimento = @data_vencimento, " +
                                 $"valor_total = @valor_total, valor_inicial = @valor_inicial " +
                                 $"WHERE id_credito = @id_credito;";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos valores dos parametros
                cmd.Parameters.Add(new MySqlParameter("@id_credito", credit.IdCredit));
                cmd.Parameters.Add(new MySqlParameter("@id_cliente", credit.IdCustomer));
                cmd.Parameters.Add(new MySqlParameter("@descricao", credit.Description));
                cmd.Parameters.Add(new MySqlParameter("@data_recebimento", credit.ReceiptDate));
                cmd.Parameters.Add(new MySqlParameter("@data_vencimento", credit.DueDate));
                cmd.Parameters.Add(new MySqlParameter("@valor_total", credit.TotalAmount));
                cmd.Parameters.Add(new MySqlParameter("@valor_inicial", credit.StartAmount));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

                // Mensagem de sucesso
                MessageBox.Show(
                    "Contas a Receber Alterado!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

            }
            catch (MySqlException) {

                // Fechando conexão com banco de disparando exceção
                conn.Close();
                throw new DatabaseEditException();
            }
            finally {

                // Fechamento da conexão
                conn.Close();
            }

        }

        public void DeleteCredit(int id_credit) {

            try {

                // Abertura da conexão com banco
                conn.Open();

                // Definição do comando de exclusão
                string command = $"DELETE FROM {Refs.TABLE_CREDITS} " +
                                 $"WHERE id_credito = @id_credito;";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos parametros do comando
                cmd.Parameters.Add(new MySqlParameter("@id_credito", id_credit));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

                // Mensagem de sucesso
                MessageBox.Show(
                    "Contas a Receber Deletada!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

            }
            catch (MySqlException) {

                // Fechamento da conexão
                conn.Close();
                throw new DatabaseDeleteException();
            }
            finally {

                // Fechamento da conexão com banco
                conn.Close();
            }

        }

        public Credit GetCreditData(int id_credit) {

            try {

                // Abertura da conexão com banco
                conn.Open();

                // Definição do comando sql
                string command = $"SELECT id_credito, id_cliente, descricao, data_recebimento, " +
                                 $"data_vencimento, valor_total, valor_inicial " +
                                 $"FROM {Refs.TABLE_CREDITS} " +
                                 $"WHERE id_credito = @id_credito;";

                // Definição do comando instanciado
                Credit result = new Credit();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Adicionando parametros a busca
                cmd.Parameters.Add(new MySqlParameter("@id_credito", id_credit));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Realizando busca no banco
                reader = cmd.ExecuteReader();

                // Recuperando dados do orçamento
                while (reader.Read()) {
                    result.IdCredit = int.Parse(reader["id_credito"].ToString());
                    result.IdCustomer = int.Parse(reader["id_cliente"].ToString());
                    result.Description = reader["descricao"].ToString();
                    result.ReceiptDate = reader["data_recebimento"].ToString();
                    result.DueDate = reader["data_vencimento"].ToString();
                    result.TotalAmount = double.Parse(reader["valor_total"].ToString());
                    result.StartAmount = double.Parse(reader["valor_inicial"].ToString());

                }
                reader.Close();

                return result;

            }
            catch (MySqlException) {

                // Fechando conexão com banco de disparando exceção
                conn.Close();
                throw new DatabaseAccessException();
            }
            finally {

                // Fechamento da conexão e retorno
                conn.Close();
            }

        }

        public List<Credit> GetAllCredit(string param) {


            try {

                // Abertura da conexão com o banco
                conn.Open();

                // Definição do comando de consulta
                string command = $"SELECT id_credito, id_cliente, descricao, data_recebimento, " +
                                 $"data_vencimento, valor_total, valor_inicial " +
                                 $"FROM {Refs.TABLE_CREDITS} " +
                                 $"WHERE descricao LIKE @param " +
                                 $"order by descricao;";

                // Definição do comando instanciado
                List<Credit> results = new List<Credit>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Adicionando parametros a busca
                cmd.Parameters.Add(new MySqlParameter("@param", $"%{param}%"));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Realizando busca no banco
                reader = cmd.ExecuteReader();

                // Verificando resultados
                while (reader.Read()) {
                    Credit credit = new Credit();
                    credit.IdCredit = int.Parse(reader["id_credito"].ToString());
                    credit.Description = reader["descricao"].ToString();
                    credit.TotalAmount = double.Parse(reader["valor_total"].ToString());
                    credit.DueDate = reader["data_vencimento"].ToString();
                    results.Add(credit);
                }

                return results;

            }
            catch (MySqlException) {

                // Fechando conexão com banco de disparando exceção
                conn.Close();
                throw new DatabaseAccessException();
            }
            finally {

                // Fechamento da conexão
                conn.Close();
            }

        }

        #endregion

        #region Utils

        public int GetIdCustomer(int id) {

            int id_cliente = 0;
            try {

                conn.Open();

                string command = $"select id_cliente from clientes " +
                                 $"where id_pessoa = @id_pessoa;";

                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.Add(new MySqlParameter("@id_pessoa", id));
                cmd.Prepare();

                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    id_cliente = int.Parse(reader["id_cliente"].ToString());
                }

                conn.Close();
                return id_cliente;

            }
            catch (MySqlException) {

                conn.Close();
                return id_cliente;
            }
        }

        public int GetIdPessoaCustomer(string name) {

            int id_pessoa = 0;
            try {

                conn.Open();

                string command = $"select id_pessoa from pessoas " +
                                 $"where nome_pessoa = @nome;";

                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.Add(new MySqlParameter("@nome", name));
                cmd.Prepare();

                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    id_pessoa = int.Parse(reader["id_pessoa"].ToString());
                }

                conn.Close();
                return id_pessoa;

            }
            catch (MySqlException) {

                conn.Close();
                return id_pessoa;
            }

        }

        public int GetIdCustomer2(int id) {

            int id_pessoa = 0;
            try {

                conn.Open();

                string command = $"select id_pessoa from clientes " +
                                 $"where id_cliente = @id_cliente;";

                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.Add(new MySqlParameter("@id_cliente", id));
                cmd.Prepare();

                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    id_pessoa = int.Parse(reader["id_pessoa"].ToString());
                }

                conn.Close();
                return id_pessoa;

            }
            catch (MySqlException) {

                conn.Close();
                return id_pessoa;
            }

        }

        public string GetNameCustomer(int id) {

            string nome = string.Empty;

            try {

                conn.Open();

                string command = $"select nome_pessoa from pessoas " +
                                 $"where id_pessoa = @id_pessoa;";

                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.Add(new MySqlParameter("@id_pessoa", id));
                cmd.Prepare();

                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    nome = reader["nome_pessoa"].ToString();
                }

                conn.Close();

                return nome;
            }
            catch (MySqlException) {
                conn.Close();
                return nome;
            }
        }

        #endregion

    }
}
