using Promig.Model.Json;
using Promig.Utils;
using System.Windows.Controls;
using System.Windows;


namespace Promig.View.Components {
    
    public partial class UserControlAbout : UserControl {

        public UserControlAbout() {
            InitializeComponent();
        }

        #region Events

        private void control_loaded(object sender, RoutedEventArgs e) {

            //Verificando se ja existe um arquivo de preferencias
            if (!CompanyData.PreferencesExists())
                //Se não o arquivo é criado
                CompanyData.CreatePreferences();

            //Preenchendo dados da empresa
            FillData(CompanyData.GetPreferencesData());
        }

        #endregion

        #region Utils

        //Método para preencher campos com dados do modelo passado como parametro
        private void FillData(CompanyModel model) {
            NameEdit.Text = model.name;
            cnpjEdit.Text = model.cnpj;
            AdressEdit.Text = model.street;
            NeighboorhoodEdit.Text = model.neighborhood;
            NumberEdit.Text = model.number;
            CityEdit.Text = model.city;
            cepEdit.Text = model.CEP;
            phone1Edit.Text = model.phone1;
            phone1Edit2.Text = model.phone2;
            phone1Edit3.Text = model.phone3;
            //Recuperando Estado
            int index = -1;
            foreach (ComboBoxItem item in cbState.Items) {
                index++;
                if (item.Content.Equals(model.UF)) break;
            }
            cbState.SelectedIndex = index;
        }

        #endregion
    }
}
