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
    class Debits {

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
        private Supplier supplier;

        public Debits() {
            this.conn = ConnectionFactory.GetConnection();
            this.supplier = new Supplier();
        }
        #endregion

        #region Data-Access

        public void AddDebit(Debit debit) {

            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando de inserção
                string command = $"INSERT INTO {Refs.TABLE_DEBITS}" +
                                 $"(id_fornecedor, descricao, data_pagamento, " +
                                 $"data_vencimento, valor_total, valor_inicial) " +
                                 $"VALUES(@id_fornecedor, @descricao, @data_pagamento, @data_vencimento, " +
                                 $"@valor_total, @valor_inicial);";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos valores dos parametros
                cmd.Parameters.Add(new MySqlParameter("@id_fornecedor", debit.IdSupplier));
                cmd.Parameters.Add(new MySqlParameter("@descricao", debit.Description));
                cmd.Parameters.Add(new MySqlParameter("@data_pagamento", debit.PaymentDate));
                cmd.Parameters.Add(new MySqlParameter("@data_vencimento", debit.DueDate));
                cmd.Parameters.Add(new MySqlParameter("@valor_total", debit.TotalAmount));
                cmd.Parameters.Add(new MySqlParameter("@valor_inicial", debit.StartAmount));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

                // Mensagem de sucesso
                MessageBox.Show(
                    "Contas a Pagar Inserido!",
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

        public void EditDebit(Debit debit) {

            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando de edição
                string command = $"UPDATE {Refs.TABLE_DEBITS} SET " +
                                 $"id_fornecedor = @id_fornecedor, descricao = @descricao, " +
                                 $"data_pagamento = @data_pagamento, data_vencimento = @data_vencimento, " +
                                 $"valor_total = @valor_total, valor_inicial = @valor_inicial " +
                                 $"WHERE id_debito = @id_debito;";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos valores dos parametros
                cmd.Parameters.Add(new MySqlParameter("@id_debito", debit.IdDebit));
                cmd.Parameters.Add(new MySqlParameter("@id_fornecedor", debit.IdSupplier));
                cmd.Parameters.Add(new MySqlParameter("@descricao", debit.Description));
                cmd.Parameters.Add(new MySqlParameter("@data_pagamento", debit.PaymentDate));
                cmd.Parameters.Add(new MySqlParameter("@data_vencimento", debit.DueDate));
                cmd.Parameters.Add(new MySqlParameter("@valor_total", debit.TotalAmount));
                cmd.Parameters.Add(new MySqlParameter("@valor_inicial", debit.StartAmount));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

                // Mensagem de sucesso
                MessageBox.Show(
                    "Contas a Pagar Alterado!",
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

        public void DeleteDebit(int id_debit) {

            try {

                // Abertura da conexão com banco
                conn.Open();

                // Definição do comando de exclusão
                string command = $"DELETE FROM {Refs.TABLE_DEBITS} " +
                                 $"WHERE id_debito = @id_debito;";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos parametros do comando
                cmd.Parameters.Add(new MySqlParameter("@id_debito", id_debit));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

                // Mensagem de sucesso
                MessageBox.Show(
                    "Contas a Pagar Deletada!",
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

        public Debit GetDebitData(int id_debit) {

            try {

                // Abertura da conexão com banco
                conn.Open();

                // Definição do comando sql
                string command = $"SELECT id_debito, id_fornecedor, descricao, data_pagamento, " +
                                 $"data_vencimento, valor_total, valor_inicial " +
                                 $"FROM {Refs.TABLE_DEBITS} " +
                                 $"WHERE id_debito = @id_debito;";

                // Definição do comando instanciado
                Debit result = new Debit();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Adicionando parametros a busca
                cmd.Parameters.Add(new MySqlParameter("@id_debito", id_debit));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Realizando busca no banco
                reader = cmd.ExecuteReader();

                // Recuperando dados do orçamento
                while (reader.Read()) {
                    result.IdDebit = int.Parse(reader["id_debito"].ToString());
                    result.IdSupplier = int.Parse(reader["id_fornecedor"].ToString());
                    result.Description = reader["descricao"].ToString();
                    result.PaymentDate = reader["data_pagamento"].ToString();
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

        public List<Debit> GetAllDebit(string param) {


            try {

                // Abertura da conexão com o banco
                conn.Open();

                // Definição do comando de consulta
                string command = $"SELECT id_debito, id_fornecedor, descricao, data_pagamento, " +
                                 $"data_vencimento, valor_total, valor_inicial " +
                                 $"FROM {Refs.TABLE_DEBITS} " +
                                 $"WHERE descricao LIKE @param " +
                                 $"order by descricao;";

                // Definição do comando instanciado
                List<Debit> results = new List<Debit>();
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
                    Debit debit = new Debit();
                    debit.IdDebit = int.Parse(reader["id_debito"].ToString());
                    debit.Description = reader["descricao"].ToString();
                    debit.TotalAmount = double.Parse(reader["valor_total"].ToString());
                    debit.DueDate = reader["data_vencimento"].ToString();
                    results.Add(debit);
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

        public int GetIdSupplier(int id) {

            int id_fornecedor = 0;
            try {

                conn.Open();

                string command = $"select id_fornecedor from fornecedores " +
                                 $"where id_pessoa = @id_pessoa;";

                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.Add(new MySqlParameter("@id_pessoa", id));
                cmd.Prepare();

                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    id_fornecedor = int.Parse(reader["id_fornecedor"].ToString());
                }

                conn.Close();
                return id_fornecedor;

            }
            catch (MySqlException) {

                conn.Close();
                return id_fornecedor;
            }
        }

        public int GetIdPessoaSuppliier(string name) {

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

        public int GetIdSupplier2(int id) {

            int id_pessoa = 0;
            try {

                conn.Open();

                string command = $"select id_pessoa from fornecedores " +
                                 $"where id_fornecedor = @id_fornecedor;";

                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.Add(new MySqlParameter("@id_fornecedor", id));
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

        public string GetNameSupplier(int id) {

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
