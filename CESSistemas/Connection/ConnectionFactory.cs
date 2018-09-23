using System.Text;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Promig.Connection {

    class ConnectionFactory {

        //Atributos para realização da conexão
        private static MySqlConnection conn;
        private static MySqlConnection root;

        //Atributos estaticos que correspondem a dados da conexão
        public static string DATABASE_SERVER = "localhost";
        public static string DATABASE_NAME = "promig";
        public static string DATABASE_USER = "promig_user";
        public static string DATABASE_PASSWORD = "promig_erp";
        public static string DATABASE_PORT = "3306";

        /// <summary>
        /// Método static para realizar conexão com banco
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Método staticpara se conectar com raiz do mysql
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection GetRootConnection() {

            //Stringbuilder para criar string de conexão
            StringBuilder connString = new StringBuilder();

            //Criando a string de conexão com banco
            connString.Append("server=").Append(DATABASE_SERVER).Append(";");
            connString.Append("user=").Append(DATABASE_USER).Append(";");
            connString.Append("port=").Append(DATABASE_PORT).Append(";");
            connString.Append("password=").Append(DATABASE_PASSWORD);

            //Tratando erros que podem ocorrer durante conexão
            try {

                //Verificando se ja há uma conexão instanciada
                if (root == null) {

                    root = new MySqlConnection(connString.ToString());
                }

                return root;
            } catch (MySqlException) {

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
