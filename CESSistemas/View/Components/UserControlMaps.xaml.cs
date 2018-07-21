﻿using System.Windows.Controls;
using System.Net;
using Microsoft.Win32;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Promig.View.Components {
    
    public partial class UserControlMaps : UserControl {

        #region Fields
        private XDocument geoDoc;
        private string location;
        private int zoom;
        private string mapType;
        #endregion

        public UserControlMaps() {
            InitializeComponent();
        }

        private void control_loaded(object sender, RoutedEventArgs e) {
            zoom = 17;
            mapType = "rooadmap";
            location = "José Cristino de Oliveira Campos, 562 Jardim Selma, Mogi Guaçu-SP";
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
