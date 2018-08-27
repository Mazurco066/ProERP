using System.Collections.Generic;
using Promig.Model;
using Promig.Exceptions;
using MySql.Data.MySqlClient;
using System.Data;

namespace Promig.Connection.Methods {
    
    /*______________________________________________________________________
     |
     |                      CLASSE DE ACESSO A DADOS
     |
     |      Classe para inserir editar, excluir, inativar e recuperar todos
     |      os dados relacionados a serviços do sistema do banco de dados.
     |
     */
    class Services {

        #region Header

        private MySqlConnection conn;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public Services() { conn = ConnectionFactory.GetConnection(); }

        #endregion

        #region data-access

        /// <summary>
        /// Método para adicionar serviço ao banco de dados
        /// </summary>
        /// <param name="service"></param>
        public void AddService(Service service) {
            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando sql
                string command = $"INSERT INTO {Refs.TABLE_SERVICES}" +
                                 $"(descricao, valor_unitario) " +
                                 $"VALUES(@description, @value)";

                // Definição de objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@description", service.Task));
                cmd.Parameters.Add(new MySqlParameter("@value", service.Value));

                // Preparação do comando para realização do insert
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();
                conn.Close();

            } catch(MySqlException) {
                conn.Close();
                throw new DatabaseInsertException();
            }
        }

        /// <summary>
        /// Método para editsr serviço no banco de dados
        /// </summary>
        /// <param name="service"></param>
        public void EditService(Service service) {
            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando sql
                string command = $"UPDATE {Refs.TABLE_SERVICES} SET " +
                                 $"descricao = @description, valor_unitario = @value " +
                                 $"WHERE id_servico = @id";

                // Definição de objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@id", service.Id));
                cmd.Parameters.Add(new MySqlParameter("@description", service.Task));
                cmd.Parameters.Add(new MySqlParameter("@value", service.Value));

                // Preparação do comando para realização do insert
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();
                conn.Close();

            } catch (MySqlException) {
                conn.Close();
                throw new DatabaseEditException();
            }
        }

        /// <summary>
        /// Método para delatar serviço no banco de dados
        /// </summary>
        /// <param name="service"></param>
        public void DeleteService(Service service) {
            try {

                // Abertura da conexão com o banco de dados
                conn.Open();

                // Definição do comando sql
                string command = $"DELETE FROM {Refs.TABLE_SERVICES} " +
                                 $"WHERE id_servico = @id";

                // Definição de objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@id", service.Id));

                // Preparação do comando para realização do insert
                cmd.Prepare();

                // Executando inserção
                cmd.ExecuteNonQuery();
                conn.Close();

            } catch (MySqlException) {
                conn.Close();
                throw new DatabaseDeleteException();
            }
        }

        /// <summary>
        /// Método para recuperar dados de um serviço no banco
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Service GetServiceData(int id) {
            try {

                // Abertura de conexão com banco de dados
                conn.Open();

                // Definição do comando sql
                string command = $"SELECT * FROM {Refs.TABLE_SERVICES} " +
                                 $"WHERE id_servico = @id";

                // Definindo comando e resultados
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Adição dos parametros ao comando
                cmd.Parameters.Add(new MySqlParameter("@id",id));

                // Preparo do comando para sua execução
                cmd.Prepare();

                // Realização da consulta
                Service _return = new Service();
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    _return.Id = (int)reader["id_servico"];
                    _return.Task = reader["descricao"].ToString();
                    _return.Value = (double)reader["valor_unitario"];
                }

                // Retorno dos dados encontrados
                conn.Close();
                return _return;

            } catch (MySqlException) {
                conn.Close();
                throw new DatabaseAccessException();
            }
        }

        /// <summary>
        /// Método para recuperar todos serviços presentes no banco de dados
        /// no formato de lista
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Service> GetAllServices(string param) {
            try {

                // Abertura de conexão com banco de dados
                conn.Open();

                // Definição do comando sql
                string command = $"SELECT * FROM {Refs.TABLE_SERVICES} " +
                                 $"WHERE descricao LIKE @param";

                // Definindo comando e resultados
                List<Service> services = new List<Service>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                // Adição dos parametros ao comando
                cmd.Parameters.Add(new MySqlParameter("@param", $"%{param}%"));

                // Preparo do comando para sua execução
                cmd.Prepare();

                // Realização da consulta
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Service service = new Service();
                    service.Id = (int)reader["id_servico"];
                    service.Task = reader["descricao"].ToString();
                    service.Value = (double)reader["valor_unitario"];
                    services.Add(service);
                }

                // Retorno dos dados encontrados
                conn.Close();
                return services;

            } catch (MySqlException) {
                conn.Close();
                throw new DatabaseAccessException();
            }
        }

        #endregion
    }
}
