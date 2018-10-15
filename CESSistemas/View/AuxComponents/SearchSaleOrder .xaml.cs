using Promig.Exceptions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Promig.Connection.Methods;
using MySql.Data.MySqlClient;
using Promig.Model;
using Promig.View.Components;
using System;
using System.Collections.Generic;

namespace Promig.View.AuxComponents {
    /// <summary>
    /// Lógica interna para SearchSupplier.xaml
    /// </summary>
    public partial class SearchSaleOrder : Window {


        #region Header
        private SaleOrders dao;
        private Estimates dao2;
        private Services dao3;
        UserControlSalesOrder window;
        public static int id_estimate = 0;

        public SearchSaleOrder(UserControlSalesOrder window) {
            InitializeComponent();

            dao = new SaleOrders();
            dao2 = new Estimates();
            dao3 = new Services();
            this.window = window;
            RefreshGrid();
        }
        #endregion

        #region Grid-Param

        private void RefreshGrid() {

            try {

                dgSaleOrder.ItemsSource = dao.GetAllSaleOrder();   

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
            RefreshGrid();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            if (dgSaleOrder.SelectedItems.Count > 0) {
                this.Close();
            }else{
                MessageBox.Show("Selecione uma venda!", "Aviso",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e) {

            this.Close();
        }

        private void control_Deactivated(object sender, EventArgs e) {
            try {

                Close();
            } catch (Exception ex) {
                Promig.Utils.Log.logException(ex);
                Promig.Utils.Log.logMessage(ex.Message);
            }
        }

        private void dgSaleOrder_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {

                if (dgSaleOrder.SelectedItems.Count > 0) {
                    SaleOrder saleOrder = dgSaleOrder.SelectedItem as SaleOrder;
                    window.dpSaleOrder.Text = saleOrder.Date_realization;
                    window.cbStuation.Text = saleOrder.Situation;
                    window.txtDiscount.Text = saleOrder.Discount.ToString();
                    window.txtTotalValueDiscount.Text = saleOrder.TotalDiscount.ToString();
                    window.txtSaleOrderNo.Text = saleOrder.No_saleOrder.ToString();
                    List<Estimate> list = dao2.GetAllEstimatesSaleOrder(saleOrder.No_estimate);
                    foreach (Estimate item in list) {
                        window.txtDescription.Text = item.Description;
                        window.txtCliente.Text = item.NameCustomer;
                        window.txtTotalValue.Text = item.TotalValue2.ToString();
                    }
                    window.lsSaleOrder.ItemsSource = dao3.GetTaskService(saleOrder.No_estimate);
                    id_estimate = saleOrder.No_estimate;
                }

            }
            catch (MySqlException) {

                throw;
            }
        }

        #endregion
    }
}
