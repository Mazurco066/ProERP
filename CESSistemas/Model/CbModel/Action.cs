namespace Promig.Model.CbModel {

    class Action {

        public int id { get; set; }
        public string description { get; set; }

        public Action() { }

        public Action(int id, string description) {
            this.id = id;
            this.description = description;
        }

    }
}
