using System.Windows.Controls;

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
            cbActive.SelectedIndex = 0;
            cbHasUser.SelectedIndex = 0;
        }

        #endregion Utils
    }
}
