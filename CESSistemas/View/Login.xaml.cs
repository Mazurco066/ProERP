using System.Windows;
using Promig.Connection.Methods;
using Promig.Exceptions;
using Promig.Model;

namespace Promig.View {
    
    public partial class Login : Window {

        #region Header

        //Definindo atributos
        private Users users;
        private Logs logs;

        //Construtor
        public Login() {
            //Inicializando componentes
            InitializeComponent();

            //Instanciando objetos
            users = new Users();
            logs = new Logs();
            Clear();
        }

        #endregion Header

        #region Events

        //Evento para o botão logar
        private void btnLogon_Click(object sender, RoutedEventArgs e) {

            if (!IsValidFields()) {
                MessageBox.Show("Validação",
                                "Há campos vazios!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning
                );
            }
            else {

                //Recuperando usuário
                User user = new User(txtUser.Text, txtPassword.Password);

                //Realizando uma tentativa de login
                try {
                    users.ValidateLogin(user);

                    //Recuperando funcionário que logou
                    Employe employe = users.GetAssiciatedEmploye(user);

                    //Gerando um log de sistema
                    Log signInRegister = new Log();
                    signInRegister.employe = employe;
                    signInRegister.action = "Usuário logou no sistema!";

                    //Registrando o log
                    logs.Register(signInRegister);

                    //Limpando campos e abrindo menu
                    Clear();
                    MainWindow.currentUsername = employe.GetName();
                    MainWindow.currentPermission = employe.GetRole();
                    MainWindow.currentId = employe.GetId();
                    MainWindow window = new MainWindow();
                    window.Show();
                    Close();
                }
                catch (NegatedAcessException err) {
                    MessageBox.Show(err.Message,
                                    "Autenticação",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error
                    );
                }
                catch(SqlCustomException err) {
                    MessageBox.Show(err.Message,
                                    "Erro",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error
                    );
                }

            }
        }

        //Evento para o botão sair
        private void btnExit_Click(object sender, RoutedEventArgs e) { App.Current.Shutdown(); }

        #endregion Events

        #region Utils

        private void Clear() {
            txtUser.Text = "";
            txtPassword.Password = "";
        }

        private bool IsValidFields() {
            return !(txtUser.Text.Equals("") ||
                     txtPassword.Password.Equals(""));
        }

        #endregion Utils
    }
}
