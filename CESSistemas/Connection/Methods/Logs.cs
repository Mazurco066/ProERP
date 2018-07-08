using System.Collections.Generic;
using Promig.Model;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Text;

namespace Promig.Connection.Methods {

    /*______________________________________________________________________
     |
     |                      CLASSE DE ACESSO A DADOS
     |
     |      Classe para inserir e recuperar dados do banco a respeito aos
     |      logs de sistema
     |
     */
    class Logs {

        #region header

        //Atributo de connexão
        private MySqlConnection conn;

        //Construtor para recuperar conexão
        public Logs() {

            //Recuperando conexão
            conn = ConnectionFactory.GetConnection();
        }

        #endregion header

        #region Data-Acess

        public void Register(Log log) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo string de inserção
                StringBuilder command = new StringBuilder();
                command.Append("INSERT INTO ").Append(ConnectionFactory.TABLE_LOGS);
                command.Append("(id_funcionario, log_date, user_action) ");
                command.Append("VALUES(@idEmp, @date, @action);");

                //Definindo objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command.ToString(), conn) {
                    CommandType = CommandType.Text
                };

                //Adicionando os parametros ao comando
                cmd.Parameters.Add(new MySqlParameter("@idEmp", log.employe.id));
                cmd.Parameters.Add(new MySqlParameter("@date", log.date));
                cmd.Parameters.Add(new MySqlParameter("@action", log.action));

                //Preparando inserção
                cmd.Prepare();

                //Executando inserção
                cmd.ExecuteNonQuery();

                //Fechando conexão com banco
                conn.Close();

            }
            catch (MySqlException) {

                //Retornando mensagem de erro
                conn.Close();
                MessageBox.Show("Erro ao registrar log no banco de dados!");

            }

        }

        public List<Log> GetAllRegisters(string date) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando de ecuperação dos dados
                string command = "select l.*, p.nome_pessoa from" +
                " pessoas p, funcionarios f, log l where p.id_pessoa = f.id_pessoa" +
                " and l.id_funcionario = f.id_funcionario and l.log_date = @date" +
                " order by l.log_date desc;";

                //Definindo objetos para recuperação de dados
                List<Log> results = new List<Log>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Adicionando os parametros e preparando consulta
                cmd.Parameters.Add(new MySqlParameter("@date", date));

                //Preparando consulta
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    //Adicionando log encontrado ao array de retorno
                    Log log = new Log();
                    log.id = ((int)reader["id_log"]);
                    log.date = ((string)reader["log_date"]);
                    log.action = ((string)reader["user_action"]);
                    log.employe = (new Employe());
                    log.employe.name = ((string)reader["nome_pessoa"]);
                    results.Add(log);

                }

                //Fechando conexão com banco
                conn.Close();

                return results;

            }
            catch (MySqlException) {

                //Fechando conexão e retornando nulo
                conn.Close();
                return null;

            }

        }

        public List<Log> GetAllRegisters(string date, string action) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando de ecuperação dos dados
                string command = "select l.*, p.nome_pessoa from" +
                " pessoas p, funcionarios f, log l where p.id_pessoa = f.id_pessoa" +
                " and l.id_funcionario = f.id_funcionario and l.log_date = @date" +
                " and l.user_action LIKE @action order by l.log_date desc;";

                //Definindo objetos para recuperaçãod e dados
                List<Log> results = new List<Log>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Adicionando os parametros e preparando consulta
                cmd.Parameters.Add(new MySqlParameter("@date", date));
                cmd.Parameters.Add(new MySqlParameter("@action", "%" + action + "%"));

                //Preparando consulta
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    //Adicionando log encontrado ao array de retorno
                    Log log = new Log();
                    log.id = ((int)reader["id_log"]);
                    log.date = ((string)reader["log_date"]);
                    log.action = ((string)reader["user_action"]);
                    log.employe = new Employe();
                    log.employe.name = ((string)reader["nome_pessoa"]);
                    results.Add(log);

                }

                //Fechando conexão com banco
                conn.Close();

                return results;

            }
            catch (MySqlException) {

                //Fechando conexão e retornando nulo
                conn.Close();
                return null;

            }

        }

        #endregion Data-Acess

    }
}
