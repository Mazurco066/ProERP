using System.Collections.Generic;
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
     |      os dados relacionados a clientes do sistema do banco de dados.
     |
     */
    class Clients {

        #region header

        //Atributos de conexão
        private MySqlConnection conn;

        //Construtor
        public Clients() {

            //Instanciando objeto de conexão
            conn = ConnectionFactory.GetConnection();
        }

        #endregion header

        #region data-acess

        //Método para adicionar cliente
        public void AddClient(Client client) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando para inserção
                string command = "BEGIN; insert into enderecos" +
                                 "(rua, numero, bairro, cidade, uf, cep) values (" +
                                 "@street, @number, @neighborhood, @city, @uf, @cep);" +
                                 "insert into pessoas(id_endereco, nome_pessoa, status) values (" +
                                 "last_insert_id(), @name, @status);" +
                                 "insert into clientes(id_pessoa, fisico, cpf_cnpj, telefone_residencial, " +
                                 "telefone_celular, contato, inscricao_estadual) values (" +
                                 "last_insert_id(), @type, @document, @residence, " +
                                 "@cellphone, @description, @state_id); COMMIT;";

                //Definindo objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@street", client.adress.street));
                cmd.Parameters.Add(new MySqlParameter("@number", client.adress.number));
                cmd.Parameters.Add(new MySqlParameter("@neighborhood", client.adress.neighborhood));
                cmd.Parameters.Add(new MySqlParameter("@city", client.adress.city));
                cmd.Parameters.Add(new MySqlParameter("@uf", client.adress.UF));
                cmd.Parameters.Add(new MySqlParameter("@cep", client.adress.CEP));
                cmd.Parameters.Add(new MySqlParameter("@name", client.name));
                cmd.Parameters.Add(new MySqlParameter("@document", client.docNumber));
                cmd.Parameters.Add(new MySqlParameter("@residence", client.residenceNumber));
                cmd.Parameters.Add(new MySqlParameter("@cellphone", client.cellNumber));
                cmd.Parameters.Add(new MySqlParameter("@description", client.description));
                cmd.Parameters.Add(new MySqlParameter("@state_id", client.stateId));
                cmd.Parameters.Add(new MySqlParameter("@status", client.IsActive()));
                cmd.Parameters.Add(new MySqlParameter("@type", client.IsPhysical()));

                //Preparando o comando com os parametros
                cmd.Prepare();

                //Executando inserção
                cmd.ExecuteNonQuery();

                //Fechando conexão com banco
                conn.Close();

                //Mensagem de sucesso
                MessageBox.Show(
                    "Cliente inserido!",
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

        //Método para alterar cliente
        public void EditClient(Client client) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando para atualização
                string command = "BEGIN; update enderecos set " +
                                 "rua = @street, numero = @number, bairro = @neighborhood," +
                                 "cidade = @city, uf = @uf, cep = @cep " +
                                 "where id_endereco = @id_endereco;" +
                                 "update pessoas set nome_pessoa = @name, status = @status " +
                                 "where id_pessoa = @id_pessoa;" +
                                 "update clientes set fisico = @type, cpf_cnpj = @document, " +
                                 "telefone_residencial = @residence, telefone_celular = @cellphone, " +
                                 "contato = @description, inscricao_estadual = @state_id " +
                                 "where id_cliente = @id_cliente; COMMIT;";

                //Definindo objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo e adicioando parametros
                cmd.Parameters.Add(new MySqlParameter("@id_endereco", client.adress.id));
                cmd.Parameters.Add(new MySqlParameter("@id_pessoa", client.id_person));
                cmd.Parameters.Add(new MySqlParameter("@id_cliente", client.id));
                cmd.Parameters.Add(new MySqlParameter("@street", client.adress.street));
                cmd.Parameters.Add(new MySqlParameter("@number", client.adress.number));
                cmd.Parameters.Add(new MySqlParameter("@neighborhood", client.adress.neighborhood));
                cmd.Parameters.Add(new MySqlParameter("@city", client.adress.city));
                cmd.Parameters.Add(new MySqlParameter("@uf", client.adress.UF));
                cmd.Parameters.Add(new MySqlParameter("@cep", client.adress.CEP));
                cmd.Parameters.Add(new MySqlParameter("@name", client.name));
                cmd.Parameters.Add(new MySqlParameter("@document", client.docNumber));
                cmd.Parameters.Add(new MySqlParameter("@residence", client.residenceNumber));
                cmd.Parameters.Add(new MySqlParameter("@cellphone", client.cellNumber));
                cmd.Parameters.Add(new MySqlParameter("@description", client.description));
                cmd.Parameters.Add(new MySqlParameter("@state_id", client.stateId));
                cmd.Parameters.Add(new MySqlParameter("@status", client.IsActive()));
                cmd.Parameters.Add(new MySqlParameter("@type", client.IsPhysical()));

                //Preparando o comando com os parametros
                cmd.Prepare();

                //Executando inserção
                cmd.ExecuteNonQuery();

                //Mensagem de sucesso
                MessageBox.Show(
                    "Cliente atualizado!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                //Fechando conexão com banco
                conn.Close();

            }
            catch (MySqlException) {

                //Fechando a conexão e retornando erro ao usuário
                conn.Close();
                throw new DatabaseEditException();

            }

        }

        //Método para recuperar dados de cliente específico
        public Client GetClientData(int id) {

            try {

                //Abrindo conexão com o banco
                conn.Open();

                //Definindo string de coneção
                string command = "select c.id_cliente, p.id_pessoa, p.nome_pessoa, " +
                                 "p.status, e.id_endereco, e.rua, e.numero, e.bairro, e.cidade, " +
                                 "e.uf, e.cep, c.fisico, c.cpf_cnpj, c.telefone_residencial, " +
                                 "c.telefone_celular, c.contato, c.inscricao_estadual " +
                                 "from enderecos e, pessoas p, clientes c " +
                                 "where e.id_endereco = p.id_endereco and " +
                                 "p.id_pessoa = c.id_pessoa and " +
                                 "c.id_cliente = @id";

                //Definindo parametros
                MySqlParameter clientId = new MySqlParameter("@id", MySqlDbType.Int64);
                clientId.Value = id;

                //Definindo comando e resultados
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Adicionando parametros e preaparando consulta
                cmd.Parameters.Add(clientId);
                cmd.Prepare();

                //Realizando consulta
                reader = cmd.ExecuteReader();

                //Definindo cliente
                Client client = new Client();
                while (reader.Read()) {

                    //Recuperando dados do cliente
                    client.id = (int)reader["id_cliente"];
                    client.id_person = (int)reader["id_pessoa"];
                    client.adress.id = (int)reader["id_endereco"];
                    client.name = (string)reader["nome_pessoa"];
                    client.adress.street = (string)reader["rua"];
                    client.adress.number = reader["numero"].ToString();
                    client.adress.neighborhood = (string)reader["bairro"];
                    client.adress.city = (string)reader["cidade"];
                    client.adress.UF = (string)reader["uf"];
                    client.adress.CEP = (string)reader["cep"];
                    client.docNumber = (string)reader["cpf_cnpj"];
                    client.description = (string)reader["contato"];
                    client.residenceNumber = (string)reader["telefone_residencial"];
                    client.cellNumber = (string)reader["telefone_celular"];
                    client.stateId = (string)reader["inscricao_estadual"];

                    //Booleanos
                    client.SetPhysical((bool)reader["fisico"]);
                    if (!(bool)reader["status"]) client.Inactivate();
                }

                //Fechando conexão
                conn.Close();

                //Retornando cliente
                return client;

            }
            catch (MySqlException) {

                //Fechando conexão e retrnando mensagem de erro
                conn.Close();
                MessageBox.Show(
                    "Erro ao tentar se conectar ao banco! Tente novamente mais tarde...",
                    "Erro de Conexão!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return new Client();
            }
        }

        //Método para recuperar todos clientes
        public List<Client> GetAllClients(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select c.id_cliente, p.nome_pessoa, e.cidade, c.cpf_cnpj, c.telefone_residencial" +
                                 " from pessoas p, clientes c, enderecos e" +
                                 " where p.id_pessoa = c.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.nome_pessoa LIKE @param;";

                //Definindo objetos para recuperação de dados
                List<Client> results = new List<Client>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros da consulta
                MySqlParameter parameter = new MySqlParameter("@param", MySqlDbType.Text);
                parameter.Value = "%" + param + "%";

                //Adicionando os parametros e preparando consulta
                cmd.Parameters.Add(parameter);
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    //Adicionando cliente encontrado ao array de retorno
                    Client client = new Client();
                    client.id = (int)reader["id_cliente"];
                    client.name = (string)reader["nome_pessoa"];
                    client.adress.city = (string)reader["cidade"];
                    client.docNumber = (string)reader["cpf_cnpj"];
                    client.residenceNumber = (string)reader["telefone_residencial"];
                    results.Add(client);

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
        public List<Client> GetAllActiveClients(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select c.id_cliente, p.nome_pessoa, e.cidade, c.cpf_cnpj, c.telefone_residencial" +
                                 " from pessoas p, clientes c, enderecos e" +
                                 " where p.id_pessoa = c.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.status = @status and" +
                                 " p.nome_pessoa LIKE @param;";

                //Definindo parametros da consulta
                MySqlParameter status = new MySqlParameter("@status", MySqlDbType.Bit);
                MySqlParameter parameter = new MySqlParameter("@param", MySqlDbType.Text);
                status.Value = 1;
                parameter.Value = "%" + param + "%";

                //Definindo objetos para recuperação de dados
                List<Client> results = new List<Client>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Adicionando os parametros e preparando consulta
                cmd.Parameters.Add(status);
                cmd.Parameters.Add(parameter);
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    //Adicionando cliente encontrado ao array de retorno
                    Client client = new Client();
                    client.id = (int)reader["id_cliente"];
                    client.name = (string)reader["nome_pessoa"];
                    client.adress.city = (string)reader["cidade"];
                    client.docNumber = (string)reader["cpf_cnpj"];
                    client.residenceNumber = (string)reader["telefone_residencial"];
                    results.Add(client);

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
        public List<Client> GetAllActiveClientsByCity(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select c.id_cliente, p.nome_pessoa, e.cidade, c.cpf_cnpj, c.telefone_residencial" +
                                 " from pessoas p, clientes c, enderecos e" +
                                 " where p.id_pessoa = c.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.status = @status and" +
                                 " e.cidade LIKE @param;";

                //Definindo parametros da consulta
                MySqlParameter status = new MySqlParameter("@status", MySqlDbType.Bit);
                MySqlParameter parameter = new MySqlParameter("@param", MySqlDbType.Text);
                status.Value = 1;
                parameter.Value = "%" + param + "%";

                //Definindo objetos para recuperação de dados
                List<Client> results = new List<Client>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Adicionando os parametros e preparando consulta
                cmd.Parameters.Add(status);
                cmd.Parameters.Add(parameter);
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    //Adicionando cliente encontrado ao array de retorno
                    Client client = new Client();
                    client.id = (int)reader["id_cliente"];
                    client.name = (string)reader["nome_pessoa"];
                    client.adress.city = (string)reader["cidade"];
                    client.docNumber = (string)reader["cpf_cnpj"];
                    client.residenceNumber = (string)reader["telefone_residencial"];
                    results.Add(client);

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
        public List<Client> GetAllActiveClientsByDocument(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select c.id_cliente, p.nome_pessoa, e.cidade, c.cpf_cnpj, c.telefone_residencial" +
                                 " from pessoas p, clientes c, enderecos e" +
                                 " where p.id_pessoa = c.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.status = @status and" +
                                 " c.cpf_cnpj LIKE @param;";

                //Definindo parametros da consulta
                MySqlParameter status = new MySqlParameter("@status", MySqlDbType.Bit);
                MySqlParameter parameter = new MySqlParameter("@param", MySqlDbType.Text);
                status.Value = 1;
                parameter.Value = "%" + param + "%";

                //Definindo objetos para recuperação de dados
                List<Client> results = new List<Client>();
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Adicionando os parametros e preparando consulta
                cmd.Parameters.Add(status);
                cmd.Parameters.Add(parameter);
                cmd.Prepare();

                //Realizando busca no banco
                reader = cmd.ExecuteReader();

                //Verificando resultados
                while (reader.Read()) {

                    //Adicionando cliente encontrado ao array de retorno
                    Client client = new Client();
                    client.id = (int)reader["id_cliente"];
                    client.name = (string)reader["nome_pessoa"];
                    client.adress.city = (string)reader["cidade"];
                    client.docNumber = (string)reader["cpf_cnpj"];
                    client.residenceNumber = (string)reader["telefone_residencial"];
                    results.Add(client);

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
