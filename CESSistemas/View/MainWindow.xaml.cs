using System.Windows;
using System.Windows.Controls;
using System;
using Promig.View.Components;
using Promig.Utils;

namespace Promig.View {
    
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            initializeUserControl();
        }

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

            //Limpando a grid
            GridMain.Children.Clear();

            //Verificando qual item foi selecionado
            switch(((ListViewItem)((ListView)sender).SelectedItem).Name){
                case "Main":
                    if (!GridMain.Children.Contains(UserControlMain.GetInstance())) 
                        GridMain.Children.Add(UserControlMain.GetInstance());
                    break;
                case "Client":
                    if (!GridMain.Children.Contains(UserControlClient.GetInstance())) 
                        GridMain.Children.Add(UserControlClient.GetInstance());              
                    break;
                case "Employes":
                    if (!GridMain.Children.Contains(UserControlEmployes.GetInstance())) 
                        GridMain.Children.Add(UserControlEmployes.GetInstance());
                    break;
                case "Supplier":
                    if (!GridMain.Children.Contains(UserControlSupplier.GetInstance())) 
                        GridMain.Children.Add(UserControlSupplier.GetInstance());
                    break;
                case "BillsToPay":
                    if (!GridMain.Children.Contains(UserControlBillsToPay.GetInstance())) 
                        GridMain.Children.Add(UserControlBillsToPay.GetInstance());
                    break;
                case "BillsToReceive":
                    if (!GridMain.Children.Contains(UserControlBillsToReceive.GetInstance())) 
                        GridMain.Children.Add(UserControlBillsToReceive.GetInstance());
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
                lblTitulo.Text = "Bem vindo! " + Login.usuarioLogado;

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
