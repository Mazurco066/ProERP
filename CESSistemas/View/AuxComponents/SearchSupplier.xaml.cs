using Promig.Exceptions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Promig.Connection.Methods;
using MySql.Data.MySqlClient;
using Promig.Model;
using Promig.View.Components;
using System;

namespace Promig.View.AuxComponents {
    /// <summary>
    /// Lógica interna para SearchSupplier.xaml
    /// </summary>
    public partial class SearchSupplier : Window {


        #region Header
        private Suppliers dao;
        UserControlBillsToPay window;

        public SearchSupplier(UserControlBillsToPay window) {
            InitializeComponent();

            dao = new Suppliers();
            this.window = window;
            RefreshGrid();
        }
        #endregion

        #region Grid-Param
        private void RefreshGrid() {

            //Limpando campo de pesquisa
            txtSearch.Text = null;

            try {

                //Filtros de busca
                switch (cbSearch.SelectedIndex) {

                    case 0: //Ativo - nome
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliers(txtSearch.Text);
                        break;

                    case 1: //Todos - nome
                        dgSuppliers.ItemsSource = dao.GetAllSuppliers(txtSearch.Text);
                        break;

                    case 2: //Ativo - Cidade
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByCity(txtSearch.Text);
                        break;

                    case 3: //Ativo - CPF
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByDocument(txtSearch.Text);
                        break;
                }
            }
            catch (DatabaseAccessException err) {
                MessageBox.Show(
                    err.Message,
                    "Problemas ao acessar o banco!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        /// <summary>
        /// Método para atualizar conteúdo da grid que recebe parametros de busca
        /// </summary>
        /// <param name="param">Conteúdo a ser buscado nos registros</param>
        private void RefreshGrid(string param) {

            try {

                //Filtros de busca
                switch (cbSearch.SelectedIndex) {

                    case 0: //Ativo - nome
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliers(param);
                        break;

                    case 1: //Todos - nome
                        dgSuppliers.ItemsSource = dao.GetAllSuppliers(param);
                        break;

                    case 2: //Ativo - Cidade
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByCity(param);
                        break;

                    case 3: //Ativo - CPF
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByDocument(param);
                        break;
                }
            }
            catch (DatabaseAccessException err) {
                MessageBox.Show(
                    err.Message,
                    "Problemas ao acessar o banco!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
        #endregion

        #region Events

        private void control_loaded(object sender, RoutedEventArgs e) {
            cbSearch.SelectedIndex = 0;
            RefreshGrid();
        }

        private void cbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RefreshGrid();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e) {
            RefreshGrid(txtSearch.Text);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            RefreshGrid();
        }

        private void dgSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            try {

                if (dgSuppliers.SelectedItems.Count > 0) {
                    Supplier source = dgSuppliers.SelectedItem as Supplier;
                    window.fornecedor_txt.Text = source.name;
                }

            }
            catch (MySqlException) {
                window.fornecedor_txt.Text = string.Empty;
                throw;
            }

        }

        private void dgSuppliers_MouseDoubleClick(object sender, MouseButtonEventArgs e) {

          if(dgSuppliers.SelectedItems.Count > 0){
                this.Close();
          }

        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            if (dgSuppliers.SelectedItems.Count > 0) {
                this.Close();
            }else{
                MessageBox.Show("Selecione um fornecedor!", "Aviso",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            window.fornecedor_txt.Text = string.Empty;
            this.Close();
        }

        private void control_Deactivated(object sender, EventArgs e) {
            try {
                window.fornecedor_txt.Text = string.Empty;
                Close();
            } catch (Exception ex) {
                Promig.Utils.Log.logException(ex);
                Promig.Utils.Log.logMessage(ex.Message);
            }
        }

        #endregion
    }
}
