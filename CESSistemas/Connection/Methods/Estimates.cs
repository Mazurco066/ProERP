using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using Promig.Connection;
using Promig.Exceptions;
using Promig.Model;
using Promig.Model.CbModel;

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
                cmd.Parameters.Add(new MySqlParameter("@id_cli", estimate.idCustomer));
                cmd.Parameters.Add(new MySqlParameter("@estimate_date", estimate.date));
                cmd.Parameters.Add(new MySqlParameter("@img_path", estimate.imgPath));
                cmd.Parameters.Add(new MySqlParameter("@description", estimate.description));
                cmd.Parameters.Add(new MySqlParameter("@payment", estimate.payCondition));
                cmd.Parameters.Add(new MySqlParameter("@days", estimate.daysExecution));
                cmd.Parameters.Add(new MySqlParameter("@total_value", estimate.totalValue));

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
                string command = $"UPDATE {Refs.TABLE_ESTIMATES}SET " +
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
                cmd.Parameters.Add(new MySqlParameter("@id_doc", estimate.docNo));
                cmd.Parameters.Add(new MySqlParameter("@id_cli", estimate.idCustomer));
                cmd.Parameters.Add(new MySqlParameter("@estimate_date", estimate.date));
                cmd.Parameters.Add(new MySqlParameter("@img_path", estimate.imgPath));
                cmd.Parameters.Add(new MySqlParameter("@description", estimate.description));
                cmd.Parameters.Add(new MySqlParameter("@payment", estimate.payCondition));
                cmd.Parameters.Add(new MySqlParameter("@days", estimate.daysExecution));
                cmd.Parameters.Add(new MySqlParameter("@total_value", estimate.totalValue));

                // Remoção de todos os items atuais do orçamento
                items.DeleteAllItems(estimate.docNo);

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

                // Adição dos items do orçamento
                foreach (ItemEstimate item in estimate.Items) {
                    items.AddItem(item);
                }

                // Fechamento da conexão
                conn.Close();

            } catch (MySqlException) {

                // Fechando conexão com banco de disparando exceção
                conn.Close();
                throw new DatabaseEditException();
            }
        }

        /// <summary>
        /// Metodo para retornar todos os orçamentos
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Estimate> GetAllEstimates(string param) {

            try {

                // Abertura da conexão com o banco
                conn.Open();

                // Definição do comando de consulta
                string command = $"SELECT e.no_documento, p.nome_pessoa, e.data_orcamento  " +
                                 $"FROM {Refs.TABLE_ESTIMATES} e, {Refs.TABLE_CLIENTS} c, {Refs.TABLE_PEOPLE} p " +
                                 $"WHERE e.id_cliente = c.id_cliente" +
                                 $"AND c.id_pessoa = p.id_pessoa";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

                // Fechamento da conexão
                conn.Close();

            } catch (MySqlException) {
                conn.Close();
            }

            return new List<Estimate>();
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