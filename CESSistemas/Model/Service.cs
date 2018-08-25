namespace Promig.Model {

    class Service {

        private string task;

        public string Task {
            get { return task; }
            set {this.task = value; }
        }

        public Service(string task) {
            this.task = task;
        }
    }
}
