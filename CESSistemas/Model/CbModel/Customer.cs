namespace Promig.Model.CbModel {

    class Customer {

        public int id { get; set; }
        public string name { get; set; }

        public Customer() { }

        public Customer(int id, string name) {
            this.id = id;
            this.name = name;
        }
    }
}
