using System.Windows.Controls;

namespace Promig.View.Components {
    
    public partial class UserControlBillsToPay : UserControl {

        #region static-instance

        //Definindo instancia para componente
        private static UserControlBillsToPay _instance;

        //Definindo método para recuperar instancia
        public static UserControlBillsToPay GetInstance() {
            if (_instance == null) _instance = new UserControlBillsToPay();
            return _instance;
        }

        #endregion static-instance

        #region constructors

        public UserControlBillsToPay() {
            InitializeComponent();
        }

        #endregion constructors
    }
}
