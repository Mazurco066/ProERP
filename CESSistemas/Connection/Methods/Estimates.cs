using System;
using System.Collections.Generic;
using System.Linq;
using Promig.Model;
using Promig.Exceptions;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace Promig.Connection.Methods {


    /*______________________________________________________________________
    |
    |                      CLASSE DE ACESSO A DADOS
    |
    |      Classe para inserir editar, excluir, inativar e recuperar todos
    |      os dados relacionados a orçamentos do sistema do banco de dados.
    |
    */


    class Estimates {

        #region Header

        // Atributo de conexão
        private MySqlConnection conn;

        public Estimates() {
            conn = ConnectionFactory.GetConnection();
        }

        #endregion

        #region Data-Acess
        public void AddEstimate(Estimate estimate) {
            try {

                // abrindo a conexão
                conn.Open();
                // comando para insert no banco
                string sql = string.Format("INSERT INTO orcamentos" +
                                           "(id_cliente, cliente, data_orcamento, caminho_imagem, descricao," +
                                           "condicao_pagto, execucao_dias, valor_total, descricao1, descricao2," +
                                           "descricao3, descricao4, descricao5, descricao6, descricao7) VALUES" +
                                           "({0}, '{1}', str_to_date('{2}', \"%d/%m/%Y %H:%i:%s\"), '{3}', '{4}', '{5}', '{6}', {7}, '{8}', '{9}'," +
                                           "'{10}', '{11}', '{12}', '{13}', '{14}');COMMIT;", estimate.IdCustomer,
                                           estimate.Customer, estimate.Date, estimate.ImgPath, estimate.Description,
                                           estimate.PayCondition, estimate.DaysExecution, estimate.TotalValue,
                                           estimate.Description1, estimate.Description2, estimate.Description3,
                                           estimate.Description4, estimate.Description5, estimate.Description6,
                                           estimate.Description7);

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Orçamento inserido!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            } catch (MySqlException) {
                // fecha conexão retornando um erro
                conn.Close();
                throw new DatabaseInsertException();
            } finally {
                // sempre fecha a conexão
                conn.Close();
            }
        }

        public void EditEstimate(Estimate estimate) {
            try {
                // abrindo a conexão
                conn.Open();
                // comando para insert no banco
                string sql = string.Format("UPDATE orcamentos SET " +
                                           "cliente = '{0}', data_orcamento = str_to_date('{1}', \"%d/%m/%Y %H:%i:%s\"), caminho_imagem = '{2}', descricao = '{3}', " +
                                           "condicao_pagto = '{4}', execucao_dias = '{5}', valor_total = {6}, descricao1 = '{7}', " +
                                           "descricao2 = '{8}', descricao3 = '{9}', descricao4 = '{10}', descricao5 = '{11}', " +
                                           "descricao6 = '{12}', descricao7 = '{13}' WHERE no_documento = {14};COMMIT;",
                                           estimate.Customer, estimate.Date, estimate.ImgPath, estimate.Description, estimate.PayCondition,
                                           estimate.DaysExecution, estimate.TotalValue, estimate.Description1, estimate.Description2,
                                           estimate.Description3, estimate.Description4, estimate.Description5, estimate.Description6,
                                           estimate.Description7, estimate.DocNo);

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Orçamento atualizado!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            } catch (MySqlException) {
                // fecha conexão retornando um erro
                conn.Close();
                throw new DatabaseEditException();
            } finally {
                // sempre fecha a conexão
                conn.Close();
            }
        }

        public void DeleteEstimate(int docno) {
            try {
                conn.Open();

                string sql = string.Format("delete from orcamentos where no_documento = {0}", docno);

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                BackAuto_Increment();

                MessageBox.Show("Registro excluido com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            } catch (MySqlException ex) {
                conn.Close();
                MessageBox.Show("Erro ao excluir registro" + ex.Message, "Erro",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Estimate GetEstimateData(int docno) {
            try {
                conn.Open();

                string sql = string.Format("select no_documento, id_cliente, cliente, data_orcamento," +
                                           "caminho_imagem, descricao, condicao_pagto, execucao_dias," +
                                           "valor_total, descricao1, descricao2, descricao3, descricao4," +
                                           "descricao5, descricao6, descricao7 from orcamentos where no_documento = {0}", docno);


                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                Estimate estimate = new Estimate();
                while (reader.Read()) {
                    estimate.DocNo = int.Parse(reader["no_documento"].ToString());
                    estimate.IdCustomer = int.Parse(reader["id_cliente"].ToString());
                    estimate.Customer = reader["cliente"].ToString();
                    estimate.Date = DateTime.Parse(DateTime.Parse(reader["data_orcamento"].ToString()).ToShortDateString());
                    estimate.ImgPath = reader["caminho_imagem"].ToString();
                    estimate.Description = reader["descricao"].ToString();
                    estimate.PayCondition = reader["condicao_pagto"].ToString();
                    estimate.TotalValue = double.Parse(reader["valor_total"].ToString());
                    estimate.Description1 = reader["descricao1"].ToString();
                    estimate.Description2 = reader["descricao2"].ToString();
                    estimate.Description3 = reader["descricao3"].ToString();
                    estimate.Description4 = reader["descricao4"].ToString();
                    estimate.Description5 = reader["descricao5"].ToString();
                    estimate.Description6 = reader["descricao6"].ToString();
                    estimate.Description7 = reader["descricao7"].ToString();

                }

                return estimate;
            } catch (MySqlException) {
                //Fechando conexão e retrnando mensagem de erro
                conn.Close();
                MessageBox.Show(
                    "Erro ao tentar se conectar ao banco!", "Erro de Conexão!", MessageBoxButton.OK, MessageBoxImage.Error);
                return new Estimate();
            } finally {
                conn.Close();
            }
        }

        public static List<String> NameCustomerList() {
            try {
                MySqlConnection connection = ConnectionFactory.GetConnection();
                connection.Open();
                List<String> namesCustomer = new List<String>();
                string sql = string.Format("select p.nome_pessoa from pessoas p, clientes c where c.id_pessoa = p.id_pessoa and p.status = true and c.id_cliente > 0");
                MySqlCommand cmd = new MySqlCommand(sql, connection) {
                    CommandType = CommandType.Text
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Client customer = new Client();
                    customer.name = reader["nome_pessoa"].ToString();
                    namesCustomer.Add(customer.name);
                }
                connection.Close();
                return namesCustomer.OrderBy(s => s).ToList();
            } catch (MySqlException) {
                throw;
            }
        }

        public List<Estimate> GetPartialEstimateList() {
            try {

                conn.Open();
                List<Estimate> data = new List<Estimate>();
                string sql = string.Format(@"select no_documento, cliente, data_orcamento, condicao_pagto, execucao_dias, valor_total from orcamentos");
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Estimate est = new Estimate();
                    est.DocNo = int.Parse(reader["no_documento"].ToString());
                    est.Customer = reader["cliente"].ToString();
                    est.Date = DateTime.Parse(DateTime.Parse(reader["data_orcamento"].ToString()).ToShortDateString());
                    est.PayCondition = reader["condicao_pagto"].ToString();
                    est.DaysExecution = reader["execucao_dias"].ToString();
                    est.TotalValue = Convert.ToDouble(reader["valor_total"].ToString());
                    data.Add(est);
                }
                conn.Close();
                return data;
            } catch (MySqlException) {

                throw;
            }
        }

        public List<Estimate> GetAllEstimate(int param) {
            try {
                conn.Open();
                string sql = string.Format("select no_documento, cliente, data_orcamento, condicao_pagto, execucao_dias, valor_total from orcamentos where no_documento = {0}", param);
                List<Estimate> result = new List<Estimate>();
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Estimate estimate = new Estimate();
                    estimate.DocNo = int.Parse(reader["no_documento"].ToString());
                    estimate.Customer = reader["cliente"].ToString();
                    estimate.Date = DateTime.Parse(DateTime.Parse(reader["data_orcamento"].ToString()).ToShortDateString());
                    estimate.PayCondition = reader["condicao_pagto"].ToString();
                    estimate.DaysExecution = reader["execucao_dias"].ToString();
                    estimate.TotalValue = double.Parse(reader["valor_total"].ToString());
                    result.Add(estimate);
                }

                return result;
            } catch (MySqlException) {

                throw;
            } finally {
                conn.Close();
            }
        }

        private void BackAuto_Increment() {
            try {
                int docno = MaxDocNo();

                conn.Open();

                if (docno > 0) {
                    string sql = string.Format("alter table orcamentos AUTO_INCREMENT = {0};", docno);
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            } catch (MySqlException) {
                conn.Close();
                throw;
            } finally {
                conn.Close();
            }
        }

        private int MaxDocNo() {
            try {
                conn.Open();

                string sql = "select max(no_documento) from orcamentos;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                int docno = 0;
                while (reader.Read()) {
                    docno = int.Parse(reader["max(no_documento)"].ToString());
                }
                conn.Close();
                return docno;
            } catch (MySqlException) {
                conn.Close();
                throw;
            }

        }
        #endregion

    }
}