using System.Collections.Generic;

namespace Promig.Model.Json {

    /*______________________________________________________________________
     |
     |                      CLASSE DE MODELAGEM DE JSON
     |
     |      Classe para configurar todo objeto json de dados da empresa
     |      que serão usados.
     |
     */
    class JsonModel {

        public string Version { get; set; }
        public IList<CompanyModel> CompanyList { get; set; } = new List<CompanyModel>();
    }
}
