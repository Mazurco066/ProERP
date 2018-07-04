using System.Windows.Controls;

namespace Promig.View.Components {
    
    public partial class UserControlEmployes : UserControl {

        #region static-instance

        //Definindo instancia estatica
        private static UserControlEmployes _instance;

        //Definindo método para recuperar instancia
        public static UserControlEmployes GetInstance() {
            if (_instance == null) _instance = new UserControlEmployes();
            return _instance;
        }

        #endregion static-instance

        #region constructors

        public UserControlEmployes() {
            InitializeComponent();
        }

        #endregion constructors
    }
}
