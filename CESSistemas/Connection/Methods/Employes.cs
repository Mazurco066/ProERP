using System.Collections.Generic;
using Promig.Model;
using Promig.Exceptions;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace Promig.Connection.Methods
{
    /*______________________________________________________________________
     |
     |                      CLASSE DE ACESSO A DADOS
     |
     |      Classe para inserir editar, excluir, inativar e recuperar todos
     |      os dados relacionados a funcionários do sistema do banco de dados.
     |
     */
    class Employes {

        #region header

        //Atributos de conexão
        private MySqlConnection conn;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public Employes() {

            //Instanciando objeto de conexão
            conn = ConnectionFactory.GetConnection();
        }

        #endregion header

        #region data-acess

        /// <summary>
        /// Método para adicionar funcionário no banco
        /// </summary>
        /// <param name="emp"></param>
        public void AddEmploye(Employe emp) {

            try {

                //Abrindo conexão
                conn.Open();

                //Definindo comando para inserção
                string command = $"BEGIN; insert into {Refs.TABLE_ADRESS}" +
                                 $"(rua, numero, bairro, cidade, uf, cep) values (" +
                                 $"@street, @number, @neighborhood, @city, @uf, @cep);" +
                                 $"insert into {Refs.TABLE_PEOPLE}(id_endereco, nome_pessoa, status) values (" +
                                 $"last_insert_id(), @name, @status);" +
                                 $"insert into {Refs.TABLE_EMPLOYES}(id_pessoa, permissao, cpf, " +
                                 $"data_admissao, funcao) values (" +
                                 $"last_insert_id(), @role, @cpf, " +
                                 $"@admission, @job);";
                if (emp.user != null)
                    command += $"insert into {Refs.TABLE_USERS}(id_funcionario, login, password) values (" +
                               $"last_insert_id(), @username, @password);";
                command += $" COMMIT;";

                //Definindo objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@street", emp.adress.street));
                cmd.Parameters.Add(new MySqlParameter("@number", emp.adress.number));
                cmd.Parameters.Add(new MySqlParameter("@neighborhood", emp.adress.neighborhood));
                cmd.Parameters.Add(new MySqlParameter("@city", emp.adress.city));
                cmd.Parameters.Add(new MySqlParameter("@uf", emp.adress.UF));
                cmd.Parameters.Add(new MySqlParameter("@cep", emp.adress.CEP));
                cmd.Parameters.Add(new MySqlParameter("@name", emp.name));
                cmd.Parameters.Add(new MySqlParameter("@status", emp.IsActive()));
                cmd.Parameters.Add(new MySqlParameter("@role", emp.role));
                cmd.Parameters.Add(new MySqlParameter("@cpf", emp.cpf));
                cmd.Parameters.Add(new MySqlParameter("@admission", emp.admission));
                cmd.Parameters.Add(new MySqlParameter("@job", emp.job));

                //Caso funsionário possuir usuário
                if (emp.user != null) {
                    cmd.Parameters.Add(new MySqlParameter("@username", emp.user.GetLogin()));
                    cmd.Parameters.Add(new MySqlParameter("@password", emp.user.GetEncryptedCode()));
                }

                //Preparando comando com os parametros
                cmd.Prepare();

                //Executando inserção
                cmd.ExecuteNonQuery();

                //Fechando conexão com banco
                conn.Close();

                //Mensagem de sucesso
                MessageBox.Show(
                    "Funcionário Inserido!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

            }
            catch (MySqlException err) {

                //Fechando a conexão e retornando erro ao usuário
                conn.Close();
                MessageBox.Show(err.Message);
                throw new DatabaseInsertException();
            }
        }

        /// <summary>
        /// Método para alterar funcionário no banco
        /// </summary>
        /// <param name="emp"></param>
        public void EditEmploye(Employe emp) {

            try {

                //Abrindo conexão
                conn.Open();

                //Definindo comando para inserção
                string command = $"BEGIN; update {Refs.TABLE_ADRESS} set " +
                                 $"rua = @street, numero = @number, bairro = @neighborhood," +
                                 $"cidade = @city, uf = @uf, cep = @cep " +
                                 $"where id_endereco = @id_endereco;" +
                                 $"update {Refs.TABLE_PEOPLE} set nome_pessoa = @name, status = @status " +
                                 $"where id_pessoa = @id_pessoa;" +
                                 $"update {Refs.TABLE_EMPLOYES} set permissao = @role, cpf = @cpf, " +
                                 $"data_admissao = @admission, funcao = @job " +
                                 $"where id_funcionario = @id_funcionario;";
                if (emp.user != null)
                    command += $"replace into {Refs.TABLE_USERS}(id_funcionario, login, password) values (" +
                               $"@id_funcionario, @username, @password);";
                else
                    command += $"delete from {Refs.TABLE_USERS} where id_funcionario = @id_funcionario;";
                command += $" COMMIT;";

                //Definindo objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@id_endereco", emp.adress.id));
                cmd.Parameters.Add(new MySqlParameter("@id_pessoa", emp.id_person));
                cmd.Parameters.Add(new MySqlParameter("@id_funcionario", emp.id));
                cmd.Parameters.Add(new MySqlParameter("@street", emp.adress.street));
                cmd.Parameters.Add(new MySqlParameter("@number", emp.adress.number));
                cmd.Parameters.Add(new MySqlParameter("@neighborhood", emp.adress.neighborhood));
                cmd.Parameters.Add(new MySqlParameter("@city", emp.adress.city));
                cmd.Parameters.Add(new MySqlParameter("@uf", emp.adress.UF));
                cmd.Parameters.Add(new MySqlParameter("@cep", emp.adress.CEP));
                cmd.Parameters.Add(new MySqlParameter("@name", emp.name));
                cmd.Parameters.Add(new MySqlParameter("@status", emp.IsActive()));
                cmd.Parameters.Add(new MySqlParameter("@role", emp.role));
                cmd.Parameters.Add(new MySqlParameter("@cpf", emp.cpf));
                cmd.Parameters.Add(new MySqlParameter("@admission", emp.admission));
                cmd.Parameters.Add(new MySqlParameter("@job", emp.job));

                //Caso funsionário possuir usuário
                if (emp.user != null) {
                    cmd.Parameters.Add(new MySqlParameter("@username", emp.user.GetLogin()));
                    cmd.Parameters.Add(new MySqlParameter("@password", emp.user.GetEncryptedCode()));
                }

                //Preparando comando com os parametros
                cmd.Prepare();

                //Executando inserção
                cmd.ExecuteNonQuery();

                //Fechando conexão com banco
                conn.Close();

                //Mensagem de sucesso
                MessageBox.Show(
                    "Funcionário Atualizado!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

            }
            catch (MySqlException) {

                //Fechando conexão e retornando erro
                conn.Close();
            }
        }

        /// <summary>
        /// Método para recuperar fornecedor especifico com base no ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employe GetEmployeData(int id) {

            try {

                //Abrindo conexão com o banco
                conn.Open();

                //Definindo string de coneção
                string command = $"select f.id_funcionario, p.id_pessoa, p.nome_pessoa, " +
                                 $"p.status, e.id_endereco, e.rua, e.numero, e.bairro, e.cidade, " +
                                 $"e.uf, e.cep, f.data_admissao, f.cpf, f.funcao, f.permissao " +
                                 $"from {Refs.TABLE_ADRESS} e, {Refs.TABLE_PEOPLE} p, {Refs.TABLE_EMPLOYES} f " +
                                 $"where e.id_endereco = p.id_endereco and " +
                                 $"p.id_pessoa = f.id_pessoa and " +
                                 $"f.id_funcionario = @id";

                //Definindo comando e resultados
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros
                MySqlParameter employeId = new MySqlParameter("@id", MySqlDbType.Int64);
                employeId.Value = id;

                //Adicionando parametros e preaparando consulta
                cmd.Parameters.Add(employeId);
                cmd.Prepare();

                //Realizando consulta
                reader = cmd.ExecuteReader();

                //Definindo cliente
                Employe employe = new Employe();
                while (reader.Read()) {

                    //Recuperando dados do cliente
                    employe.id = (int)reader["id_funcionario"];
                    employe.id_person = (int)reader["id_pessoa"];
                    employe.adress.id = (int)reader["id_endereco"];
                    employe.name = (string)reader["nome_pessoa"];
                    employe.adress.street = (string)reader["rua"];
                    employe.adress.number = reader["numero"].ToString();
                    employe.adress.neighborhood = (string)reader["bairro"];
                    employe.adress.city = (string)reader["cidade"];
                    employe.adress.UF = (string)reader["uf"];
                    employe.adress.CEP = (string)reader["cep"];
                    employe.cpf = (string)reader["cpf"];
                    employe.role = (string)reader["permissao"];
                    employe.job = (string)reader["funcao"];
                    employe.admission = (string)reader["data_admissao"];

                    //Booleanos
                    if (!(bool)reader["status"]) employe.Inactivate();

                }
                reader.Close();

                //Segunda query, opcionals e funcionário possuir usuário
                if (!employe.role.Equals("none")) {

                    command = $"select u.login, u.password " +
                              $"from {Refs.TABLE_USERS} u, {Refs.TABLE_EMPLOYES} f " +
                              $"where u.id_funcionario = f.id_funcionario " +
                              $"and f.id_funcionario = @id;";

                    //Definindo novo comando
                    cmd = new MySqlCommand(command, conn) {
                        CommandType = CommandType.Text
                    };

                    //Limpando parametros e preparando outra consulta
                    cmd.Parameters.Add(employeId);
                    cmd.Prepare();

                    //Executando consulta por usuário
                    reader = cmd.ExecuteReader();

                    //Verifiando resultados
                    while (reader.Read()) {

                        User user = new User();
                        user.SetLogin((string)reader["login"]);
                        user.SetEncryptedCode((string)reader["password"]);
                        employe.user = user;
                    }

                }

                //Fechando conexão
                conn.Close();

                //Retornando cliente
                return employe;

            }
            catch (MySqlException) {

                //Fechando conexão e retrnando mensagem de erro
                conn.Close();
                MessageBox.Show(
                    "Ops... Ocorreu algum erro de conexão com o banco! Tente novamente mais tarde...",
                    "Erro de Conexão!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                //Retornando usuário vazio
                Employe _return = new Employe();
                _return.name = "";
                _return.role = "none";
                return _return;

            }

        }

        /// <summary>
        /// Método para recuperar todos funcionários
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Employe> GetAllEmployes(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = $"select f.id_funcionario, p.nome_pessoa, e.cidade, f.cpf" +
                                 $" from {Refs.TABLE_PEOPLE} p, {Refs.TABLE_EMPLOYES} f, {Refs.TABLE_ADRESS} e" +
                                 $" where p.id_pessoa = f.id_pessoa and" +
                                 $" p.id_endereco = e.id_endereco and" +
                                 $" p.nome_pessoa LIKE @param;";

                //Definindo objetos para recuperação de dados
                List<Employe> results = new List<Employe>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros da consulta
                cmd.Parameters.Add(new MySqlParameter("@param", "%" + param + "%"));

                //Preparando consulta
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    //Adicionando cliente encontrado ao array de retorno
                    Employe employe = new Employe();
                    employe.id = (int)reader["id_funcionario"];
                    employe.name = (string)reader["nome_pessoa"];
                    employe.adress.city = (string)reader["cidade"];
                    employe.cpf = (string)reader["cpf"];
                    results.Add(employe);
                }

                //Fechando conexão e retornando clientes
                conn.Close();
                return results;

            }
            catch (MySqlException) {

                //Fechando conexão e retornando erro
                conn.Close();
                throw new DatabaseAccessException();
            }

        }

        /// <summary>
        /// Método para recuperar todos funcionários ativos
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Employe> GetAllActiveEmployes(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = $"select f.id_funcionario, p.nome_pessoa, e.cidade, f.cpf" +
                                 $" from {Refs.TABLE_PEOPLE} p, {Refs.TABLE_EMPLOYES} f, {Refs.TABLE_ADRESS} e" +
                                 $" where p.id_pessoa = f.id_pessoa and" +
                                 $" p.id_endereco = e.id_endereco and" +
                                 $" p.status = @status and" +
                                 $" p.nome_pessoa LIKE @param;";

                //Definindo objetos para recuperação de dados
                List<Employe> results = new List<Employe>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros da consulta
                cmd.Parameters.Add(new MySqlParameter("@status", true));
                cmd.Parameters.Add(new MySqlParameter("@param", "%" + param + "%"));

                //Preparando comando sql
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    //Adicionando cliente encontrado ao array de retorno
                    Employe employe = new Employe();
                    employe.id = (int)reader["id_funcionario"];
                    employe.name = (string)reader["nome_pessoa"];
                    employe.adress.city = (string)reader["cidade"];
                    employe.cpf = (string)reader["cpf"];
                    results.Add(employe);

                }

                //Fechando conexão e retornando clientes
                conn.Close();
                return results;

            }
            catch (MySqlException) {

                //Fechando conexão e retornando erro
                conn.Close();
                throw new DatabaseAccessException();
            }

        }

        /// <summary>
        /// Método para recuperar todos funcionários ativos por cidade
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Employe> GetAllActiveEmployesByCity(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = $"select f.id_funcionario, p.nome_pessoa, e.cidade, f.cpf" +
                                 $" from {Refs.TABLE_PEOPLE} p, {Refs.TABLE_EMPLOYES} f, {Refs.TABLE_ADRESS} e" +
                                 $" where p.id_pessoa = f.id_pessoa and" +
                                 $" p.id_endereco = e.id_endereco and" +
                                 $" p.status = @status and" +
                                 $" e.cidade LIKE @param;";

                //Definindo objetos para recuperação de dados
                List<Employe> results = new List<Employe>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros da consulta
                cmd.Parameters.Add(new MySqlParameter("@status", true));
                cmd.Parameters.Add(new MySqlParameter("@param", "%" + param + "%"));

                //Preparando a consulta
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    //Adicionando cliente encontrado ao array de retorno
                    Employe employe = new Employe();
                    employe.id = (int)reader["id_funcionario"];
                    employe.name = (string)reader["nome_pessoa"];
                    employe.adress.city = (string)reader["cidade"];
                    employe.cpf = (string)reader["cpf"];
                    results.Add(employe);

                }

                //Fechando conexão e retornando clientes
                conn.Close();
                return results;

            }
            catch (MySqlException) {

                //Fechando conexão e retornando erro
                conn.Close();
                throw new DatabaseAccessException();
            }

        }

        /// <summary>
        /// Método para recuperar todos funcionários ativos por documento
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Employe> GetAllActiveEmployesByDocument(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = $"select f.id_funcionario, p.nome_pessoa, e.cidade, f.cpf" +
                                 $" from {Refs.TABLE_PEOPLE} p, {Refs.TABLE_EMPLOYES} f, {Refs.TABLE_ADRESS} e" +
                                 $" where p.id_pessoa = f.id_pessoa and" +
                                 $" p.id_endereco = e.id_endereco and" +
                                 $" p.status = @status and" +
                                 $" f.cpf LIKE @param;";

                //Definindo objetos para recuperação de dados
                List<Employe> results = new List<Employe>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros da consulta
                cmd.Parameters.Add(new MySqlParameter("@status", true));
                cmd.Parameters.Add(new MySqlParameter("@param", "%" + param + "%"));

                //Preparando consulta
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    //Adicionando cliente encontrado ao array de retorno
                    Employe employe = new Employe();
                    employe.id = (int)reader["id_funcionario"];
                    employe.name = (string)reader["nome_pessoa"];
                    employe.adress.city = (string)reader["cidade"];
                    employe.cpf = (string)reader["cpf"];
                    results.Add(employe);

                }

                //Fechando conexão e retornando clientes
                conn.Close();
                return results;

            }
            catch (MySqlException) {

                //Fechando conexão e retornando erro
                conn.Close();
                throw new DatabaseAccessException();
            }

        }

        #endregion data-acess

    }
}
