namespace Promig.Model {

    class Service {

        private int id;
        private string task;
        private double value;

        public Service() {

        }

        public Service(string task) {
            this.task = task;
        }

        public int Id {
            get { return id; }
            set { this.id = value; }
        }

        public string Task {
            get { return task; }
            set {this.task = value; }
        }

        public double Value {
            get { return value; }
            set { this.value = value; }
        }
    }
}
