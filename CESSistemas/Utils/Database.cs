using Promig.Connection;
using MySql.Data.MySqlClient;
using System.Windows;

namespace Promig.Utils {

    class Database {

        #region Methods

        /// <summary>
        /// Método para criar banco de dados no mysql
        /// </summary>
        public static void Create() {
            using (var conn = ConnectionFactory.GetRootConnection()) {
                using (var cmd = conn.CreateCommand()) {
                    conn.Open();
                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{ConnectionFactory.DATABASE_NAME}`;";
                    cmd.ExecuteNonQuery();
                }
            }     
        }

        /// <summary>
        /// Método para delatar banco de dados no mysql
        /// </summary>
        public static void Delete() {
            using (var conn = ConnectionFactory.GetRootConnection()) {
                using (var cmd = conn.CreateCommand()) {
                    conn.Open();
                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{ConnectionFactory.DATABASE_NAME}`;";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Inner-Tables

        public class Tables {

            #region Methods

            /// <summary>
            /// Método para criar tabelas dentro do banco de dados primig no mysql
            /// </summary>
            public static void Create() {
                try {

                    // Definindo comando de criação de cada tabela e do banco de dados
                    string adress = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_ADRESS}(" +
                        "id_endereco int not null auto_increment, " +
                        "rua varchar(255) not null, " +
                        "numero int not null, " +
                        "bairro varchar(255) not null, " +
                        "cidade varchar(255) not null, " +
                        "uf varchar(2) not null, " +
                        "cep varchar(8) not null, " +
                        "CONSTRAINT PRK_ID_ENDERECO PRIMARY KEY(id_endereco));";
                    string people = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_PEOPLE}(" +
                        "id_pessoa int not null auto_increment, " +
                        "id_endereco int not null, " +
                        "nome_pessoa varchar(255) not null, " +
                        "status boolean not null, " +
                        "CONSTRAINT PRK_ID_PESSOA PRIMARY KEY(id_pessoa), " +
                        "CONSTRAINT FRK_ID_PESSOA_ENDERECO FOREIGN KEY(id_endereco) REFERENCES enderecos(id_endereco));";
                    string employees = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_EMPLOYES}(" +
                        "id_funcionario int not null auto_increment, " +
                         "id_pessoa int not null, " +
                        "permissao varchar(255) not null, " +
                        "cpf varchar(11) unique not null, " +
                        "data_admissao varchar(10), " +
                        "funcao varchar(255), " +
                        "CONSTRAINT PRK_ID_FUNCIONARIO PRIMARY KEY(id_funcionario), " +
                        "CONSTRAINT FRK_ID_FUNCIONARIO FOREIGN KEY(id_pessoa) REFERENCES pessoas(id_pessoa));";
                    string clients = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_CLIENTS}(" +
                        "id_cliente int not null auto_increment, " +
                        "id_pessoa int not null, " +
                        "fisico boolean not null, " +
                        "cpf_cnpj varchar(14), " +
                        "telefone_residencial varchar(20), " +
                        "telefone_celular varchar(20), " +
                        "contato varchar(255), " +
                        "inscricao_estadual varchar(30), " +
                        "CONSTRAINT PRK_ID_CLIENTE PRIMARY KEY(id_cliente), " +
                        "CONSTRAINT FRK_ID_CLIENTE FOREIGN KEY(id_pessoa) REFERENCES pessoas(id_pessoa));";
                    string suppliers = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_SUPPLIERS}(" +
                        "id_fornecedor int not null auto_increment, " +
                        "id_pessoa int not null, " +
                        "razao_social varchar(255), " +
                        "cnpj varchar(14), " +
                        "tel_residencial varchar(20), " +
                        "tel_celular varchar(20), " +
                        "CONSTRAINT PRK_ID_FORNECEDOR PRIMARY KEY(id_fornecedor), " +
                        "CONSTRAINT FRK_ID_FORNECEDOR FOREIGN KEY(id_pessoa) REFERENCES pessoas(id_pessoa));";
                    string users = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_USERS}(" +
                        "login varchar(255) not null, " +
                        "password varchar(255) not null, " +
                        "id_funcionario int not null, " +
                        "CONSTRAINT UNK_LOGIN_USUARIO UNIQUE KEY(login), " +
                        "CONSTRAINT FRK_ID_USUARIO FOREIGN KEY(id_funcionario) REFERENCES funcionarios(id_funcionario));";
                    string debts = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_DEBITS}(" +
                        "id_debito int not null auto_increment, " +
                        "id_fornecedor int not null, " +
                        "descricao text, " +
                        "data_pagamento varchar(12), " +
                        "data_vencimento varchar(12), " +
                        "valor_total double, " +
                        "valor_inicial double, " +
                        "CONSTRAINT PRK_ID_DEBITO PRIMARY KEY(id_debito), " +
                        "CONSTRAINT FRK_ID_DEBITO FOREIGN KEY(id_fornecedor) REFERENCES fornecedores(id_fornecedor));";
                    string credits = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_CREDITS}(" +
                        "id_credito int not null auto_increment, " +
                        "id_cliente int not null, " +
                        "descricao text, " +
                        "data_recebimento varchar(12), " +
                        "data_vencimento varchar(12), " +
                        "valor_total double, " +
                        "valor_inicial double, " +
                        "CONSTRAINT PRK_ID_CREDITO PRIMARY KEY(id_credito), " +
                        "CONSTRAINT FRK_ID_CREDITO FOREIGN KEY(id_cliente) REFERENCES clientes(id_cliente));";
                    string estimates = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_ESTIMATES}(" +
                        "no_documento int not null auto_increment, " +
                        "id_cliente int not null, " +
                        "data_orcamento varchar(12), " +
                        "caminho_imagem varchar(255), " +
                        "descricao varchar(255), " +
                        "condicao_pagto varchar(10), " +
                        "execucao_dias varchar(10), " +
                        "valor_total double not null, " +
                        "constraint PRK_DOCNO primary key(no_documento), " +
                        "constraint FRK_CLIENTE foreign key(id_cliente) references pessoas(id_pessoa));";
                    string services = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_SERVICES}(" +
                        "id_servico int not null auto_increment, " +
                        "descricao varchar(255), " +
                        "valor_unitario double, " +
                        "constraint PRK_ID_SERVICO primary key(id_servico));";
                    string estimate_services = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_ESTIMATE_SERVICES}(" +
                        "id_orcamento int not null, " +
                        "id_servico int not null, " +
                        "quantidade int not null, " +
                        "constraint FRK_ID_ORCAMENTO foreign key(id_orcamento) references orcamentos(no_documento), " +
                        "constraint FRK_ID_SERVICO foreign key(id_servico) references servicos(id_servico));";
                    string logs = $"CREATE TABLE IF NOT EXISTS {Refs.TABLE_LOGS}(" +
                        "id_log int not null AUTO_INCREMENT, " +
                        "id_funcionario int not null, " +
                        "log_date varchar(255), " +
                        "user_action varchar(255), " +
                        "CONSTRAINT PRK_ID_LOG PRIMARY KEY(id_log), " +
                        "CONSTRAINT FRK_ID_FUNCIONARIO_LOG FOREIGN KEY(id_funcionario) REFERENCES funcionarios(id_funcionario));";

                    // Montando a ordem da criação das tabelas
                    string command = $"{adress}" +
                                     $"{people}" +
                                     $"{employees}" +
                                     $"{clients}" +
                                     $"{suppliers}" +
                                     $"{users}" +
                                     $"{debts}" +
                                     $"{credits}" +
                                     $"{estimates}" +
                                     $"{services}" +
                                     $"{estimate_services}" +
                                     $"{logs}";

                    // Criação das tabelas no banco
                    using (var conn = ConnectionFactory.GetConnection()) {
                        using (var cmd = conn.CreateCommand()) {
                            conn.Open();
                            cmd.CommandText = command;
                            cmd.ExecuteNonQuery();
                        }
                    }  
                    
                } catch (MySqlException err) { MessageBox.Show($"Erro ao criar tabelas no banco de dados: {err.Message}",
                                                               "Erro",
                                                               MessageBoxButton.OK,
                                                               MessageBoxImage.Error);
                }
            }

            /// <summary>
            /// Método para remover tabelas dentro do banco de dados primig no mysql
            /// </summary>
            public static void Delete() {
                try {

                    // Definindo comando de remoção de cada tabela e do banco de dados
                    string adress = $"DROP TABLE IF EXISTS {Refs.TABLE_ADRESS};";
                    string people = $"DROP TABLE IF EXISTS {Refs.TABLE_PEOPLE};";
                    string employees = $"DROP TABLE IF EXISTS {Refs.TABLE_EMPLOYES};";
                    string clients = $"DROP TABLE IF EXISTS {Refs.TABLE_CLIENTS};";
                    string suppliers = $"DROP TABLE IF EXISTS {Refs.TABLE_SUPPLIERS};";
                    string users = $"DROP TABLE IF EXISTS {Refs.TABLE_USERS};";
                    string debts = $"DROP TABLE IF EXISTS {Refs.TABLE_DEBITS};";
                    string credits = $"DROP TABLE IF EXISTS {Refs.TABLE_CREDITS};";
                    string estimates = $"DROP TABLE IF EXISTS {Refs.TABLE_ESTIMATES};";
                    string services = $"DROP TABLE IF EXISTS {Refs.TABLE_SERVICES};";
                    string estimate_services = $"DROP TABLE IF EXISTS {Refs.TABLE_ESTIMATE_SERVICES};";
                    string logs = $"DROP TABLE IF EXISTS {Refs.TABLE_LOGS};";

                    // Montando a ordem da remoção das tabelas
                    string command = $"{estimate_services}" +
                                     $"{services}" +
                                     $"{estimates}" +
                                     $"{logs}" +
                                     $"{credits}" +
                                     $"{debts}" +
                                     $"{users}" +
                                     $"{employees}" +
                                     $"{clients}" +
                                     $"{suppliers}" +
                                     $"{people}" +
                                     $"{adress}";

                    // Remoção das tabelas no banco
                    using (var conn = ConnectionFactory.GetConnection()) {
                        using (var cmd = conn.CreateCommand()) {
                            conn.Open();
                            cmd.CommandText = command;
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                catch (MySqlException err) {
                    MessageBox.Show($"Erro ao Deletar tabelas no banco de dados: {err.Message}",
                                      "Erro",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                }
            }

            #endregion
        }

        #endregion
    }
}
