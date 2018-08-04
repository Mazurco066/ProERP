using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Promig.Model.Json;
using System.Windows.Controls;
using System.Windows;
using System.IO;


namespace Promig.View.Components {
    
    public partial class UserControlAbout : UserControl {

        #region Header
        private string rawPath = "C:\\ProERP\\Preferences\\";
        private string path = "C:\\ProERP\\Preferences\\preferences.json";
        #endregion

        public UserControlAbout() {
            InitializeComponent();
        }

        #region Events

        private void control_loaded(object sender, RoutedEventArgs e) {

            //Verificando se ja existe um arquivo de preferencias
            if (!Directory.Exists(rawPath)) Directory.CreateDirectory(rawPath);
            Directory.SetCurrentDirectory(rawPath);

            //Criando arquivo de preferencias JSON
            CreateJson();
        }

        #endregion

        #region Data-Methods

        private void CreateJson() {

            //Criando dados pré definidos da empresa
            var model = new JsonModel {
                Version = "1.0.0",
                CompanyList = {
                    new CompanyModel {
                        name = "Promig Serralheria e Instalações Industriais",
                        cnpj = "12345678912345",
                        street = "rua",
                        neighborhood = "bairro",
                        number = "numero",
                        city = "cidade",
                        CEP = "cep",
                        UF = "uf",
                        phone1 = "19971818810",
                        phone2 = "19992539978",
                        phone3 = "1935691924"
                    }
                }
            };

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(model, serializerSettings);
            File.WriteAllText(path, json);
        }

        #endregion
    }
}
