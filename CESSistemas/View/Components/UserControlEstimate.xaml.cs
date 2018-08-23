using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using Promig.Connection.Methods;
using Promig.Exceptions;
using Promig.Model;
using Promig.Model.CbModel;
using Promig.Model.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Promig.Utils;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using Promig.Connection;
using System.IO;
using System.Collections.ObjectModel;

namespace Promig.View.Components {

    public partial class UserControlEstimate : UserControl {

        #region Header

        private string imgDirectoryPath;
        private Estimates dao;
        private Estimate aux;
        private Employe _employe;

        #endregion

        #region Constructor

        public UserControlEstimate() {

            // Inicializando componentes
            InitializeComponent();

            // Inicializando path's
            imgDirectoryPath = "C:\\ProERP\\Config\\Internal-Data\\";

            // Inicializando objetos
            aux = null;
            dao = new Estimates();
            _employe = new Employe();
            _employe.id = MainWindow.currentId;
        }

        #endregion

        #region Events

        /// <summary>
        /// Evento ao carregar controle de usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void control_loaded(object sender, RoutedEventArgs e) {

            // Alimentando items do combo box de clientes
            cbCustomer.ItemsSource = dao.NameCustomerList();
            cbCustomer.DisplayMemberPath = "name";
            cbCustomer.SelectedValuePath = "id";
        }

        /// <summary>
        /// Evento ao selecionar imagem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogo_Click(object sender, System.Windows.RoutedEventArgs e) {

            // Definindo uma dialog para recuperar local da imagem que o usuario quer usar
            OpenFileDialog boxDialog = new OpenFileDialog();
            boxDialog.Title = "Escolha o logo";
            boxDialog.Filter = "Imagens suportadas|*.jpg;*.jpeg;*.png|" +
                               "JPEG(*.jpeg;*.jpg)|*.jpg;*.jpeg|" +
                               "Portable Network Graphic (*.png)|*.png";
            boxDialog.ShowDialog();           
            
            // Recuperando o caminho da imagem selecionada
            string choosenImgPath = boxDialog.FileName;

            // Criando pasta de imagens se não existir
            if (!Directory.Exists(imgDirectoryPath))
                Directory.CreateDirectory(imgDirectoryPath);

            // Realizando copia da imagem para pasta dentro do sistema
            string destPath = $"{imgDirectoryPath}{DateTime.Now.ToString("ddMMyyyyhhmmss")}.jpg";
            File.Copy(choosenImgPath, destPath);
            image.Source = new BitmapImage(new Uri(destPath));
        }

        #endregion
    }
}
