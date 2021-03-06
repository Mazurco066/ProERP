﻿namespace Promig.Model {

    class Service {

        #region Attributtes

        private int id;
        private string task;
        private double value;

        #endregion

        #region Constructors

        public Service() { }
        public Service(string task) { this.task = task; }
        public Service(int id, string task, double value) {
            this.id = id;
            this.task = task;
            this.value = value;
        }

        #endregion

        #region Getter-Setter

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

        #endregion
    }
}
