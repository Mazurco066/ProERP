using System.Windows.Controls;
using Promig.Utils;

namespace Promig.View.Components {
    
    public partial class UserControlEmployes : UserControl {

        #region constructors

        public UserControlEmployes() {
            InitializeComponent();
            SetDefaults();
        }

        #endregion constructors

        #region Events

        private void cbHasUser_SelectionChanged(object sender, SelectionChangedEventArgs args) {

            if (cbHasUser.SelectedIndex > 1)
                gridUser.Opacity = 0;
            else
                gridUser.Opacity = 100;
            
        }

        #endregion Events

        #region Utils

        private void SetDefaults() {

            //Definindo valor inicial de combobox
            cbActive.SelectedIndex = 0;
            cbHasUser.SelectedIndex = 0;
            cbState.SelectedIndex = 0;

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

        #endregion Utils
    }
}
