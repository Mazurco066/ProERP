using System.Windows.Controls;

namespace Promig.View.Components {
    
    public partial class UserControlUsers : UserControl {

        #region static-instance

        //Definindo instancia estatica do componente
        private static UserControlUsers _instance;

        //Definindo recuperação de instancia
        public static UserControlUsers GetInstance() {
            if (_instance == null) _instance = new UserControlUsers();
            return _instance;
        }

        #endregion static-instance

        #region constructors

        public UserControlUsers() {
            InitializeComponent();
        }

        #endregion constructors
    }
}