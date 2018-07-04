using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using Promig.Exceptions;
using Promig.Model;

namespace Promig.Connection.Methods {

    class Users {

        #region header

        //Atributo de connexão
        private SqlConnection conn;

        //Construtor para recuperar conexão
        public Users() {

            //Recuperando conexão
            conn = ConnectionFactory.GetConnection();
        }

        #endregion header

        /**
         * Método para efetuar login do usuário
         */
        public void ValidateLogin(User user) {

            try {

                //Abrindo conexão com o banco
                conn.Open();

                //Definindo comando sql de consulta que sera executado
                string command = "select * from usuarios u, funcionarios f, pessoas p " +
                                 "where f.id_funcionario = u.id_funcionario and " +
                                 "f.id_pessoa = p.id_pessoa and u.login = @username and" +
                                 " u.password = @hash and p.status = true;";

                //Instanciando objetos para busca
                List<User> results = new List<User>();
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Adicionando os parametros e preparando consulta
                cmd.Parameters.Add(new SqlParameter("@username", user.GetLogin()));
                cmd.Parameters.Add(new SqlParameter("@hash", user.GetMD5Hash()));

                //Preparando comando
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    //Adicionando usuário encontrado ao array de resultados
                    User u = new User((string)reader["login"], (string)reader["password"]);
                    results.Add(u);
                }

                //Verificando se há resultados compatíveis
                if (results != null && results.Count > 0) {

                    //Fechando conexão com o banco
                    conn.Close();

                }
                else {

                    //Fechando conexão com o banco
                    conn.Close();

                    //Ops não há usuários com essa autentificação
                    throw new NegatedAcessException();

                }

            }
            catch (SqlException) {

                //Fechando conexão com o banco
                conn.Close();

                //Ops erro de conexão com o banco
                throw new SqlCustomException();
            }

        }

        /**
         * Método para recuperar funcionário correpondente ao usuário
         */
        public Employe GetAssiciatedEmploye(User user) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo objetos de uso na consulta
                Employe e = new Employe();

                //Definindo comando da consulta
                string command = "select f.id_funcionario, f.permissao, p.nome_pessoa " +
                                 "from usuarios u, funcionarios f, pessoas p where " +
                                 "u.id_funcionario = f.id_funcionario and " +
                                 "f.id_pessoa = p.id_pessoa and " +
                                 "u.login = @username;";

                //Instanciando objetos para busca
                List<Employe> results = new List<Employe>();
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Adicionando parametros e preparando consulta
                cmd.Parameters.Add(new SqlParameter("@username", user.GetLogin()));

                //Preparando consulta
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    e.SetId((int)reader["id_funcionario"]);
                    e.SetName((string)reader["nome_pessoa"]);
                    e.SetRole((string)reader["permissao"]);
                }


                //Fechando conexão com banco
                conn.Close();

                //Retornando funcionário associado
                return e;

            }
            catch (SqlException) {

                //Retornando mensagem de erro
                conn.Close();
                MessageBox.Show("Erro ao se conectar com banco!");
                return new Employe();
            }

        }

    }
}
