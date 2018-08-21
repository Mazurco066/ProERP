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

        /// <summary>
        /// Método para verificar existencia do arquivo de preferencias
        /// </summary>
        /// <returns></returns>
        public static bool PreferencesExists() {

            //Verificando se ja existe um arquivo de preferencias
            if (!Directory.Exists(rawPath)) {
                Directory.CreateDirectory(rawPath);
                return false;
            }
            if (!File.Exists(path)) return false;

            return true;
        }

        /// <summary>
        /// Método para criar arquivo de preferencias com valores pré-definidos
        /// </summary>
        public static void CreatePreferences() {

            //Criando dados pré definidos da empresa
            var model = new CompanyModel {
                name = Crypt.Encrypt("Promig Serralheria"),
                cnpj = Crypt.Encrypt("11111111111111"),
                street = Crypt.Encrypt("Rua Eustorgio Coelho"),
                neighborhood = Crypt.Encrypt("Parque do Estado II"),
                number = Crypt.Encrypt("82"),
                city = Crypt.Encrypt("Mogi Mirim"),
                CEP = Crypt.Encrypt("13807698"),
                UF = Crypt.Encrypt("SP"),
                phone1 = Crypt.Encrypt("(19)97181-8810"),
                phone2 = Crypt.Encrypt("(19)99253-9978"),
                phone3 = Crypt.Encrypt("(19)35691-924"),
                email = Crypt.Encrypt("promig.me@gmail.com"),
                website = Crypt.Encrypt("www.promig.com.br")
            };
                
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(model, serializerSettings);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Método para retornar endereço no formato correto para api de mapas
        /// </summary>
        /// <returns></returns>
        public static string GetFormatedAdress() {
            //Procurando o arquivo json de preferencias pelo caminho de diretório
            using (StreamReader r = new StreamReader(path)) {

                //Lendo arquivo json e fazendo o parse do mesmo
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);

                //Instanciando um modelo vazio para preenchimento
                CompanyModel model = new CompanyModel();

                //Preenchendo os dados presentes no json no modelo definido
                foreach (var item in jobj.Properties()) {
                    switch (item.Name) {
                        case "street":  //Propriedade Rua
                            model.street = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "neighborhood":    //Propriedade Bairro
                            model.neighborhood = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "number":  //Propriedade Número
                            model.number = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "city":    //Proprieade Cidade
                            model.city = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "cep:":   //Propriedade CEP
                            model.CEP = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "uf":  //Propriedade UF
                            model.UF = Crypt.Decrypt(item.Value.ToString());
                            break;
                    }
                }

                //Retornando endereco formatado do modelo recuperado
                return $"{model.street}, {model.number} {model.neighborhood}, {model.city}-{model.UF}";
            }
        }

        public static string GetPdfFooterData() {
            //Procurando o arquivo json de preferencias pelo caminho de diretório
            using (StreamReader r = new StreamReader(path)) {

                //Lendo arquivo json e fazendo o parse do mesmo
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);

                //Instanciando um modelo vazio para preenchimento
                CompanyModel model = new CompanyModel();

                //Preenchendo os dados presentes no json no modelo definido
                foreach (var item in jobj.Properties()) {
                    switch (item.Name) {
                        case "phone1":  //Propriedade Telefone 1
                            model.phone1 = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "phone2":    //Propriedade Telefone 2
                            model.phone2 = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "phone3":  //Propriedade Telefone 3
                            model.phone3 = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "email":    //Proprieade Email
                            model.email = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "website":   //Propriedade Website
                            model.website = Crypt.Decrypt(item.Value.ToString());
                            break;
                    }
                }

                //Retornando endereco formatado do modelo recuperado
                return $"Contato: Telefone1:{model.phone1} | Telefone2:{model.phone2} | Telefone3:{model.phone3} | " +
                       $"Email:{model.email} | Website:{model.website}";
            }
        }

        /// <summary>
        /// Métodopara tetornar dados do arquivo em estrutura de classe
        /// </summary>
        /// <returns></returns>
        public static CompanyModel GetPreferencesData() {

            //Procurando o arquivo json de preferencias pelo caminho de diretório
            using (StreamReader r = new StreamReader(path)) {
                
                //Lendo arquivo json e fazendo o parse do mesmo
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);

                //Instanciando um modelo vazio para preenchimento
                CompanyModel model = new CompanyModel();

                //Preenchendo os dados presentes no json no modelo definido para retorno
                foreach (var item in jobj.Properties()) {
                    switch (item.Name) {
                        case "name":    //Proprieade Nome
                            model.name = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "cnpj":   //Propriedade Cnpj
                            model.cnpj = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "street":  //Propriedade Rua
                            model.street = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "neighborhood":    //Propriedade Bairro
                            model.neighborhood = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "number":  //Propriedade Número
                            model.number = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "city":    //Proprieade Cidade
                            model.city = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "cep":   //Propriedade CEP
                            model.CEP = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "uf":  //Propriedade UF
                            model.UF = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "phone1":    //Propriedade Telefone 1
                            model.phone1 = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "phone2":  //Propriedade Telefone 2
                            model.phone2 = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "phone3":  //Propriedade Telefone 3
                            model.phone3 = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "email": //Propriedade Email
                            model.email = Crypt.Decrypt(item.Value.ToString());
                            break;
                        case "website": //Propriedade Website
                            model.website = Crypt.Decrypt(item.Value.ToString());
                            break;
                    }
                }

                //Retornando modelo preenchido
                return model;
            }
        }

        /// <summary>
        /// Método para atualizar dados do arquivo de preferencias
        /// </summary>
        /// <param name="model"></param>
        public static void UpdatePreferences(CompanyModel model) {

            //Procurando o arquivo json de preferencias pelo caminho de diretório
            using (StreamReader r = new StreamReader(path)) {

                //Lendo arquivo json e fazendo o parse do mesmo
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);

                //Preenchendo os dados presentes no json no modelo definido para retorno
                foreach (var item in jobj.Properties()) {
                    switch (item.Name) {
                        case "name":    //Proprieade Nome
                            item.Value = Crypt.Encrypt(model.name);
                            break;
                        case "cnpj":   //Propriedade Cnpj
                            item.Value = Crypt.Encrypt(model.cnpj);
                            break;
                        case "street":  //Propriedade Rua
                            item.Value = Crypt.Encrypt(model.street);
                            break;
                        case "neighborhood":    //Propriedade Bairro
                            item.Value = Crypt.Encrypt(model.neighborhood);
                            break;
                        case "number":  //Propriedade Número
                            item.Value = Crypt.Encrypt(model.number);
                            break;
                        case "city":    //Proprieade Cidade
                            item.Value = Crypt.Encrypt(model.city);
                            break;
                        case "cep":   //Propriedade CEP
                            item.Value = Crypt.Encrypt(model.CEP);
                            break;
                        case "uf":  //Propriedade UF
                            item.Value = Crypt.Encrypt(model.UF);
                            break;
                        case "phone1":    //Propriedade Telefone 1
                            item.Value = Crypt.Encrypt(model.phone1);
                            break;
                        case "phone2":  //Propriedade Telefone 2
                            item.Value = Crypt.Encrypt(model.phone2);
                            break;
                        case "phone3":  //Propriedade Telefone 3
                            item.Value = Crypt.Encrypt(model.phone3);
                            break;
                        case "email": //Propriedade Email
                            item.Value = Crypt.Encrypt(model.email);
                            break;
                        case "website": //Propriedade Website
                            item.Value = Crypt.Encrypt(model.website);
                            break;
                    }
                }

                //Fechando o processo de leitura para iniciar gravação de dados
                r.Close();  
                var result = jobj.ToString();
                File.WriteAllText(path, result);
            }
        }

        #endregion
    }
}
