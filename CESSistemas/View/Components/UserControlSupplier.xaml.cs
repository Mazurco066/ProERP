using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Promig.Connection.Methods;
using Promig.Exceptions;
using Promig.Model;
using Promig.Utils;

namespace Promig.View.Components {
    
    public partial class UserControlSupplier : UserControl {

        #region Header

        private Suppliers dao;
        private Logs logs;
        private Supplier aux;
        private Employe _employe;
        private int actionIndex;

        #endregion

        #region Constructors

        public UserControlSupplier() {

            //Iniciando os componentes
            InitializeComponent();

            //Instanciando objetos
            actionIndex = -1;
            _employe = new Employe();
            _employe.id = MainWindow.currentId;
            aux = null;
            logs = new Logs();
            dao = new Suppliers();
        }

        #endregion

        #region Events

        //Evento ao carregar componente
        private void control_loaded(object sender, RoutedEventArgs e) {
            SetDefaults();
            BlockFields();
            RefreshGrid();
        }

        //Evento ao alternar parametro de pesquisa
        private void cbSearch_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            RefreshGrid();
        }

        //Evento ao pesquisar na caixa de pesquisa
        private void txtSearch_KeyDown(object sender, RoutedEventArgs e) {
            RefreshGrid(txtSearch.Text);
        }

        //Evento para atualizar grid manualmente
        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            RefreshGrid();
        }

        #endregion

        #region Data-Gathering

        #endregion

        #region Grid-Param

        private void RefreshGrid() {

            //Limpando campo de pesquisa
            txtSearch.Text = null;

            try {

                //Filtros de busca
                switch (cbSearch.SelectedIndex) {

                    case 0: //Ativo - nome
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliers(txtSearch.Text);
                        break;

                    case 1: //Todos - nome
                        dgSuppliers.ItemsSource = dao.GetAllSuppliers(txtSearch.Text);
                        break;

                    case 2: //Ativo - Cidade
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByCity(txtSearch.Text);
                        break;

                    case 3: //Ativo - CPF
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByDocument(txtSearch.Text);
                        break;
                }
            }
            catch(DatabaseAccessException err) {
                MessageBox.Show(
                    err.Message,
                    "Problemas ao acessar o banco!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void RefreshGrid(string param) {

            try {

                //Filtros de busca
                switch (cbSearch.SelectedIndex) {

                    case 0: //Ativo - nome
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliers(param);
                        break;

                    case 1: //Todos - nome
                        dgSuppliers.ItemsSource = dao.GetAllSuppliers(param);
                        break;

                    case 2: //Ativo - Cidade
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByCity(param);
                        break;

                    case 3: //Ativo - CPF
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByDocument(param);
                        break;
                }
            }
            catch(DatabaseAccessException err) {
                MessageBox.Show(
                    err.Message,
                    "Problemas ao acessar o banco!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        #endregion

        #region Utils

        private void SetDefaults() {

            //Definindo valor inicial de combobox
            cbActive.SelectedIndex = 0;
            cbState.SelectedIndex = 0;
            cbSearch.SelectedIndex = 0;
        }

        private bool IsValidFields() {
            //Verificando se ha ou n usuário
            return !(NameEdit.Text.Equals("") ||
                    cnpjEdit.Text.Equals("") ||
                    AdressEdit.Text.Equals("") ||
                    NeighboorhoodEdit.Text.Equals("") ||
                    CityEdit.Text.Equals("") ||
                    NumberEdit.Text.Equals("") ||
                    cepEdit.Text.Equals("") ||
                    phone1Edit.Text.Equals("")
            );
        }

        private void ClearFields() {

            //Comboboxes
            cbActive.SelectedIndex = 0;
            cbState.SelectedIndex = 0;

            //Campos de texto
            NameEdit.Text = null;
            cnpjEdit.Text = null;
            AdressEdit.Text = null;
            NeighboorhoodEdit.Text = null;
            NumberEdit.Text = null;
            cepEdit.Text = null;
            CityEdit.Text = null;
            phone1Edit.Text = null;
            phone2Edit.Text = null;
        }

        private void EnableFields() {

            //Comboboxes
            cbActive.IsEnabled = true;
            cbState.IsEnabled = true;

            //Campos de texto
            NameEdit.IsEnabled = true;
            cnpjEdit.IsEnabled = true;
            AdressEdit.IsEnabled = true;
            NeighboorhoodEdit.IsEnabled = true;
            NumberEdit.IsEnabled = true;
            cepEdit.IsEnabled = true;
            CityEdit.IsEnabled = true;
            phone1Edit.IsEnabled = true;
            phone2Edit.IsEnabled = true;

            //Botões
            btnCancelar.IsEnabled = true;
            btnSalvar.IsEnabled = true;
        }

        private void BlockFields() {

            //Comboboxes
            cbActive.IsEnabled = false;
            cbState.IsEnabled = false;

            //Campos de texto
            NameEdit.IsEnabled = false;
            cnpjEdit.IsEnabled = false;
            AdressEdit.IsEnabled = false;
            NeighboorhoodEdit.IsEnabled = false;
            NumberEdit.IsEnabled = false;
            cepEdit.IsEnabled = false;
            CityEdit.IsEnabled = false;
            phone1Edit.IsEnabled = false;
            phone2Edit.IsEnabled = false;

            //Botões
            btnCancelar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
        }

        #endregion
    }
}
