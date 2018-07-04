using System.Windows.Controls;

namespace Promig.View.Components {
    
    public partial class UserControlMain : UserControl {

        #region static-instance

        //Definindo instancia do componente
        private static UserControlMain _instance;

        //Definindo método para recuperar instancia
        public static UserControlMain GetInstance() {
            if (_instance == null) _instance = new UserControlMain();
            return _instance;
        }

        #endregion static-instance

        #region constructors

        public UserControlMain() {
            InitializeComponent();
        }

        #endregion constructors
    }
}
