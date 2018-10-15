using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using Promig.Connection;
using Promig.Exceptions;
using Promig.Model;
using Promig.Model.CbModel;
using Promig.View.Components;

namespace Promig.Connection.Methods {

    /*______________________________________________________________________
     |
     |                      CLASSE DE ACESSO A DADOS
     |
     |      Classe para inserir editar, excluir, inativar e recuperar todos
     |      os dados relacionados a items de orçamentos do sistema do 
     |      banco de dados.
     |
     */
    class Estimates {

        #region header

        //Atributos de conexão
        private MySqlConnection conn;
        private ItemEstimates items;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public Estimates() {
            conn = ConnectionFactory.GetConnection();
            items = new ItemEstimates();
        }

        #endregion header

        #region Data-Access

        /// <summary>
        /// Metodo para adicionar orçamento ao banco de dados
        /// </summary>
        /// <param name="estimate">Orçamento a ser inserido no banco</param>
        public void AddEstimate(Estimate estimate) {

            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando de inserção
                string command = $"INSERT INTO {Refs.TABLE_ESTIMATES}" +
                                 $"(id_cliente, data_orcamento, caminho_imagem, " +
                                 $"descricao, condicao_pagto, execucao_dias, valor_total) " +
                                 $"VALUES(@id_cli, @estimate_date, @img_path, @description, " +
                                 $"@payment, @days, @total_value);";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos valores dos parametros
                cmd.Parameters.Add(new MySqlParameter("@id_cli", estimate.IdCustomer));
                cmd.Parameters.Add(new MySqlParameter("@estimate_date", estimate.Date));
                cmd.Parameters.Add(new MySqlParameter("@img_path", estimate.ImgPath));
                cmd.Parameters.Add(new MySqlParameter("@description", estimate.Description));
                cmd.Parameters.Add(new MySqlParameter("@payment", estimate.PayCondition));
                cmd.Parameters.Add(new MySqlParameter("@days", estimate.DaysExecution));
                cmd.Parameters.Add(new MySqlParameter("@total_value", estimate.TotalValue));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

                // Adição dos items do orçamento
                foreach (ItemEstimate item in estimate.Items) {
                    items.AddItem(item);
                }

                // Fechando conexão com banco
                conn.Close();

                // Mensagem de sucesso
                MessageBox.Show(
                    "Orçamento Inserido!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

            } catch (MySqlException err) {

                // Fechando conexão com banco e disparando exceção
                MessageBox.Show(err.Message);
                conn.Close();
                throw new DatabaseInsertException();
            }
        }

        /// <summary>
        /// Metodo para alterar orçamento no banco de dados
        /// </summary>
        /// <param name="estimate">Orçamento a ser alterado no banco</param>
        public void EditEstimate(Estimate estimate) {

            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando de edição
                string command = $"UPDATE {Refs.TABLE_ESTIMATES} SET " +
                                 $"id_cliente = @id_cli, data_orcamento = @estimate_date, " +
                                 $"caminho_imagem = @img_path, descricao = @description, " +
                                 $"condicao_pagto = @payment, execucao_dias = @days, " +
                                 $"valor_total = @total_value " +
                                 $"WHERE no_documento = @id_doc;";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos valores dos parametros
                cmd.Parameters.Add(new MySqlParameter("@id_doc", estimate.DocNo));
                cmd.Parameters.Add(new MySqlParameter("@id_cli", estimate.IdCustomer));
                cmd.Parameters.Add(new MySqlParameter("@estimate_date", estimate.Date));
                cmd.Parameters.Add(new MySqlParameter("@img_path", estimate.ImgPath));
                cmd.Parameters.Add(new MySqlParameter("@description", estimate.Description));
                cmd.Parameters.Add(new MySqlParameter("@payment", estimate.PayCondition));
                cmd.Parameters.Add(new MySqlParameter("@days", estimate.DaysExecution));
                cmd.Parameters.Add(new MySqlParameter("@total_value", estimate.TotalValue));

                // Remoção de todos os items atuais do orçamento
                items.DeleteAllItems(estimate.DocNo);

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

                // Adição dos items do orçamento
                foreach (ItemEstimate item in estimate.Items) {
                    items.EditItem(item, estimate.DocNo);
                }

                // Mensagem de sucesso
                MessageBox.Show(
                    "Orçamento Alterado!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Fechamento da conexão
                conn.Close();

            } catch (MySqlException) {

                // Fechando conexão com banco de disparando exceção
                conn.Close();
                throw new DatabaseEditException();
            }
        }

