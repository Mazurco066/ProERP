using System.Windows;

namespace Promig.View {
    
    public partial class Login : Window {

        #region Header
        
        public Login() {
            InitializeComponent();
            Clear();
        }

        #endregion Header

        #region Events

        //Evento para o botão logar
        private void btnLogon_Click(object sender, RoutedEventArgs e) {

            if (txtUser.Text.Equals("admin") && txtPassword.Password.Equals("admin")) {
                MainWindow.currentUsername = "admin";
                MainWindow.currentPermission = 1;
                MainWindow window = new MainWindow();
                window.Show();
                Close();
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

        #endregion Utils
    }
}
