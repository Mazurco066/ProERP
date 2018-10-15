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
    |      os dados relacionados a pedidos de venda do sistema do 
    |      banco de dados.
    |
   */
    class SaleOrders {

        #region Header

        //Atributos de conexão
        private MySqlConnection conn;

        public SaleOrders(){
            conn = ConnectionFactory.GetConnection();
        }

        #endregion

        #region Data-Access

        public void AddSaleOrder(SaleOrder saleOrder) {
            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando sql
                string command = $"INSERT INTO {Refs.TABLE_SALE_ORDER}" +
                                 $"(no_orcamento, data_realizacao, situacao, desconto, vl_total_desconto) " +
                                 $"VALUES(@no_orcamento, @data_realizacao, @situacao, @desconto, @vl_total_desconto)";

                // Definição de objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@no_orcamento", saleOrder.No_estimate));
                cmd.Parameters.Add(new MySqlParameter("@data_realizacao", saleOrder.Date_realization));
                cmd.Parameters.Add(new MySqlParameter("@situacao", saleOrder.Situation));
                cmd.Parameters.Add(new MySqlParameter("@desconto", saleOrder.Discount));
                cmd.Parameters.Add(new MySqlParameter("@vl_total_desconto", saleOrder.TotalDiscount));

                // Preparação do comando para realização do insert
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();
                conn.Close();

            }
            catch (MySqlException) {
                conn.Close();
                throw new DatabaseInsertException();
            }
        }

        public void EditSaleOrder(SaleOrder saleOrder) {
            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando sql
                string command = $"UPDATE {Refs.TABLE_SALE_ORDER} SET " +
                                 $"no_orcamento = @no_orcamento, data_realizacao = @data_realizacao, " +
                                 $"situacao = @situacao, desconto = @desconto, vl_total_desconto = @vl_total_desconto " +
                                 $"WHERE no_venda = @id";

                // Definição de objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@id", saleOrder.No_saleOrder));
                cmd.Parameters.Add(new MySqlParameter("@no_orcamento", saleOrder.No_estimate));
                cmd.Parameters.Add(new MySqlParameter("@data_realizacao", saleOrder.Date_realization));
                cmd.Parameters.Add(new MySqlParameter("@situacao", saleOrder.Situation));
                cmd.Parameters.Add(new MySqlParameter("@desconto", saleOrder.Discount));
                cmd.Parameters.Add(new MySqlParameter("@vl_total_desconto", saleOrder.TotalDiscount));

                // Preparação do comando para realização do insert
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();
                conn.Close();

            }
            catch (MySqlException) {
                conn.Close();
                throw new DatabaseEditException();
            }
        }

        public void DeleteSaleOrder(SaleOrder saleOrder) {
            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando sql
                string command = $"DELETE FROM {Refs.TABLE_SALE_ORDER} " +
                                 $"WHERE no_venda = @id";

                // Definição de objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@id", saleOrder.No_saleOrder));

                // Preparação do comando para realização do insert
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();
                conn.Close();

            }
            catch (MySqlException) {
                conn.Close();
                throw new DatabaseDeleteException();
            }
        }

        public List<SaleOrder> GetAllSaleOrder(){
            try {

                // Abertura da conexão com o banco
                conn.Open();

                // Definição do comando de consulta
                string command = $"SELECT no_venda, no_orcamento, data_realizacao, situacao, desconto, vl_total_desconto " +
                                 $"FROM {Refs.TABLE_SALE_ORDER}";

                // Definição do comando instanciado
                List<SaleOrder> results = new List<SaleOrder>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Preparando comando com os parametros
                cmd.Prepare();

                // Realizando busca no banco
                reader = cmd.ExecuteReader();

                // Verificando resultados
                while (reader.Read()) {
                    SaleOrder sl = new SaleOrder();
                    sl.No_saleOrder = (int)reader["no_venda"];
                    sl.No_estimate = (int)reader["no_orcamento"];
                    sl.Date_realization = reader["data_realizacao"].ToString();
                    sl.Situation = reader["situacao"].ToString();
                    sl.Discount = (double)reader["desconto"];
                    sl.TotalDiscount = (double)reader["vl_total_desconto"];
                    results.Add(sl);
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

        #endregion

    }
}
