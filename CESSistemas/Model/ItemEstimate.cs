namespace Promig.Model {

    class ItemEstimate {

        #region Attributes

        private Service service;
        private int amount;

        #endregion

        #region Constructors

        public ItemEstimate() { }
        public ItemEstimate(Service service, int amount) {
            this.service = service;
            this.amount = amount;
        }

        #endregion

        #region Getter-Setter

        public Service Service {
            get { return service; }
            set { service = value; }
        }

        public int Amount {
            get { return amount; }
            set { amount = value; }
        }

        #endregion
    }
}
