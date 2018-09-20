using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Drawing;
using System.Text.RegularExpressions;
using static System.Console;
using System.Net.NetworkInformation;

namespace Promig.View.Components {
    
    public partial class UserControlMain : UserControl {

        #region constructors

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public UserControlMain() {
            InitializeComponent();
            ShowFeed();
        }

        #endregion constructors

        #region Methods

        private void ShowFeed() {
            try {

                if (!isNetWorkConnection()) {
                    MessageBox.Show("Sem conexão com a internet!", "Alerta!",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }

                using (var reader = System.Xml.XmlReader.Create("http://g1.globo.com/dynamo/economia/rss2.xml")) {
                    System.ServiceModel.Syndication.SyndicationFeed feed = System.ServiceModel.Syndication.SyndicationFeed.Load(reader);
                    foreach (var item in feed.Items) {
                        int s, f;
                        string noticia = "NOTÍCIA: " + item.Title.Text;
                        noticia += "\n";
                        noticia += string.Format("FONTE: {0}", item.Links.First().Uri.ToString());
                        noticia += "\n";
                        if (item.Summary != null) {
                            //noticia += item.Summary.Text;
                            s = item.Summary.Text.IndexOf("<");
                            f = item.Summary.Text.IndexOf(">");
                            if (f != -1) {
                                noticia += item.Summary.Text.Substring(f + 7);
                            }
                        }
                        noticia += "\n______________________________________________________________________________";
                        noticia += "______________________________________________________________________________";
                        noticia += "______________________________________________________________________________";
                        noticia += "______________________________________________________________________________";

                        lsFeed.Items.Add(noticia);
                    }
                }
            }
            catch (Exception) {
                MessageBox.Show("Não é possivel carregar notícias, contacte o administrador!", "Informação",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool isNetWorkConnection() {
            if (NetworkInterface.GetIsNetworkAvailable()) {
                stWifiOn.Visibility = Visibility.Visible;
                stWifiOf.Visibility = Visibility.Hidden;
                return true;
            }
            else {
                stWifiOf.Visibility = Visibility.Visible;
                stWifiOn.Visibility = Visibility.Hidden;
                return false;
            }
        }


        #endregion

        #region Events

        private void btnLimpar_Click(object sender, RoutedEventArgs e) {
            lsFeed.Items.Clear();
        }

        private void btnCarregar_Click(object sender, RoutedEventArgs e) {
            ShowFeed();
        }

        #endregion
    }
}
