using Geocoding;
using Geocoding.Microsoft;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Promig.Utils;
using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;


namespace Promig.View {
    
    public partial class MapWindow : Window {

        #region Fields
        private string location;
        private double zoom;
        private Microsoft.Maps.MapControl.WPF.Location loc;
        #endregion

        #region Constructors

        public MapWindow() {
            //Inicializando componentes
            InitializeComponent();

            //Definindo valores padrão
            zoom = 17.0;
            location = "";
        }

        public MapWindow(string location) {
            //Inicializando componentes
            InitializeComponent();

            //Definindo valores padrão
            zoom = 17.0;
            this.location = location;
        }

        #endregion

        #region Events

        //Evento ao carregar janela a qual chamara metodo para setar localização
        private void control_loaded(object sender, RoutedEventArgs e) {
            SetLocation();
        }

        //Evento para o botão voltar da janela
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        //Evento para imprimir visualização de mapa atual
        private void btnPrint_Click(object sender, RoutedEventArgs e) {
            PrintMap();
        }

        //Evento para fechar janela ao clicar fora
        private void control_Deactivated(object sender, EventArgs e) {
            try {
                Close();
            }
            catch (Exception ex) {
                Log.logException(ex);
                Log.logMessage(ex.Message);
            }
        }

        //Evento ao recentralizar o mapa na localização
        private void btnRefresh_Click(object sender, EventArgs e) {
            Refresh();
        }

        #endregion

        #region MapsAPI

        //Metodo para posicionar o mapa na localização passada por endereço
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

        private void PrintMap() {

            //Criando um novo documento com margem 40
            Document document = new Document(PageSize.A4);
            document.SetMargins(40, 40, 40, 40);
            document.AddCreationDate();

            //Definindo o caminho que será salvo o arquivo PDF            
            string rawPath = $"C:\\ProERP\\PDFMaps\\";

            //Criando diretório para armazenar mapas se não existir
            if (!Directory.Exists(rawPath)) Directory.CreateDirectory(rawPath);

            //Configurando caminho do novo arquivo pdf a ser salvo
            Directory.SetCurrentDirectory(rawPath);
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"{DateTime.Now.ToString("ddMMyyyyhhmmss")}roadmap.pdf");

            //Criando arquivo em branco para testes
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
        }

        //Metodo para ver coordenadas previamente encontradas no metodo SetLocation()
        private void Refresh() {
            bingMap.SetView(loc, zoom);    //Visualização em mapa
        }

        #endregion
    }
}
