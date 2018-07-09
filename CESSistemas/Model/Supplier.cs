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

        public int id { get; set; }
        public string cnpj { get; set; }
        public string resPhone { get; set; }
        public string cellPhone { get; set; }

        #endregion Attributes

        #region Constructors

        public Supplier() : base() {

            id = -1;
            cnpj = "";
            resPhone = "";
            cellPhone = "";
        }

        #endregion Constructors
    }

}
