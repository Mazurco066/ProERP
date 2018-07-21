using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading;
using System.Net;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Promig.View.Components {
    
    public partial class UserControlMaps : UserControl {

        #region Fields
        private XDocument geoDoc;
        private SaveFileDialog saveDialog;
        private string location;
        private int zoom;
        private string mapType;
        private double lat;
        private double lng;
        #endregion

        public UserControlMaps() {
            InitializeComponent();
        }

        private void control_loaded(object sender, RoutedEventArgs e) {
            ShowMap();
        }

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

        /*private void ShowGeoCodeData() {

            var responseStatus = geoDoc...<status>
        }*/

        private void ShowMap() {
            BitmapImage img = new BitmapImage();
            string mapURl = $"http://maps.googleapis.com/maps/api/staticmap?size=500x400&markers=size:mid%7Ccolor:red%7C{location}&zoom={zoom}&maptype={mapType}&sensor=false";
            img.BeginInit();
            img.UriSource = new System.Uri(mapURl);
            img.EndInit();
            mapImg.Source = img;
        }
    }
}
