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
    /// Lógica interna para SearchCustomer.xaml
    /// </summary>
    public partial class SearchCustomer : Window {

        #region Header
        private Clients dao;
        UserControlBillsToReceive window;

        public SearchCustomer(UserControlBillsToReceive window) {
            InitializeComponent();

            dao = new Clients();
            this.window = window;
            RefreshGrid();
        }
        #endregion

        #region Grid-Param

        /// <summary>
        /// Método para atualizar conteúdo da grid
        /// </summary>
        private void RefreshGrid() {

            //Limpando campo de pesquisa
            txtSearch.Text = null;

            try {

                //Filtros de busca
                switch (cbSearch.SelectedIndex) {

                    case 0: //Ativo - nome
                        dgClients.ItemsSource = dao.GetAllActiveClients(txtSearch.Text);
                        break;

                    case 1: //Todos - nome
                        dgClients.ItemsSource = dao.GetAllClients(txtSearch.Text);
                        break;

                    case 2: //Ativo - Cidade
                        dgClients.ItemsSource = dao.GetAllActiveClientsByCity(txtSearch.Text);
                        break;

                    case 3: //Ativo - CPF
                        dgClients.ItemsSource = dao.GetAllActiveClientsByDocument(txtSearch.Text);
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
        /// Método para atualizar conteúdo da grid com filtro de busca
        /// </summary>
        /// <param name="param">Conteúdo a ser buscado nos registros</param>
        private void RefreshGrid(string param) {

            try {

                //Filtros de busca
                switch (cbSearch.SelectedIndex) {

                    case 0: //Ativo - nome
                        dgClients.ItemsSource = dao.GetAllActiveClients(param);
                        break;

                    case 1: //Todos - nome
                        dgClients.ItemsSource = dao.GetAllClients(param);
                        break;

                    case 2: //Ativo - Cidade
                        dgClients.ItemsSource = dao.GetAllActiveClientsByCity(param);
                        break;

                    case 3: //Ativo - CPF
                        dgClients.ItemsSource = dao.GetAllActiveClientsByDocument(param);
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

        private void dgClients_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            try {

                if (dgClients.SelectedItems.Count > 0) {
                    Client source = dgClients.SelectedItem as Client;
                    window.cliente_txt.Text = source.name;
                }

            }
            catch (MySqlException) {
                window.cliente_txt.Text = string.Empty;
                throw;
            }


        }

        private void dgClients_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (dgClients.SelectedItems.Count > 0) {
                this.Close();
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            if (dgClients.SelectedItems.Count > 0) {
                this.Close();
            }else{
                MessageBox.Show("Selecione um cliente!", "Aviso",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            window.cliente_txt.Text = string.Empty;
            this.Close();
        }

        
        private void control_Deactivated(object sender, EventArgs e) {
            try {
                window.cliente_txt.Text = string.Empty;
                Close();
            } catch (Exception ex) {
                Promig.Utils.Log.logException(ex);
                Promig.Utils.Log.logMessage(ex.Message);
            }
        }
        #endregion
    }
}
