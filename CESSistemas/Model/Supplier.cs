namespace Promig.Model {

    /*______________________________________________________________________
     |
     |                      CLASSE DE MODELAGEM DE DADOS
     |
     |      Classe para modelar objeto de fornecedor dentro do sistema, contendo
     |      seus respectivos atributos e métodos.
     |
     */
    class Supplier : Person {

        #region Attributes

        private int id;
        private string cnpj;
        private string resPhone;
        private string cellPhone;

        #endregion Attributes

        #region Constructors

        public Supplier() : base() {

            id = -1;
            cnpj = "";
            resPhone = "";
            cellPhone = "";
        }

        #endregion Constructors

        #region Getter-Setter

        public int GetId() { return id; }

        public void SetId(int id) { this.id = id; }

        public string GetCnpj() { return cnpj; }

        public void SetCnpj(string cnpj) { this.cnpj = cnpj; }

        public string GetResPhone() { return resPhone; }

        public void SetResPhone(string resPhone) { this.resPhone = resPhone; }

        public string GetCellPhone() { return cellPhone; }

        public void SetCellPhone(string cellPhone) { this.cellPhone = cellPhone; }

        #endregion Getter-Setter

        #region Class-Methods

        public string[] ToStringArray() {

            //Definindo vetor que será retornado
            string[] _return = new string[4];

            //Preenchendo dados do cliente
            _return[0] = GetId().ToString();
            _return[1] = GetName();
            _return[2] = GetAdress().GetCity();
            _return[3] = GetCnpj();

            //Retornando informações do cliente para grid
            return _return;

        }

        #endregion Class-Methods

    }

}
