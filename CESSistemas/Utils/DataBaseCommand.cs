using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Promig.Connections;

namespace Promig.Utils
{
    class DataBaseCommand{

        // inserir usuario e senha padrao
        public static void access() {
            try {
                Connection conexao = new Connection();
                string usuario = "admin";
                string senha = "eKkqAeJvo3t60TtipgWMjg==";
                string end = "sem endereço";
                string bairro = "sem bairro";
                string cidade = "mogi mirim";
                string estado = "SP";
                string cpf = "33322211100";
                string email = "email@email.com";
                string tipo = "Admin";
                string sql = string.Format("INSERT INTO [dbo].[USUARIOS]([nome_usuario],[senha],[endereco],[bairro],[cidade],[estado],[cpf],[email],[tipo])" +
                                           "VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", usuario, senha, end, bairro, cidade, estado, cpf, email, tipo);

                conexao.Database.ExecuteSqlCommand(sql);
            }catch(Exception ex){
                Log.logException(ex);
                Log.logMessage(ex.Message);
            }
        }

        // verificar se a tabela esta fazia
        public static bool existeDados() {
            Connection conexao = new Connection();
            if (!conexao.USUARIOS.Any()) {
                return false;
            }
            else {
                return true;
            }
        }

        // criar banco de dados se nao existir
        public static  void createDataBaseEF() {
            Connection conexao = new Connection();
            if (!conexao.Database.Exists()) {
                conexao.Database.CreateIfNotExists();
            }
        }

        // inserir usuario e senha padrao caso tabela esteja vazia
        public static void createUserPassword(){
            if (!existeDados()) {
                access();
            }
        }
    }
}
