using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading;
using System.Net;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using System.Xml.Linq;

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

        private void GeoSubData() {

        }
    }
}
