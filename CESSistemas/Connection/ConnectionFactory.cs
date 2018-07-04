using System.Text;
using System.Data.SqlClient;
using System.Windows;

namespace Promig.Connection {

    class ConnectionFactory {

        //Atributos para realização da conexão
        private static SqlConnection conn;

        //Atributos estaticos que correspondem a dados da conexão
        public static string DATABASE_SERVER = "localhost";
        public static string DATABASE_NAME = "promig";
        public static string DATABASE_USER = "root";
        public static string DATABASE_PASSWORD = "";

        //Método estático para retornar conexão com banco
        public static SqlConnection GetConnection() {

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

                    conn = new SqlConnection(connString.ToString());
                }

                return conn;
            }
            catch (SqlException) {

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
