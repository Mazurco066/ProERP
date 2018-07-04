using System.Windows.Controls;

namespace Promig.View.Components {
    
    public partial class UserControlClient : UserControl {

        #region static-instance

        //Definindo instancia do componentes
        private static UserControlClient _instance;

        //Definindo método para recuperar instancias
        public static UserControlClient GetInstance() {
            if (_instance == null) _instance = new UserControlClient();
            return _instance;
        }

        #endregion static-instance

        #region constructors

        public UserControlClient() {
            InitializeComponent();
        }

        #endregion constructors
    }
}
