using System.Windows;
using System.Net;
using System.Xml.Linq;
using System.Windows.Media.Imaging;

namespace Promig.View {
    
    public partial class MapWindow : Window {

        #region Fields
        private XDocument geoDoc;
        private string location;
        private int zoom;
        private string mapType;
        #endregion

        #region Constructors

        public MapWindow() {
            //Inicializando componentes
            InitializeComponent();

            //Definindo valores padrão
            zoom = 18;
            mapType = "rooadmap";
            location = "";
        }

        public MapWindow(string location) {
            //Inicializando componentes
            InitializeComponent();

            //Definindo valores padrão
            zoom = 18;
            mapType = "rooadmap";
            this.location = location;
        }

        #endregion

        #region Events

        private void control_loaded(object sender, RoutedEventArgs e) {
            ShowMap();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        #endregion

        #region MapsAPI

        private void GetGeocodeData() {
            string geocodeUrl = $"http://maps.googleapis.com/maps/api/geocode/xml?address={location}&sensor=false";
            try {
                geoDoc = XDocument.Load(geocodeUrl);
            }
            catch (WebException ex) {
                MessageBox.Show(
                    ex.Message,
                    "Map App",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void ShowMap() {
            BitmapImage img = new BitmapImage();
            string mapURl = $"http://maps.googleapis.com/maps/api/staticmap?size=500x400&markers=size:mid%7Ccolor:red%7C{location}&zoom={zoom}&maptype={mapType}&sensor=false";
            img.BeginInit();
            img.UriSource = new System.Uri(mapURl);
            img.EndInit();
            mapImage.Source = img;
        }

        #endregion
    }
}
