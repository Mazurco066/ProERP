using Geocoding;
using Geocoding.Microsoft;
using Promig.Model;
using Promig.Model.Json;
using Promig.Utils;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Input;


namespace Promig.View.Components {
    
    public partial class UserControlAbout : UserControl {

        #region Header

        private string location;
        private double zoom;
        private Microsoft.Maps.MapControl.WPF.Location loc;

        #endregion

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public UserControlAbout() {

            //Inicializando componentes
            InitializeComponent();

            //Definindo valores padrões
            zoom = 17.0;
            location = "";
        }

        #region Events

        /// <summary>
        /// Evento ao carregar a tela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void control_loaded(object sender, RoutedEventArgs e) {

            //Verificando se ja existe um arquivo de preferencias
            if (!CompanyData.PreferencesExists())
                //Se não o arquivo é criado
                CompanyData.CreatePreferences();

            //Preenchendo dados da empresa
            FillData(CompanyData.GetPreferencesData());

            //Visualizando localização da empresa em mapa
            location = CompanyData.GetFormatedAdress();
            SetLocation();
        }

        /// <summary>
        /// Método para atualizar visualização em mapa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh(object sender, RoutedEventArgs e) {
            bingMap.SetView(loc, zoom);    //Visualização em mapa
        }

        /// <summary>
        /// Evento ao digitar no campo número para bloquear caracteres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberEdit_KeyDown(object sender, KeyEventArgs e) {
            KeyConverter kv = new KeyConverter();
            if ((char.IsNumber((string)kv.ConvertTo(e.Key, typeof(string)), 0) == false)) {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Evento ao apertar enter na  caixa de texto do CEP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cepEdit_PreviewKeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                string cep = cepEdit.Text.Replace("-", "").Replace("_", "");
                Adress adress = WSAdress.GetAdress(cep);
                //Recuperando dados gerais
                AdressEdit.Text = adress.street;
                NeighboorhoodEdit.Text = adress.neighborhood;
                CityEdit.Text = adress.city;
                //Recuperando Estado
                int index = -1;
                foreach (ComboBoxItem item in cbState.Items) {
                    index++;
                    if (item.Content.Equals(adress.UF)) break;
                }
                cbState.SelectedIndex = index;
            }
        }

        /// <summary>
        /// Evento ao apertar botão salvar preferencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalvar_Click(object sender, RoutedEventArgs e) {

            //Verificando se há campos vazios
            if (IsFilledFields()) {

                //Instanciando um modelo para ser armazenado os novos dados
                CompanyModel model = new CompanyModel();

                //Preenchendo os novos dados
                ComboBoxItem selected = cbState.Items[cbState.SelectedIndex] as ComboBoxItem;
                model.name = NameEdit.Text;
                model.cnpj = cnpjEdit.Text;
                model.street = AdressEdit.Text;
                model.neighborhood = NeighboorhoodEdit.Text;
                model.city = CityEdit.Text;
                model.number = NumberEdit.Text;
                model.CEP = cepEdit.Text;
                model.UF = selected.Content.ToString();
                model.phone1 = phone1Edit.Text;
                model.phone2 = phone1Edit2.Text;
                model.phone3 = phone1Edit3.Text;
                model.email = EmailEdit.Text;
                model.website = WebsiteEdit.Text;

                //Chamando método de atualização de dados
                CompanyData.UpdatePreferences(model);

                //Atualizando informações do mapa
                location = CompanyData.GetFormatedAdress();
                SetLocation();

                //Retornando mensagem de sucesso
                MessageBox.Show(
                    "Dados da empresa atualizados com sucesso!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            else {
                MessageBox.Show(
                    "Há Campos Vazios",
                    "Validação de Entrada",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        #endregion

        #region Map-Mathods

        /// <summary>
        /// Método para visualizar no mapa localização desejada
        /// </summary>
        private void SetLocation() {

            //Instanciando objeto para recuperar as cordenadas
            IGeocoder geocoder = new BingMapsGeocoder(
                "AsHgFB0MOC02SgIYNbIwV9WOuo94eLp3brN5PvlD9Vu-p9DSjVUYfUZZIS5jfOeb"
            );
            IEnumerable<Address> results = geocoder.Geocode(this.location);

            //Recuperando primeiro endereco
            loc = new Microsoft.Maps.MapControl.WPF.Location();
            foreach (Address a in results) {
                loc = new Microsoft.Maps.MapControl.WPF.Location(
                    a.Coordinates.Latitude,
                    a.Coordinates.Longitude
                );
                break;
            }

            //Visualizando coordenadas encontradas no mapa com zoom proprio
            bingMap.SetView(loc, zoom);    //Visualização em mapa
            bingMarker.Location = loc;     //Marcador de destino
        }

        #endregion

        #region Utils

        /// <summary>
        /// Método para autopreencher campos de texto
        /// </summary>
        /// <param name="model"></param>
        private void FillData(CompanyModel model) {
            NameEdit.Text = model.name;
            cnpjEdit.Text = model.cnpj;
            AdressEdit.Text = model.street;
            NeighboorhoodEdit.Text = model.neighborhood;
            NumberEdit.Text = model.number;
            CityEdit.Text = model.city;
            cepEdit.Text = model.CEP;
            phone1Edit.Text = model.phone1;
            phone1Edit2.Text = model.phone2;
            phone1Edit3.Text = model.phone3;
            EmailEdit.Text = model.email;
            WebsiteEdit.Text = model.website;

            //Recuperando Estado
            int index = -1;
            foreach (ComboBoxItem item in cbState.Items) {
                index++;
                if (item.Content.Equals(model.UF)) break;
            }
            cbState.SelectedIndex = index;
        }

        /// <summary>
        /// Método para verificar se os campos foram preenchidos
        /// </summary>
        /// <returns></returns>
        private bool IsFilledFields() {
            return !(
                NameEdit.Text.Equals(string.Empty) ||
                cnpjEdit.Text.Equals(string.Empty) ||
                AdressEdit.Text.Equals(string.Empty) ||
                NeighboorhoodEdit.Text.Equals(string.Empty) ||
                NumberEdit.Text.Equals(string.Empty) ||
                cepEdit.Text.Equals(string.Empty) ||
                CityEdit.Text.Equals(string.Empty) ||
                phone1Edit.Text.Equals(string.Empty) ||
                phone1Edit2.Text.Equals(string.Empty) ||
                EmailEdit.Text.Equals(string.Empty) ||
                WebsiteEdit.Text.Equals(string.Empty)
            );
        }

        #endregion
    }
}
