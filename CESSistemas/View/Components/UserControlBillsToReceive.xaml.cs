using System.Windows.Controls;

namespace Promig.View.Components {

    public partial class UserControlBillsToReceive : UserControl {

        #region static-instance

        //Definindo instancia para componente
        private static UserControlBillsToReceive _instance;

        //Definindo método para recuperar instancia
        public static UserControlBillsToReceive GetInstance() {
            if (_instance == null) _instance = new UserControlBillsToReceive();
            return _instance;
        }

        #endregion static-instance

        #region constructors

        public UserControlBillsToReceive() {
            InitializeComponent();
        }

        #endregion constructors
    }
}
