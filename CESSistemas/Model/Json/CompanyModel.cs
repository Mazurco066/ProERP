namespace Promig.Model.Json {

    /*______________________________________________________________________
     |
     |                      CLASSE DE MODELAGEM DE JSON
     |
     |      Classe para modelar objeto json que sera gravado e resuperado
     |      na parte de geração de notas e orçamentos.
     |
     */
    class CompanyModel {
        public string name { get; set; }
        public string cnpj { get; set; }
        public string street { get; set; }
        public string neighborhood { get; set; }
        public string number { get; set; }
        public string city { get; set; }
        public string CEP { get; set; }
        public string UF { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string phone3 { get; set; }
        public string email { get; set; }
        public string website { get; set; }
    }
}
