using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Promig.Connections;
using Promig.Utils;

namespace Promig
{
    /// <summary>
    /// Lógica interna para Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        public static string usuarioLogado;
        
        public Login()
        {
            InitializeComponent();
        }

        // botao entrar
        private void btnEntrar_Click(object sender, RoutedEventArgs e) {
            try {
                Connection conexao = new Connection();

                if (txtUsuario.Text.Equals(string.Empty) || txtSenha.Password.Equals(string.Empty)) {
                    MessageBox.Show("Campo usuário ou senha vazio!");
                    txtUsuario.Focus();
                    return;
                }

                var sql = conexao.USUARIOS.Where(usuario => usuario.nome_usuario == txtUsuario.Text);
                USUARIOS usu = sql.FirstOrDefault();
                string usuResult = usu.nome_usuario;
                string usuSenhaResult = usu.senha;

                if ((txtUsuario.Text.Equals(usu.nome_usuario)) && (txtSenha.Password.ToString().Equals(Criptografia.Decrypt(usu.senha)))) {
                    usuarioLogado = txtUsuario.Text;
                    MainWindow janela = new MainWindow();
                    janela.Show();
                    this.Hide();
                    Close();
                }
                else {
                    MessageBox.Show("Usuário ou senha inválidos!");
                    clearTextBox();
                }
            }catch(Exception ex){
                Log.logException(ex);
                Log.logMessage(ex.Message);
                clearTextBox();
            }
        }

        //Metodo para limpar campos(textBox)
        public void clearTextBox() {
            txtUsuario.Clear();
            txtSenha.Clear();
            txtUsuario.Focus();
        }

        // botao sair
        private void btnSair_Click(object sender, RoutedEventArgs e) {
            App.Current.Shutdown();
        }

        // entrar com a tecla enter
        private void txtSenha_KeyDown_1(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                btnEntrar_Click(sender, e);
            }
        }
    }
}
