using System.Text;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Promig.Connection {

    class ConnectionFactory {

        //Atributos para realização da conexão
        private static MySqlConnection conn;

        //Atributos estaticos que correspondem a dados da conexão
        public static string DATABASE_SERVER = "localhost";
        public static string DATABASE_NAME = "promig";
        public static string DATABASE_USER = "promig_user";
        public static string DATABASE_PASSWORD = "promig_erp";

        //Definindo nome das tabelas
        public static string TABLE_USERS = "usuarios";
        public static string TABLE_ADRESS = "enderecos";
        public static string TABLE_PEOPLE = "pessoas";
        public static string TABLE_EMPLOYES = "funcionarios";
        public static string TABLE_LOGS = "log";

        //Método estático para retornar conexão com banco
        public static MySqlConnection GetConnection() {

            //Stringbuilder para criar string de conexão
            StringBuilder connString = new StringBuilder();

            //Criando a string de conexão com banco
            connString.Append("Server=").Append(DATABASE_SERVER).Append(";");
            connString.Append("Database=").Append(DATABASE_NAME).Append(";");
            connString.Append("Uid=").Append(DATABASE_USER).Append(";");
            connString.Append("Pwd=").Append(DATABASE_PASSWORD);

            //Tratando erros que podem ocorrer durante conexão
            try {

                //Verificando se ja há uma conexão instanciada
                if (conn == null) {

                    conn = new MySqlConnection(connString.ToString());
                }

                return conn;
            }
            catch (MySqlException) {

                //Retornando mensagem de erro em dialog de erro
                MessageBox.Show("Erro de Conexão",
                                "Impossível estabelecer conexão",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error
                );
                return null;
            }
        }
    }
}
