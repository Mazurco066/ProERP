using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Promig.Model.Json;
using System.IO;


namespace Promig.Utils {

    class CompanyData {

        #region Header

        private static string rawPath = "C:\\ProERP\\Preferences\\";
        private static string path = "C:\\ProERP\\Preferences\\preferences.json";

        #endregion


        #region Utils

        public static bool PreferencesExists() {

            //Verificando se ja existe um arquivo de preferencias
            if (!Directory.Exists(rawPath)) {
                Directory.CreateDirectory(rawPath);
                return false;
            }
            if (!File.Exists(path)) return false;

            return true;
        }

        public static void CreatePreferences() {

            //Criando dados pré definidos da empresa
            var model = new CompanyModel {
                name = "Promig Serralheria e Instalações Industriais",
                cnpj = "11111111111111",
                street = "Rua Eustórgio Coelho",
                neighborhood = "Parque do Estado II",
                number = "82",
                city = "Mogi Mirim",
                CEP = "13807698",
                UF = "SP",
                phone1 = "19971818810",
                phone2 = "19992539978",
                phone3 = "1935691924"
            };
                
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(model, serializerSettings);
            File.WriteAllText(path, json);
        }

        public string GetFormatedAdress() {
            //Procurando o arquivo json de preferencias pelo caminho de diretório
            using (StreamReader r = new StreamReader("C:\\ProERP\\Preferences\\preferences.json")) {

                //Lendo arquivo json e fazendo o parse do mesmo
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);

                //Instanciando um modelo vazio para preenchimento
                CompanyModel model = new CompanyModel();

                //Preenchendo os dados presentes no json no modelo definido
                foreach (var item in jobj.Properties()) {
                    switch (item.Name) {
                        case "street":  //Propriedade Rua
                            model.street = item.Value.ToString();
                            break;
                        case "neighborhood":    //Propriedade Bairro
                            model.neighborhood = item.Value.ToString();
                            break;
                        case "number":  //Propriedade Número
                            model.number = item.Value.ToString();
                            break;
                        case "city":    //Proprieade Cidade
                            model.city = item.Value.ToString();
                            break;
                        case "cep:":   //Propriedade CEP
                            model.CEP = item.Value.ToString();
                            break;
                        case "uf":  //Propriedade UF
                            model.UF = item.Value.ToString();
                            break;
                    }
                }

                //Retornando endereco formatado do modelo recuperado
                return $"{model.street}, {model.number} {model.neighborhood}, {model.city}-{model.UF}";
            }
        }

        public static CompanyModel GetPreferencesData() {

            //Procurando o arquivo json de preferencias pelo caminho de diretório
            using (StreamReader r = new StreamReader("C:\\ProERP\\Preferences\\preferences.json")) {
                
                //Lendo arquivo json e fazendo o parse do mesmo
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);

                //Instanciando um modelo vazio para preenchimento
                CompanyModel model = new CompanyModel();

                //Preenchendo os dados presentes no json no modelo definido para retorno
                foreach (var item in jobj.Properties()) {
                    switch (item.Name) {
                        case "name":    //Proprieade Nome
                            model.name = item.Value.ToString();
                            break;
                        case "cnpj":   //Propriedade Cnpj
                            model.cnpj = item.Value.ToString();
                            break;
                        case "street":  //Propriedade Rua
                            model.street = item.Value.ToString();
                            break;
                        case "neighborhood":    //Propriedade Bairro
                            model.neighborhood = item.Value.ToString();
                            break;
                        case "number":  //Propriedade Número
                            model.number = item.Value.ToString();
                            break;
                        case "city":    //Proprieade Cidade
                            model.city = item.Value.ToString();
                            break;
                        case "cep":   //Propriedade CEP
                            model.CEP = item.Value.ToString();
                            break;
                        case "uf":  //Propriedade UF
                            model.UF = item.Value.ToString();
                            break;
                        case "phone1":    //Propriedade Telefone 1
                            model.phone1 = item.Value.ToString();
                            break;
                        case "phone2":  //Propriedade Telefone 2
                            model.phone2 = item.Value.ToString();
                            break;
                        case "phone3":  //Propriedade Telefone 3
                            model.phone3 = item.Value.ToString();
                            break;
                    }
                }

                //Retornando modelo preenchido
                return model;
            }
        }

        #endregion
    }
}
