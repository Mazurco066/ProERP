using System.Windows;
using System.Windows.Input;
using Promig.Connection.Methods;
using Promig.Exceptions;
using Promig.Model;

namespace Promig.View {
    
    public partial class Login : Window {

        #region Header

        //Definindo atributos
        private Users users;
        private Logs logs;

        /// <summary>
        /// Construtor padrão
        /// </summary>
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

        /// <summary>
        /// Evento ao clicar no botão de logar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    MainWindow.currentUsername = employe.name;
                    MainWindow.currentPermission = employe.role;
                    MainWindow.currentId = employe.id;
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

        /// <summary>
        /// Evento ao pressionar botão enter para logar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_PreviewKeyUp(object sender, KeyEventArgs e) {

            if (e.Key == Key.Enter) {

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
                        MainWindow.currentUsername = employe.name;
                        MainWindow.currentPermission = employe.role;
                        MainWindow.currentId = employe.id;
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
                    catch (SqlCustomException err) {
                        MessageBox.Show(err.Message,
                                        "Erro",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error
                        );
                    }

                }
            }
        }

                /// <summary>
                /// Evento ao pressionar botão sair
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private void btnExit_Click(object sender, RoutedEventArgs e) { App.Current.Shutdown(); }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Tab) {
                txtUser.Focus();
            }
            else {
                return;
            }
        }

        #endregion Events

        #region Utils

        /// <summary>
        /// Método para limpar campos
        /// </summary>
        private void Clear() {
            txtUser.Text = "";
            txtPassword.Password = "";
        }

        /// <summary>
        /// Método para verificar se campos estão preenchidos
        /// </summary>
        /// <returns></returns>
        private bool IsValidFields() {
            return !(txtUser.Text.Equals("") ||
                     txtPassword.Password.Equals(""));
        }

        #endregion Utils

    }
}
