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
        public int GetId() {

            return id;
        }

        public void SetId(int id) {

            this.id = id;
        }

        public void SetDocNumber(string docNumber) {

            this.docNumber = docNumber;
        }

        public string GetDocNumber() {

            return docNumber;
        }

        public void SetResidenceNumber(string residenceNumber) {

            this.residenceNumber = residenceNumber;
        }

        public string GetResidenceNumber() {

            return residenceNumber;
        }

        public void SetCellNumber(string cellNumber) {

            this.cellNumber = cellNumber;
        }

        public string GetCellNumber() {

            return cellNumber;
        }

        public void SetStateId(string stateId) {

            this.stateId = stateId;
        }

        public string GetStateId() {

            return stateId;
        }

        public void SetDescription(string description) {

            this.description = description;
        }

        public string GetDescription() {

            return description;
        }

        public bool IsPhysical() {

            return physical;
        }

        public void SetPhysical(bool physical) {

            this.physical = physical;
        }

        #endregion Getter-Setter

        #region Class-Methods

        //Método para impressão na grid
        public string[] ToStringArray() {

            //Definindo vetor que será retornado
            string[] _return = new string[5];

            //Preenchendo dados do cliente
            _return[0] = GetId().ToString();
            _return[1] = GetName();
            _return[2] = GetAdress().GetCity();
            _return[3] = docNumber;
            _return[4] = residenceNumber;

            //Retornando informações do cliente para grid
            return _return;

        }

        #endregion Class-Methods

    }
}
