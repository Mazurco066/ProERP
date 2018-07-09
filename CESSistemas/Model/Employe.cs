namespace Promig.Model {

    /*______________________________________________________________________
     |
     |                      CLASSE DE MODELAGEM DE DADOS
     |
     |      Classe para modelar objeto de funcionário dentro do sistema, contendo
     |      seus respectivos atributos e métodos.
     |
     */
    class Employe : Person {

        #region Attributes

        public int id { get; set; }
        public string cpf { get; set; }
        public string rg { get; set; }
        public string role { get; set; }
        public string admission { get; set; }
        public string demission { get; set; }
        public string job { get; set; }
        public User user { get; set; }

        #endregion Attributes

        #region Constructors

        public Employe() {

            this.id = -1;
            base.SetIdPerson(-1);
            base.SetName("model");
            this.role = "User";
            this.cpf = "";
            this.rg = "";
            this.admission = "";
            this.demission = "";
            this.job = "";
            this.user = null;
        }

        #endregion Constructors
    }
}
