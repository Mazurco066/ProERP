using System.Windows;
using System.Windows.Controls;
using Promig.Models;

namespace Promig {
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
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
            Application.Current.Shutdown();
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
            UserControl usc = null;
            GridMain.Children.Clear();

            switch(((ListViewItem)((ListView)sender).SelectedItem).Name){
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
    }
}
