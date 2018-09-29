using Geocoding;
using Geocoding.Microsoft;
using Promig.Utils;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Promig.View {
    
    public partial class MapWindow : Window {

        #region Fields
        private string location;
        private double zoom;
        private Microsoft.Maps.MapControl.WPF.Location loc;
        #endregion

        #region Constructors

        /// <summary>
        /// Construtor padrão sem argumentos
        /// </summary>
        public MapWindow() {
            //Inicializando componentes
            InitializeComponent();

            //Definindo valores padrão
            zoom = 17.0;
            location = "";
        }

        /// <summary>
        /// Construtor utilizado com argumentos
        /// </summary>
        /// <param name="location"></param>
        public MapWindow(string location) {
            //Inicializando componentes
            InitializeComponent();

            //Definindo valores padrão
            zoom = 17.0;
            this.location = location;
        }

        #endregion

        #region Events

        /// <summary>
        /// Evento ao carregar tela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void control_loaded(object sender, RoutedEventArgs e) {
            SetLocation();
        }

        /// <summary>
        /// Evento ao pressionar botão voltar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /// <summary>
        /// Evento para bitão de gerar pdf de mapa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, RoutedEventArgs e) {
            if (isNetWorkConnection()) PrintMap();
            else MessageBox.Show("Sem conexão com a internet para imprimir mapa!", "Alerta!",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Evento para fechar a janela ao clicar fora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void control_Deactivated(object sender, EventArgs e) {
            try {
                Close();
            }
            catch (Exception ex) {
                Log.logException(ex);
                Log.logMessage(ex.Message);
            }
        }

        /// <summary>
        /// Evento para o botão de recentralizar mapa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e) {
            if (isNetWorkConnection()) Refresh();
            else MessageBox.Show("Sem conexão com a internet para restaurar mapa!", "Alerta!",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        #endregion

        #region MapsAPI

        /// <summary>
        /// Método para visualizar localização passada como parametro no construtor
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

        /// <summary>
        /// Método para imprimir mapa em pdf
        /// </summary>
        private void PrintMap() {
            // Exportando mapa para arquivo pdf
            reportLabel.Content = PdfModel.ExportMapPdf(location, loc);
        }

        /// <summary>
        /// Método para recentralizar mapa
        /// </summary>
        private void Refresh() {
            bingMap.SetView(loc, zoom);    //Visualização em mapa
        }

        #endregion

        #region Utils

        /// <summary>
        /// Método para verificar conexão com internet
        /// </summary>
        /// <returns></returns>
        private bool isNetWorkConnection() {
            if (NetworkInterface.GetIsNetworkAvailable()) {
                return true;
            } else {
                return false;
            }
        }

        #endregion
    }
}
