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
     |      os dados relacionados a fornecedores do sistema no banco de dados.
     |
     */
    class Suppliers {

        #region header

        //Atributos de conexão
        private MySqlConnection conn;

        //Construtor
        public Suppliers() {

            //Instanciando objeto de conexão
            conn = ConnectionFactory.GetConnection();
        }

        #endregion header

        //Método para inserir fornecedor no banco
        public void AddSupplier(Supplier supplier) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando para inserção
                string command = "BEGIN; insert into enderecos(rua, numero, bairro, cidade, uf, cep) " +
                                 "values (@street, @number, @neighborhood, @city, @uf, @cep);" +
                                 "insert into pessoas(id_endereco, nome_pessoa, status) " +
                                 "values (last_insert_id(), @name, @status);" +
                                 "insert into fornecedores(id_pessoa, cnpj, tel_residencial, tel_celular) " +
                                 "values (last_insert_id(), @cnpj, @resPhone, @cellPhone);" +
                                 "COMMIT;";

                //Definindo objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@street", supplier.GetAdress().GetStreet()));
                cmd.Parameters.Add(new MySqlParameter("@number", supplier.GetAdress().GetNumber()));
                cmd.Parameters.Add(new MySqlParameter("@neighborhood", supplier.GetAdress().GetNeighborhood()));
                cmd.Parameters.Add(new MySqlParameter("@city", supplier.GetAdress().GetCity()));
                cmd.Parameters.Add(new MySqlParameter("@uf", supplier.GetAdress().GetUF()));
                cmd.Parameters.Add(new MySqlParameter("@cep", supplier.GetAdress().GetCEP()));
                cmd.Parameters.Add(new MySqlParameter("@name", supplier.GetName()));
                cmd.Parameters.Add(new MySqlParameter("@cnpj", supplier.GetCnpj()));
                cmd.Parameters.Add(new MySqlParameter("@resPhone", supplier.GetResPhone()));
                cmd.Parameters.Add(new MySqlParameter("@cellPhone", supplier.GetCellPhone()));
                cmd.Parameters.Add(new MySqlParameter("@status", supplier.IsActive()));

                //Preparando o comando com os parametros
                cmd.Prepare();

                //Executando inserção
                cmd.ExecuteNonQuery();

                //Fechando conexão com banco
                conn.Close();

                //Mensagem de sucesso
                MessageBox.Show(
                    "Fornecedor inserido!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (MySqlException) {
                //Fechando connexão e lançando exceção de erro
                conn.Close();
                throw new DatabaseInsertException();
            }
        }

        //Método para alterar fornecedor no banco
        public void EditSupplier(Supplier supplier) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando para atualização
                string command = "BEGIN; update enderecos set " +
                                 "rua = @street, numero = @number, bairro = @neighborhood," +
                                 "cidade = @city, uf = @uf, cep = @cep " +
                                 "where id_endereco = @adressId;" +
                                 "update pessoas set nome_pessoa = @name, status = @status " +
                                 "where id_pessoa = @personId;" +
                                 "update fornecedores set cnpj = @cnpj, tel_residencial = @resPhone, " +
                                 "tel_celular = @cellPhone " +
                                 "where id_fornecedor = @suplierId; COMMIT;";

                //Definindo objetos para inserção dos dados
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };

                //Definindo parametros para inserção
                cmd.Parameters.Add(new MySqlParameter("@adressId", supplier.GetAdress().GetId()));
                cmd.Parameters.Add(new MySqlParameter("@personId", supplier.GetIdPerson()));
                cmd.Parameters.Add(new MySqlParameter("@suplierId", supplier.GetId()));
                cmd.Parameters.Add(new MySqlParameter("@street", supplier.GetAdress().GetStreet()));
                cmd.Parameters.Add(new MySqlParameter("@number", supplier.GetAdress().GetNumber()));
                cmd.Parameters.Add(new MySqlParameter("@neighborhood", supplier.GetAdress().GetNeighborhood()));
                cmd.Parameters.Add(new MySqlParameter("@city", supplier.GetAdress().GetCity()));
                cmd.Parameters.Add(new MySqlParameter("@uf", supplier.GetAdress().GetUF()));
                cmd.Parameters.Add(new MySqlParameter("@cep", supplier.GetAdress().GetCEP()));
                cmd.Parameters.Add(new MySqlParameter("@name", supplier.GetName()));
                cmd.Parameters.Add(new MySqlParameter("@cnpj", supplier.GetCnpj()));
                cmd.Parameters.Add(new MySqlParameter("@resPhone", supplier.GetResPhone()));
                cmd.Parameters.Add(new MySqlParameter("@cellphone", supplier.GetCellPhone()));
                cmd.Parameters.Add(new MySqlParameter("@status", supplier.IsActive()));

                //Preparando o comando com os parametros
                cmd.Prepare();

                //Executando inserção
                cmd.ExecuteNonQuery();

                //Fechando conexão com banco
                conn.Close();

                //Mensagem de sucesso
                MessageBox.Show(
                    "Fornecedor atualizado!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (MySqlException) {
                //Fechando connexão e lançando exceção de erro
                conn.Close();
                throw new DatabaseEditException();
            }
        }

        //Método para recuperar dados de fornecedor específico
        public Supplier GetSupplierData(int id) {

            try {

                //Abrindo conexão com o banco
                conn.Open();

                //Definindo string de coneção
                string command = "select f.id_fornecedor, p.id_pessoa, p.nome_pessoa, " +
                                 "p.status, e.id_endereco, e.rua, e.numero, e.bairro, e.cidade, " +
                                 "e.uf, e.cep, f.cnpj, f.tel_residencial, f.tel_celular " +
                                 "from enderecos e, pessoas p, fornecedores f " +
                                 "where e.id_endereco = p.id_endereco and " +
                                 "p.id_pessoa = f.id_pessoa and " +
                                 "f.id_fornecedor = @id";

                //Definindo comando e resultados
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(command, conn) {
                    CommandType = CommandType.Text
                };


                //Definindo parametros
                cmd.Parameters.Add(new MySqlParameter("@id", id));

                //Preparando o comando para consulta
                cmd.Prepare();

                //Realizando consulta
                reader = cmd.ExecuteReader();

                //Definindo cliente
                Supplier suplier = new Supplier();
                while (reader.Read()) {

                    //Recuperando dados do cliente
                    suplier.SetId((int)reader["id_fornecedor"]);
                    suplier.SetIdPerson((int)reader["id_pessoa"]);
                    suplier.GetAdress().SetId((int)reader["id_endereco"]);
                    suplier.SetName((string)reader["nome_pessoa"]);
                    suplier.GetAdress().SetStreet(reader["rua"].ToString());
                    suplier.GetAdress().SetNumber(reader["numero"].ToString());
                    suplier.GetAdress().SetNeighborhood((string)reader["bairro"]);
                    suplier.GetAdress().SetCity((string)reader["cidade"]);
                    suplier.GetAdress().SetUF((string)reader["uf"]);
                    suplier.GetAdress().SetCEP((string)reader["cep"]);
                    suplier.SetCnpj((string)reader["cnpj"]);
                    suplier.SetResPhone((string)reader["tel_residencial"]);
                    suplier.SetCellPhone((string)reader["tel_celular"]);

                    //Booleanos
                    if (!(bool)reader["status"]) suplier.Inactivate();

                }

                //Fechando conexão
                conn.Close();

                //Retornando cliente
                return suplier;

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
                return new Supplier();

            }

        }

        //Método para recuperar todos fornecedores
        public List<Supplier> GetAllSuppliers(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select f.id_fornecedor, p.nome_pessoa, e.cidade, f.cnpj" +
                                 " from pessoas p, fornecedores f, enderecos e" +
                                 " where p.id_pessoa = f.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.nome_pessoa LIKE @param;";

                //Definindo objetos para recuperação de dados
                List<Supplier> results = new List<Supplier>();
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
                    Supplier supplier = new Supplier();
                    supplier.SetId((int)reader["id_fornecedor"]);
                    supplier.SetName((string)reader["nome_pessoa"]);
                    supplier.GetAdress().SetCity((string)reader["cidade"]);
                    supplier.SetCnpj((string)reader["cnpj"]);
                    results.Add(supplier);
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

        //Método para recuperar todos fornecedores ativos
        public List<Supplier> GetAllActiveSuppliers(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select f.id_fornecedor, p.nome_pessoa, e.cidade, f.cnpj" +
                                 " from pessoas p, fornecedores f, enderecos e" +
                                 " where p.id_pessoa = f.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.status = @status and" +
                                 " p.nome_pessoa LIKE @param;";

                //Definindo objetos para recuperação de dados
                List<Supplier> results = new List<Supplier>();
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
                    Supplier supplier = new Supplier();
                    supplier.SetId((int)reader["id_fornecedor"]);
                    supplier.SetName((string)reader["nome_pessoa"]);
                    supplier.GetAdress().SetCity((string)reader["cidade"]);
                    supplier.SetCnpj((string)reader["cnpj"]);
                    results.Add(supplier);

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
        public List<Supplier> GetAllActiveSuppliersByCity(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select f.id_fornecedor, p.nome_pessoa, e.cidade, f.cnpj" +
                                 " from pessoas p, fornecedores f, enderecos e" +
                                 " where p.id_pessoa = f.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.status = @status and" +
                                 " e.cidade LIKE @param;";

                //Definindo objetos para recuperação de dados
                List<Supplier> results = new List<Supplier>();
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
                    Supplier supplier = new Supplier();
                    supplier.SetId((int)reader["id_fornecedor"]);
                    supplier.SetName((string)reader["nome_pessoa"]);
                    supplier.GetAdress().SetCity((string)reader["cidade"]);
                    supplier.SetCnpj((string)reader["cnpj"]);
                    results.Add(supplier);

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
        public List<Supplier> GetAllActiveSuppliersByDocument(string param) {

            try {

                //Abrindo conexão com banco
                conn.Open();

                //Definindo comando da consulta
                string command = "select f.id_fornecedor, p.nome_pessoa, e.cidade, f.cnpj" +
                                 " from pessoas p, fornecedores f, enderecos e" +
                                 " where p.id_pessoa = f.id_pessoa and" +
                                 " p.id_endereco = e.id_endereco and" +
                                 " p.status = @status and" +
                                 " c.cpf_cnpj LIKE @param;";

                //Definindo objetos para recuperação de dados
                List<Supplier> results = new List<Supplier>();
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
                    Supplier supplier = new Supplier();
                    supplier.SetId((int)reader["id_fornecedor"]);
                    supplier.SetName((string)reader["nome_pessoa"]);
                    supplier.GetAdress().SetCity((string)reader["cidade"]);
                    supplier.SetCnpj((string)reader["cnpj"]);
                    results.Add(supplier);

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

    }
}
