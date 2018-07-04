using System.Windows.Controls;

namespace Promig.View.Components {
    
    public partial class UserControlSupplier : UserControl {

        #region static-instance

        //Definindo instancia estatica para tela
        private UserControlSupplier _instance;

        //Definindo método para recuperação da instancia
        public UserControlSupplier GetInstance() {
            if (_instance == null) _instance = new UserControlSupplier();
            return _instance;
        }

        #endregion static-instance

        #region constructors

        public UserControlSupplier() {
            InitializeComponent();
        }

        #endregion constructors
    }
}
