using System.Windows;
using System.Windows.Controls;
using System;
using Promig.View.Components;
using Promig.Utils;

namespace Promig.View {
    
    public partial class MainWindow : Window {

        #region Header

        //Definindo flags
        public static string currentUsername;
        public static int currentPermission;

        //Definindo construtor
        public MainWindow() {
            InitializeComponent();
            initializeUserControl();
        }

        #endregion Header

        // inicializar userControl com a tela de pagina principal
        private void initializeUserControl(){
            UserControl usc = null;
            usc = new UserControlMain();
            GridMain.Children.Add(usc);
        }

        // botao de logout
        private void btnLogout_Click(object sender, RoutedEventArgs e) {
            Login janela = new Login();
            janela.Show();
            Close();
        }

        // abrir o menu
        private void btnAbrirMenu_Click(object sender, RoutedEventArgs e) {
            btnAbrirMenu.Visibility = Visibility.Collapsed;
            btnFecharMenu.Visibility = Visibility.Visible;
        }
        // fechar o menu
        private void btnFecharMenu_Click(object sender, RoutedEventArgs e) {
            btnAbrirMenu.Visibility = Visibility.Visible;
            btnFecharMenu.Visibility = Visibility.Collapsed;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            //Limpando a view atual
            UserControl usc = null;
            GridMain.Children.Clear();

            //Verificando qual tela será carregada
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name) {
                case "Main":
                    usc = new UserControlMain();
                    GridMain.Children.Add(usc);
                    break;
                case "Client":
                    usc = new UserControlClient();
                    GridMain.Children.Add(usc);
                    break;
                case "Employes":
                    usc = new UserControlEmployes();
                    GridMain.Children.Add(usc);
                    break;
                case "Supplier":
                    usc = new UserControlSupplier();
                    GridMain.Children.Add(usc);
                    break;
                case "BillsToPay":
                    usc = new UserControlBillsToPay();
                    GridMain.Children.Add(usc);
                    break;
                case "BillsToReceive":
                    usc = new UserControlBillsToReceive();
                    GridMain.Children.Add(usc);
                    break;
                case "Sair":
                    System.Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        private void btnUsuarios_Click(object sender, RoutedEventArgs e) {
            UserControl usc = null;
            GridMain.Children.Clear();
            usc = new UserControlUsers();
            GridMain.Children.Add(usc);
        }

        // operacoes ao carregar o form
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            try {
                lblTitulo.Text = "Bem vindo! " + currentUsername;

                /*Connection conexao = new Connection();
                var sql = from u in conexao.USUARIOS where u.nome_usuario == Login.usuarioLogado select u.tipo;
                if(string.Equals(sql.FirstOrDefault(), "Admin", StringComparison.OrdinalIgnoreCase)){
                    return;
                }else{
                    esconderBotoes();
                }*/
            }
            catch (Exception ex) {
                Log.logException(ex);
                Log.logMessage(ex.Message);
            }
        }

        // desabilitar botoes
        private void esconderBotoes(){
            Employes.IsEnabled = false;
            BillsToPay.IsEnabled = false;
            BillsToReceive.IsEnabled = false;
            btnUsuarios.IsEnabled = false;

            Employes.Opacity = .50;
            BillsToPay.Opacity = .50;
            BillsToReceive.Opacity = .50;
            btnUsuarios.Opacity = .50;
        }
    }
}
