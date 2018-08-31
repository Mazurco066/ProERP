using MySql.Data.MySqlClient;
using System.Data;
using Promig.Connection;
using Promig.Model;
using Promig.Model.CbModel;
using Promig.Utils;

namespace Promig.Connection.Methods {

    /*______________________________________________________________________
     |
     |                      CLASSE DE ACESSO A DADOS
     |
     |      Classe para inserir editar, excluir, inativar e recuperar todos
     |      os dados relacionados a items de orçamento do sistema do 
     |      banco de dados.
     |
     */
    class ItemEstimates {

        #region Header

        private MySqlConnection conn;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public ItemEstimates() { conn = ConnectionFactory.GetConnection(); }

        #endregion

        #region Data-Access

        /// <summary>
        /// Método para adicionar item em um orçamento
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(ItemEstimate item) {
            try {

                // Definindo comando de inserção
                string command = $"INSERT INTO {Refs.TABLE_ESTIMATE_SERVICES}" +
                                 $"(id_orcamento, id_servico, quantidade) " +
                                 $"VALUES(last_insert_id(), @id_service, @amount);";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos valores dos parametros
                cmd.Parameters.Add(new MySqlParameter("@id_service", item.Service.Id));
                cmd.Parameters.Add(new MySqlParameter("@amount", item.Amount));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

            } catch (MySqlException err) {
                Utils.Log.logException(err);
                Utils.Log.logMessage(err.Message);
            }
        }

        /// <summary>
        /// Método para editar item de orçamento
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        public void EditItem(ItemEstimate item, int id) {
            try {

                // Definindo comando de inserção
                string command = $"UPDATE {Refs.TABLE_ESTIMATE_SERVICES} SET " +
                                 $"(id_orcamento, id_servico, quantidade) " +
                                 $"VALUES(@id_estimate, @id_service, @amount);";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos valores dos parametros
                cmd.Parameters.Add(new MySqlParameter("@id_estimate", id));
                cmd.Parameters.Add(new MySqlParameter("@id_service", item.Service.Id));
                cmd.Parameters.Add(new MySqlParameter("@amount", item.Amount));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

            } catch (MySqlException err) {
                Utils.Log.logException(err);
                Utils.Log.logMessage(err.Message);
            }   
        }

        /// <summary>
        /// Metodo para deletar todos items de um orçamento
        /// </summary>
        /// <param name="id"></param>
        public void DeleteAllItems(int id) {
            try {

                // Definindo comando de inserção
                string command = $"DELETE FROM {Refs.TABLE_ESTIMATE_SERVICES} " +
                                 $"WHERE id_orcamento = @id_estimate;";

                // Definição do comando instanciado
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definição dos valores dos parametros
                cmd.Parameters.Add(new MySqlParameter("@id_estimate", id));

                // Preparando comando com os parametros
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();

            } catch (MySqlException err) {
                Utils.Log.logException(err);
                Utils.Log.logMessage(err.Message);
            }
        }

        #endregion
    }
}
