using System.Windows;
using System.Windows.Controls;
using Promig.Connection.Methods;
using Promig.Model;
using Promig.Utils;

namespace Promig.View.Components {
    
    public partial class UserControlEmployes : UserControl {

        #region Header

        private Employes dao;
        private Logs logs;
        private Employe aux;
        private Employe _employe;
        private int actionIndex;

        #endregion

        #region Constructors

        public UserControlEmployes() {

            //Carregando os componentes
            InitializeComponent();

            //Instanciando objetos
            actionIndex = -1;
            _employe = new Employe();
            aux = null;
            logs = new Logs();
            dao = new Employes();
        }

        #endregion 

        #region Events

        //Evento ao carregar componente
        private void control_loaded(object sender, RoutedEventArgs e) {
            SetDefaults();
            RefreshGrid();
        }

        //Evento ao alternar tipo de usuário
        private void cbHasUser_SelectionChanged(object sender, SelectionChangedEventArgs args) {

            if (cbHasUser.SelectedIndex > 1)
                gridUser.Opacity = 0;
            else
                gridUser.Opacity = 100;
            
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

        #endregion Events

        #region Grid-Param

        private void RefreshGrid() {

            //Limpando campo de pesquisa
            txtSearch.Text = null;

            //Filtros de busca
            switch (cbSearch.SelectedIndex) {

                case 0: //Ativo - nome
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployes(txtSearch.Text);
                    break;

                case 1: //Todos - nome
                    dgUsuarios.ItemsSource = dao.GetAllEmployes(txtSearch.Text);
                    break;

                case 2: //Ativo - Cidade
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployesByCity(txtSearch.Text);
                    break;

                case 3: //Ativo - CPF
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployesByDocument(txtSearch.Text);
                    break;
            }
        }

        private void RefreshGrid(string param) {

            //Filtros de busca
            switch (cbSearch.SelectedIndex) {

                case 0: //Ativo - nome
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployes(param);
                    break;

                case 1: //Todos - nome
                    dgUsuarios.ItemsSource = dao.GetAllEmployes(param);
                    break;

                case 2: //Ativo - Cidade
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployesByCity(param);
                    break;

                case 3: //Ativo - CPF
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployesByDocument(param);
                    break;
            }
        }

        #endregion

        #region Utils

        private void SetDefaults() {

            //Definindo valor inicial de combobox
            cbActive.SelectedIndex = 0;
            cbHasUser.SelectedIndex = 2;
            cbState.SelectedIndex = 0;
            cbSearch.SelectedIndex = 0;

            //Definindo data inicial
            admissionEdit.Text = DateBr.GetDateBr();
        }

        private bool IsValidFields() {
            return !(NameEdit.Text.Equals("") ||
                    cpfEdit.Text.Equals("") ||
                    AdressEdit.Text.Equals("") ||
                    NeighboorhoodEdit.Text.Equals("") ||
                    CityEdit.Text.Equals("") ||
                    NumberEdit.Text.Equals("") ||
                    cepEdit.Text.Equals("") ||
                    admissionEdit.Text.Equals("") ||
                    RoleEdit.Text.Equals("")
            );
        }

        private void ClearFields() {

            //Comboboxes
            cbHasUser.SelectedIndex = 0;
            cbActive.SelectedIndex = 0;

            //Campos de texto
            NameEdit.Text = null;
            cpfEdit.Text = null;
            AdressEdit.Text = null;
            NeighboorhoodEdit.Text = null;
            NumberEdit.Text = null;
            cepEdit.Text = null;
            admissionEdit.Text = null;
            RoleEdit.Text = null;
            usernameEdit.Text = null;
            passwordEdit.Password = null;
        }

        private void EnableFields() {

            //Comboboxes
            cbHasUser.IsEnabled = true;
            cbActive.IsEnabled = true;

            //Campos de texto
            NameEdit.IsEnabled = true;
            cpfEdit.IsEnabled = true;
            AdressEdit.IsEnabled = true;
            NeighboorhoodEdit.IsEnabled = true;
            NumberEdit.IsEnabled = true;
            cepEdit.IsEnabled = true;
            admissionEdit.IsEnabled = true;
            RoleEdit.IsEnabled = true;
            usernameEdit.IsEnabled = true;
            passwordEdit.IsEnabled = true;

            //Botões
            btnCancelar.IsEnabled = true;
            btnSalvar.IsEnabled = true;
        }

        private void BlockFields() {

            //Comboboxes
            cbHasUser.IsEnabled = false;
            cbActive.IsEnabled = false;

            //Campos de texto
            NameEdit.IsEnabled = false;
            cpfEdit.IsEnabled = false;
            AdressEdit.IsEnabled = false;
            NeighboorhoodEdit.IsEnabled = false;
            NumberEdit.IsEnabled = false;
            cepEdit.IsEnabled = false;
            admissionEdit.IsEnabled = false;
            RoleEdit.IsEnabled = false;
            usernameEdit.IsEnabled = false;
            passwordEdit.IsEnabled = false;

            //Botões
            btnCancelar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
        }

        #endregion 
    }
}
