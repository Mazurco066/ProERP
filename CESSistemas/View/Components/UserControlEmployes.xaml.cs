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

        #endregion Events

        #region Grid-Param

        private void RefreshGrid() {
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

        #endregion 
    }
}
