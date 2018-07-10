namespace Promig.Model {

    /*______________________________________________________________________
     |
     |                      CLASSE DE MODELAGEM DE DADOS
     |
     |      Classe para modelar objeto de cliente dentro do sistema, contendo
     |      seus respectivos atributos e métodos.
     |
     */
    class Client : Person {

        #region Attrib

        //Atributos
        public int id { get; set; }
        public string docNumber { get; set; }
        public string residenceNumber { get; set; }
        public string cellNumber { get; set; }
        public string stateId { get; set; }
        public string description { get; set; }
        public bool physical { get; set; }

        #endregion Attrib

        #region Constructors

        //Construtores
        public Client() : base() {

            this.id = -1;
            this.docNumber = "";
            this.residenceNumber = "";
            this.cellNumber = "";
            this.description = "";
            this.physical = true;
        }

        #endregion Constructors

        #region Getter-Setter

        //Getter/Setter

        public bool IsPhysical() {

            return physical;
        }

        public void SetPhysical(bool physical) {

            this.physical = physical;
        }

        #endregion Getter-Setter
    }
}