        public void DeleteEstimate(int doc_no) {
            try {

                // Abertura da conexão com banco
                conn.Open();

                // Definição do comando de exclusão
                string command = $"DELETE FROM {Refs.TABLE_ESTIMATES} " +
                                 $"WHERE no_documento = @noDoc;";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos parametros do comando
                cmd.Parameters.Add(new MySqlParameter("@noDoc", doc_no));

                // Remoção de todos os items referentes ao orçamento
                items.DeleteAllItems(doc_no);

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

                // Mensagem de sucesso
                MessageBox.Show(
                    "Orçamento Deletado!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Fechamento da conexão com banco
                conn.Close();

            } catch (MySqlException) {

                // Fechamento da conexão
                conn.Close();
                throw new DatabaseDeleteException();
            }
        }

        public Estimate GetEstimateData(int doc_no) {
            try {

                // Abertura da conexão com banco
                conn.Open();

                // Definição do comando sql
                string command = $"SELECT o.no_documento, o.descricao, p.nome_pessoa, o.data_orcamento, " +
                                 $"o.caminho_imagem, o.condicao_pagto, o.id_cliente, o.execucao_dias, o.valor_total " +
                                 $"FROM {Refs.TABLE_ESTIMATES} o, {Refs.TABLE_CLIENTS} c, {Refs.TABLE_PEOPLE} p " +
                                 $"WHERE o.id_cliente = c.id_cliente " +
                                 $"AND c.id_pessoa = p.id_pessoa " +
                                 $"AND o.no_documento = @docNo;";

                // Definição do comando instanciado
                Estimate result = new Estimate();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Adicionando parametros a busca
                cmd.Parameters.Add(new MySqlParameter("@docNo", doc_no));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Realizando busca no banco
                reader = cmd.ExecuteReader();

                // Recuperando dados do orçamento
                while (reader.Read()) {
                    result.DocNo = (int)reader["no_documento"];
                    result.IdCustomer = (int)reader["id_cliente"];
                    result.NameCustomer = reader["nome_pessoa"].ToString();
                    result.ImgPath = reader["caminho_imagem"].ToString();
                    result.Description = reader["descricao"].ToString();
                    result.PayCondition = reader["condicao_pagto"].ToString();
                    result.DaysExecution = reader["execucao_dias"].ToString();
                    result.Date = reader["data_orcamento"].ToString();
                    result.TotalValue = (double)reader["valor_total"];
                } reader.Close();

                // Recuperando serviços do orçamento
                result.Items = items.GetAllItems(doc_no);

                // Fechamento da conexão e retorno
                conn.Close();
                return result;

            } catch (MySqlException) {

                // Fechando conexão com banco de disparando exceção
                conn.Close();
                throw new DatabaseAccessException();
            }
        }

        /// <summary>
        /// Metodo para retornar todos os orçamentos
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Estimate> GetAllEstimates(string param, UserControlSalesOrder window) {

            try {

                // Abertura da conexão com o banco
                conn.Open();

                // Definição do comando de consulta
                string command = $"SELECT o.no_documento, o.descricao, p.nome_pessoa, o.data_orcamento, o.valor_total " +
                                 $"FROM {Refs.TABLE_ESTIMATES} o, {Refs.TABLE_CLIENTS} c, {Refs.TABLE_PEOPLE} p " +
                                 $"WHERE o.id_cliente = c.id_cliente " +
                                 $"AND c.id_pessoa = p.id_pessoa " +
                                 $"AND (p.nome_pessoa LIKE @paramSearch " +
                                 $"OR o.descricao LIKE @paramSearch " +
                                 $"OR o.data_orcamento LIKE @paramSearch);";

                // Definição do comando instanciado
                List<Estimate> results = new List<Estimate>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Adicionando parametros a busca
                cmd.Parameters.Add(new MySqlParameter("@paramSearch", $"%{param}%"));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Realizando busca no banco
                reader = cmd.ExecuteReader();

                // Verificando resultados
                while (reader.Read()) {
                    Estimate estimate = new Estimate();
                    estimate.DocNo = (int)reader["no_documento"];
                    estimate.NameCustomer = reader["nome_pessoa"].ToString();
                    estimate.Description = reader["descricao"].ToString();
                    estimate.Date = reader["data_orcamento"].ToString();
                    if(window != null){
                        estimate.TotalValue2 = (double)reader["valor_total"];
                    }
                    results.Add(estimate);
                }

                // Fechamento da conexão
                conn.Close();
                return results;

            } catch (MySqlException) {

                // Fechando conexão com banco de disparando exceção
                conn.Close();
                throw new DatabaseAccessException();
            }
        }

        public List<Estimate> GetAllEstimatesSaleOrder(int param) {

            try {

                // Abertura da conexão com o banco
                conn.Open();

                // Definição do comando de consulta
                string command = $"SELECT o.no_documento, o.descricao, p.nome_pessoa, o.data_orcamento, o.valor_total " +
                                 $"FROM {Refs.TABLE_ESTIMATES} o, {Refs.TABLE_PEOPLE} p, {Refs.TABLE_CLIENTS} c " +
                                 $"WHERE o.id_cliente = c.id_cliente " +
                                 $"AND c.id_pessoa = p.id_pessoa " +
                                 $"AND o.no_documento = @param";

                // Definição do comando instanciado
                List<Estimate> results = new List<Estimate>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Adicionando parametros a busca
                cmd.Parameters.Add(new MySqlParameter("@param", param));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Realizando busca no banco
                reader = cmd.ExecuteReader();

                // Verificando resultados
                while (reader.Read()) {
                    Estimate estimate = new Estimate();
                    estimate.DocNo = (int)reader["no_documento"];
                    estimate.NameCustomer = reader["nome_pessoa"].ToString();
                    estimate.Description = reader["descricao"].ToString();
                    estimate.Date = reader["data_orcamento"].ToString();
                    estimate.TotalValue2 = (double)reader["valor_total"];
                    results.Add(estimate);
                }

                // Fechamento da conexão
                conn.Close();
                return results;

            }
            catch (MySqlException) {

                // Fechando conexão com banco de disparando exceção
                conn.Close();
                throw new DatabaseAccessException();
            }
        }

        /// <summary>
        /// Metodo para retornar relação de nomes de todos clientes com seus 
        /// respecgivos ID's
        /// </summary>
        /// <returns>Retorna uma lista no modelo Customer com nomes e ids de clientes
        /// presentes no banco de dados</returns>
        public List<Customer> NameCustomerList() {

            try {

                // Abertura da conexão com o banco
                conn.Open();

                // Definindo comando de query
                string command = $"select c.id_cliente, p.nome_pessoa from " +
                                 $"{Refs.TABLE_PEOPLE} p, " +
                                 $"{Refs.TABLE_CLIENTS} c " +
                                 $"where c.id_pessoa = p.id_pessoa and " +
                                 $"p.status = @status";

                // Definindo comando e resultados
                MySqlDataReader reader;
                List<Customer> names = new List<Customer>();
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definindo parametros de busca
                cmd.Parameters.Add(new MySqlParameter("@status", true));
                cmd.Prepare();

                // Realização da consulta
                reader = cmd.ExecuteReader();

                // Preenchendo lista de nomes
                while (reader.Read()) {
                    names.Add(new Customer(
                        (int)reader["id_cliente"],
                        reader["nome_pessoa"].ToString()
                    ));
                }

                // Fechamento da conexão
                conn.Close();

                // Retorno da lista de nomes
                return names;

            } catch (MySqlException) {

                // Fechando conexão e retrnando mensagem de erro
                conn.Close();
                MessageBox.Show(
                    "Erro ao tentar se conectar ao banco! Tente novamente mais tarde...",
                    "Erro de Conexão!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return new List<Customer>();
            }
        }

        #endregion
    }
}