using System.Windows;
using System.Windows.Controls;
using Promig.View.Components;

namespace Promig.View {
    
    public partial class MainWindow : Window {

        #region Header

        //Definindo flags
        public static string currentUsername;
        public static string currentPermission;
        public static int currentId;

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

        //Evento ao clicar no botão de Logs
        private void btnAuditoria_Click(object sender, RoutedEventArgs e) {
            UserControl usc = null;
            GridMain.Children.Clear();
            usc = new UserControlLogs();
            GridMain.Children.Add(usc);
        }

        // operacoes ao carregar o form
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            //Definindo nome do usuário ao abrir
            lblTitulo.Text = "Bem vindo " + currentUsername + "!";

            //Verificando permissões
            if(currentPermission.Equals("Admin")){
                return;
            }else{
                esconderBotoes();
            }
        }

        // desabilitar botoes
        private void esconderBotoes(){
            Employes.IsEnabled = false;
            BillsToPay.IsEnabled = false;
            BillsToReceive.IsEnabled = false;
            btnAuditoria.IsEnabled = false;

            Employes.Opacity = .50;
            BillsToPay.Opacity = .50;
            BillsToReceive.Opacity = .50;
            btnAuditoria.Opacity = .50;
        }
    }
}
