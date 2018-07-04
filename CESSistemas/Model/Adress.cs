namespace Promig.Model {

    /*______________________________________________________________________
     |
     |                      CLASSE DE MODELAGEM DE DADOS
     |
     |      Classe para modelar objeto de endereços dentro do sistema, contendo
     |      seus respectivos atributos e métodos.
     |
     */
    class Adress {

        #region Attrib

        //Atributos
        private int id;
        private string street;
        private string neighborhood;
        private string number;
        private string city;
        private string CEP;
        private string UF;

        #endregion Attrib

        #region Constructors

        //Construtores
        public Adress() {

            this.id = -1;
            this.street = "";
            this.neighborhood = "";
            this.number = "";
            this.city = "";
            this.CEP = "";
            this.UF = "";

        }

        #endregion Constructors

        #region Getter-Setter

        public int GetId() {

            return id;
        }

        public void SetId(int id) {

            this.id = id;
        }

        public string GetStreet() {

            return street;
        }

        public void SetStreet(string street) {

            this.street = street;
        }

        public string GetNeighborhood() {

            return neighborhood;
        }

        public void SetNeighborhood(string neighborhood) {

            this.neighborhood = neighborhood;
        }

        public string GetNumber() {

            return number;
        }

        public void SetNumber(string number) {

            this.number = number;
        }

        public string GetCity() {

            return city;
        }

        public void SetCity(string city) {

            this.city = city;
        }

        public string GetCEP() {

            return this.CEP;
        }

        public void SetCEP(string CEP) {

            this.CEP = CEP;
        }

        public string GetUF() {

            return UF;
        }

        public void SetUF(string UF) {

            this.UF = UF;
        }

        #endregion Getter-Setter

        #region Class-Methods

        //Métodos gerais
        public string[] GetString() {

            string[] _return = new string[6];
            _return[0] = this.street;
            _return[1] = this.neighborhood;
            _return[2] = this.number;
            _return[3] = this.city;
            _return[4] = this.CEP;
            _return[5] = this.UF;

            return _return;
        }

        #endregion Class-Methods

    }
}
