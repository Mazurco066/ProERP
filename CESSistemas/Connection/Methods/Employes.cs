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

        //Construtor
        public Employes() {

            //Instanciando objeto de conexão
            conn = ConnectionFactory.GetConnection();
        }

        #endregion header

        #region data-acess

        //Método para adicionar funcionário
        public void AddEmploye(Employe emp) {

            try {

                //Abrindo conexão
                conn.Open();

                //Definindo comando para inserção
                string command = "BEGIN; insert into enderecos" +
                                 "(rua, numero, bairro, cidade, uf, cep) values (" +
                                 "@street, @number, @neighborhood, @city, @uf, @cep);" +
                                 "insert into pessoas(id_endereco, nome_pessoa, status) values (" +
                                 "last_insert_id(), @name, @status);" +
                                 "insert into funcionarios(id_pessoa, permissao, cpf, " +
                                 "data_admissao, funcao) values (" +
                                 "last_insert_id(), @role, @cpf, @rg, " +
                                 "@admission, @job);";
                if (emp.GetUser() != null)
                    command += "insert into usuarios(id_funcionario, login, password) values (" +
                               "last_insert_id(), @username, @password);";
                command += " COMMIT;";

                //Definindo objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@street", emp.GetAdress().GetStreet()));
                cmd.Parameters.Add(new MySqlParameter("@number", emp.GetAdress().GetNumber()));
                cmd.Parameters.Add(new MySqlParameter("@neighborhood", emp.GetAdress().GetNeighborhood()));
                cmd.Parameters.Add(new MySqlParameter("@city", emp.GetAdress().GetCity()));
                cmd.Parameters.Add(new MySqlParameter("@uf", emp.GetAdress().GetUF()));
                cmd.Parameters.Add(new MySqlParameter("@cep", emp.GetAdress().GetCEP()));
                cmd.Parameters.Add(new MySqlParameter("@name", emp.GetName()));
                cmd.Parameters.Add(new MySqlParameter("@status", emp.IsActive()));
                cmd.Parameters.Add(new MySqlParameter("@role", emp.GetRole()));
                cmd.Parameters.Add(new MySqlParameter("@cpf", emp.GetCpf()));
                cmd.Parameters.Add(new MySqlParameter("@admission", emp.GetAdmission()));
                cmd.Parameters.Add(new MySqlParameter("@job", emp.GetJob()));

                //Caso funsionário possuir usuário
                if (emp.GetUser() != null) {
                    cmd.Parameters.Add(new MySqlParameter("@username", emp.GetUser().GetLogin()));
                    cmd.Parameters.Add(new MySqlParameter("@password", emp.GetUser().GetMD5Hash()));
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
            catch (MySqlException) {

                //Fechando a conexão e retornando erro ao usuário
                conn.Close();
                throw new DatabaseInsertException();
            }
        }

        //Método para editar funcionário
        public void EditEmploye(Employe emp) {

            try {

                //Abrindo conexão
                conn.Open();

                //Definindo comando para inserção
                string command = "BEGIN; update enderecos set " +
                                 "rua = @street, numero = @number, bairro = @neighborhood," +
                                 "cidade = @city, uf = @uf, cep = @cep " +
                                 "where id_endereco = @id_endereco;" +
                                 "update pessoas set nome_pessoa = @name, status = @status " +
                                 "where id_pessoa = @id_pessoa;" +
                                 "update funcionarios set permissao = @role, cpf = @cpf, " +
                                 "data_admissao = @admission, funcao = @job " +
                                 "where id_funcionario = @id_funcionario;";
                if (emp.GetUser() != null)
                    command += "replace into usuarios(id_funcionario, login, password) values (" +
                               "@id_funcionario, @username, @password);";
                else
                    command += "delete from usuarios where id_funcionario = @id_funcionario;";
                command += " COMMIT;";

                //Definindo objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@id_endereco", emp.GetAdress().GetId()));
                cmd.Parameters.Add(new MySqlParameter("@id_pessoa", emp.GetIdPerson()));
                cmd.Parameters.Add(new MySqlParameter("@id_funcionario", emp.GetId()));
                cmd.Parameters.Add(new MySqlParameter("@street", emp.GetAdress().GetStreet()));
                cmd.Parameters.Add(new MySqlParameter("@number", emp.GetAdress().GetNumber()));
                cmd.Parameters.Add(new MySqlParameter("@neighborhood", emp.GetAdress().GetNeighborhood()));
                cmd.Parameters.Add(new MySqlParameter("@city", emp.GetAdress().GetCity()));
                cmd.Parameters.Add(new MySqlParameter("@uf", emp.GetAdress().GetUF()));
                cmd.Parameters.Add(new MySqlParameter("@cep", emp.GetAdress().GetCEP()));
                cmd.Parameters.Add(new MySqlParameter("@name", emp.GetName()));
                cmd.Parameters.Add(new MySqlParameter("@status", emp.IsActive()));
                cmd.Parameters.Add(new MySqlParameter("@role", emp.GetRole()));
                cmd.Parameters.Add(new MySqlParameter("@cpf", emp.GetCpf()));
                cmd.Parameters.Add(new MySqlParameter("@admission", emp.GetAdmission()));
                cmd.Parameters.Add(new MySqlParameter("@job", emp.GetJob()));

                //Caso funsionário possuir usuário
                if (emp.GetUser() != null) {
                    cmd.Parameters.Add(new MySqlParameter("@username", emp.GetUser().GetLogin()));
                    cmd.Parameters.Add(new MySqlParameter("@password", emp.GetUser().GetMD5Hash()));
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

        //Método para recuperar dados de funcionário específico
        public Employe GetEmployeData(int id) {

            try {

                //Abrindo conexão com o banco
                conn.Open();

                //Definindo string de coneção
                string command = "select f.id_funcionario, p.id_pessoa, p.nome_pessoa, " +
                                 "p.status, e.id_endereco, e.rua, e.numero, e.bairro, e.cidade, " +
                                 "e.uf, e.cep, f.data_admissao, f.cpf, f.funcao, f.permissao " +
                                 "from enderecos e, pessoas p, funcionarios f " +
                                 "where e.id_endereco = p.id_endereco and " +
                                 "p.id_pessoa = f.id_pessoa and " +
                                 "f.id_funcionario = @id";

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
                    employe.SetId((int)reader["id_funcionario"]);
                    employe.SetIdPerson((int)reader["id_pessoa"]);
                    employe.GetAdress().SetId((int)reader["id_endereco"]);
                    employe.SetName((string)reader["nome_pessoa"]);
                    employe.GetAdress().SetStreet((string)reader["rua"]);
                    employe.GetAdress().SetNumber(reader["numero"].ToString());
                    employe.GetAdress().SetNeighborhood((string)reader["bairro"]);
                    employe.GetAdress().SetCity((string)reader["cidade"]);
                    employe.GetAdress().SetUF((string)reader["uf"]);
                    employe.GetAdress().SetCEP((string)reader["cep"]);
                    employe.SetCpf((string)reader["cpf"]);
                    employe.SetRole((string)reader["permissao"]);
                    employe.SetJob((string)reader["funcao"]);
                    employe.SetAdmission((string)reader["data_admissao"]);

                    //Booleanos
                    if (!(bool)reader["status"]) employe.Inactivate();

                }
                reader.Close();

                //Segunda query, opcionals e funcionário possuir usuário
                if (!employe.GetRole().Equals("none")) {

                    command = "select u.login, u.password " +
                              "from usuarios u, funcionarios f " +
                              "where u.id_funcionario = f.id_funcionario " +
                              "and f.id_funcionario = @id;";

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
                        user.SetMD5Hash((string)reader["password"]);
                        employe.SetUser(user);
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
                _return.SetName("");
                _return.SetRole("none");
                return _return;

            }

        }

        //Método para recuperar todos clientes
        public List<Employe> GetAllEmployes(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select f.id_funcionario, p.nome_pessoa, e.cidade, f.cpf" +
                                 " from pessoas p, funcionarios f, enderecos e" +
                                 " where p.id_pessoa = f.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.nome_pessoa LIKE @param;";

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
                    employe.SetId((int)reader["id_funcionario"]);
                    employe.SetName((string)reader["nome_pessoa"]);
                    employe.GetAdress().SetCity((string)reader["cidade"]);
                    employe.SetCpf((string)reader["cpf"]);
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

        //Método para recuperar todos clientes ativos
        public List<Employe> GetAllActiveEmployes(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select f.id_funcionario, p.nome_pessoa, e.cidade, f.cpf" +
                                 " from pessoas p, funcionarios f, enderecos e" +
                                 " where p.id_pessoa = f.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.status = @status and" +
                                 " p.nome_pessoa LIKE @param;";

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
                    employe.SetId((int)reader["id_funcionario"]);
                    employe.SetName((string)reader["nome_pessoa"]);
                    employe.GetAdress().SetCity((string)reader["cidade"]);
                    employe.SetCpf((string)reader["cpf"]);
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

        //Método para recuperar todos clientes ativos
        public List<Employe> GetAllActiveEmployesByCity(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select f.id_funcionario, p.nome_pessoa, e.cidade, f.cpf" +
                                 " from pessoas p, funcionarios f, enderecos e" +
                                 " where p.id_pessoa = f.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.status = @status and" +
                                 " e.cidade LIKE @param;";

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
                    employe.SetId((int)reader["id_funcionario"]);
                    employe.SetName((string)reader["nome_pessoa"]);
                    employe.GetAdress().SetCity((string)reader["cidade"]);
                    employe.SetCpf((string)reader["cpf"]);
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

        //Método para recuperar todos clientes ativos
        public List<Employe> GetAllActiveEmployesByDocument(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select f.id_funcionario, p.nome_pessoa, e.cidade, f.cpf" +
                                 " from pessoas p, funcionarios f, enderecos e" +
                                 " where p.id_pessoa = f.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.status = @status and" +
                                 " f.cpf LIKE @param;";

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
                    employe.SetId((int)reader["id_funcionario"]);
                    employe.SetName((string)reader["nome_pessoa"]);
                    employe.GetAdress().SetCity((string)reader["cidade"]);
                    employe.SetCpf((string)reader["cpf"]);
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
