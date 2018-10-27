using Promig.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Promig.Model;
using Promig.Connection.Methods;
using Promig.View.AuxComponents;

namespace Promig.View.Components {
    /// <summary>
    /// Interação lógica para UserControl1.xam
    /// </summary>
    public partial class UserControlSalesOrder : UserControl {


        #region Header

        private SaleOrders dao;
        private Estimates dao2;
        private Services dao3;
        private SaleOrder aux;
        private Employe _employe;
        private Logs logs;
        private int actionIndex = -1;

        #endregion

        #region Constructor

        public UserControlSalesOrder() {
            InitializeComponent();
            aux = null;
            dao = new SaleOrders();
            dao2 = new Estimates();
            dao3 = new Services();
            logs = new Logs();
            _employe = new Employe();
            _employe.id = MainWindow.currentId;
        }

        #endregion

        #region Grid-Params

        private void RefreshGrid() {
            // Limpando campo de busca
            txtSearch.Text = string.Empty;
            try {

                // Recuperando todos orçamentos em modo de exibição
                dgSaleOrder.ItemsSource = dao2.GetAllEstimates(txtSearch.Text, this);

            }
            catch (DatabaseAccessException err) {
                MessageBox.Show(
                    err.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
        private void RefreshGrid(string param) {
            try { dgSaleOrder.ItemsSource = dao2.GetAllEstimates(param, this); }
            catch (DatabaseAccessException err) {
                MessageBox.Show(
                    err.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        #endregion

        #region Methods

        private void BlockFields() {

            // others components
            dpSaleOrder.IsEnabled = false;
            cbStuation.IsEnabled = false;
            txtDiscount.IsEnabled = false;

            // buttons
            btnSalvar.IsEnabled = false;
        }

        private void EnableFields() {

            // others components
            dpSaleOrder.IsEnabled = true;
            cbStuation.IsEnabled = true;
            txtDiscount.IsEnabled = true;

            // buttons
            btnSalvar.IsEnabled = true;
        }

        private void ClearFields() {
            txtSearch.Text = string.Empty;
            txtSaleOrderNo.Text = string.Empty;
            txtCliente.Text = string.Empty;
            txtTotalValue.Text = string.Empty;
            txtTotalValueDiscount.Text = string.Empty;
            txtDiscount.Text = string.Empty;
            dpSaleOrder.Text = string.Empty;
            cbStuation.Text = string.Empty;
            lsSaleOrder.ItemsSource = null;
        }

        private bool Validate() {
            if (dgSaleOrder.SelectedItems.Count > 0) {
                if ((dpSaleOrder.SelectedDate != null) &&
                  (cbStuation.SelectedItem != null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        private bool EditValidate() {
            if ((dpSaleOrder.SelectedDate != null) &&
              (cbStuation.SelectedItem != null)) {
                return true;
            }
            else {
                return false;
            }
        }

        #endregion

        #region Events

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            RefreshGrid();
            BlockFields();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            RefreshGrid(txtSearch.Text);
            txtSearch.Text = string.Empty;
        }

        private void dgSaleOrder_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (dgSaleOrder.SelectedItems.Count > 0) {
                try {

                    Estimate est = dgSaleOrder.SelectedItem as Estimate;
                    txtDescription.Text = est.Description;
                    txtCliente.Text = est.NameCustomer;
                    txtTotalValue.Text = est.TotalValue2.ToString();

                    lsSaleOrder.ItemsSource = dao3.GetTaskService(est.DocNo);

                }
                catch (DatabaseAccessException err) {
                    MessageBox.Show(
                        err.Message,
                        "Erro",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e) {
            actionIndex = 1;
            btnCancelar.Content = "Cancelar";
            aux = new SaleOrder();
            EnableFields();
        }

        private void txtDiscount_LostFocus(object sender, RoutedEventArgs e) {
            try {
                if (txtDiscount.Text != string.Empty) {
                    double valor = double.Parse(txtTotalValue.Text) - double.Parse(txtDiscount.Text);
                    txtTotalValueDiscount.Text = valor.ToString();
                }
                else {
                    txtDiscount.Text = "0.00";
                    txtTotalValueDiscount.Text = txtTotalValue.Text;
                }
            }catch(Exception){
                MessageBox.Show("Erro de digitação!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e) {
            switch (actionIndex) {

                case 1:
                    AddSaleOrder();
                    break;
                case 2:
                    EditSaleOrder();
                    break;
                case 3:
                    DeleteSaleOrder();
                    break;
                default:
                    MessageBox.Show(
                            "Nenhuma Operação Selecionada!",
                            "Erro no preenchimento do formulário",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                    break;
            }
        }

        private void DiscountEdit_KeyDown(object sender, KeyEventArgs e) {
            bool handled = true; ;
            KeyConverter kv = new KeyConverter();
            if ((string.Compare((string)kv.ConvertTo(e.Key, typeof(string)), "OemComma") == 0)) {
                if (!txtDiscount.Text.Contains(",")) handled = false;
            } else if ((char.IsNumber((string)kv.ConvertTo(e.Key, typeof(string)), 0)))
                handled = false;
            e.Handled = handled;
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e) {
            actionIndex = 2;
            EnableFields();
            aux = new SaleOrder();
            SearchSaleOrder window = new SearchSaleOrder(this);
            window.Show();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            actionIndex = 3;
            btnSalvar.IsEnabled = true;
            aux = new SaleOrder();
            SearchSaleOrder window = new SearchSaleOrder(this);
            window.Show();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) {
            BlockFields();
            ClearFields();
            actionIndex = -1;
        }

        #endregion

        # region Data-Gathering

        private void AddSaleOrder() {
            if (Validate()) {

                try {

                    Estimate est = dgSaleOrder.SelectedItem as Estimate;
                    aux.No_estimate = est.DocNo;
                    aux.Date_realization = dpSaleOrder.Text;
                    aux.Situation = cbStuation.Text;
                    aux.Discount = double.Parse(txtDiscount.Text);
                    aux.TotalDiscount = double.Parse(txtTotalValueDiscount.Text);

                    dao.AddSaleOrder(aux);

                    Model.Log added = new Model.Log();
                    added.employe = _employe;
                    added.action = $"Pedido de venda para o cliente {txtCliente.Text} foi inserido!";
                    logs.Register(added);

                    RefreshGrid();
                    ClearFields();
                    BlockFields();
                    actionIndex = -1;
                    aux = null;

                }
                catch (Exception err) {
                    MessageBox.Show(
                        "Erro", 
                        err.Message, 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error
                    );
                }
                
            }
            else {
                MessageBox.Show("Campos vazios!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditSaleOrder() {
            try {

                if (EditValidate()) {
                    aux.No_saleOrder = int.Parse(txtSaleOrderNo.Text);
                    aux.No_estimate = SearchSaleOrder.id_estimate;
                    aux.Date_realization = dpSaleOrder.Text;
                    aux.Situation = cbStuation.Text;
                    aux.Discount = double.Parse(txtDiscount.Text);
                    aux.TotalDiscount = double.Parse(txtTotalValueDiscount.Text);

                    dao.EditSaleOrder(aux);

                    Model.Log edited = new Model.Log();
                    edited.employe = _employe;
                    edited.action = $"Pedido de venda para o cliente {txtCliente.Text} foi alterado!";
                    logs.Register(edited);

                    RefreshGrid();
                    ClearFields();
                    BlockFields();
                    actionIndex = -1;
                    aux = null;
                }
                else {
                    MessageBox.Show(
                            "Nenhuma venda foi selecionada! Selecione uma para prosseguir...",
                            "Validação",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                    return;
                }

            }
            catch (DatabaseEditException) {

                throw;
            }
            finally {
                SearchSaleOrder.id_estimate = 0;
            }
        }

        private void DeleteSaleOrder() {
            try {

                if (EditValidate()) {

                    aux.No_saleOrder = int.Parse(txtSaleOrderNo.Text);
                    dao.DeleteSaleOrder(aux);

                    Model.Log deleted = new Model.Log();
                    deleted.employe = _employe;
                    deleted.action = $"Venda numero: {aux.No_saleOrder} foi removido no sistema!";
                    logs.Register(deleted);

                    RefreshGrid();
                    ClearFields();
                    BlockFields();
                    actionIndex = -1;
                    aux = null;
                }
                else {
                    MessageBox.Show(
                            "Nenhuma venda foi selecionada! Selecione uma para prosseguir...",
                            "Validação",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                    return;
                }

            }
            catch (DatabaseDeleteException err) {
                MessageBox.Show(
                 err.Message,
                 "Erro",
                 MessageBoxButton.OK,
                 MessageBoxImage.Error
             );
            }
            catch (Exception err) {
                MessageBox.Show(
                    err.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        #endregion
    }
}
