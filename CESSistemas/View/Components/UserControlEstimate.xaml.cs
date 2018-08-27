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
        private int actionIndex = -1;
        private bool canDelete = false;
        private Estimates dao;
        private Estimate aux;
        private Employe _employe;
        private Logs logs;

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
            logs = new Logs();
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

            // Definindo configurações iniciais da tela
            SetDefaults();
            BlockFields();
            //RefreshGrid();
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
            bool success = boxDialog.ShowDialog() == true;

            // Verificando se alguma imagem foi escolhida
            if (success) {

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
        }

        /// <summary>
        /// Evento do botão adicionar fornecedor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e) {
            actionIndex = 1;
            canDelete = false;
            btnCancelar.Content = "Cancelar";
            aux = new Estimate();
            ClearFields();
            EnableFields();
        }

        /// <summary>
        /// Evento do botão alterar orçamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (dgEstimate.SelectedItems.Count > 0) {            
                actionIndex = 2;
                canDelete = true;
                btnCancelar.Content = "Excluir";
                EnableFields();
            } else {
                MessageBox.Show(
                        "Nenhum orçamento foi selecionado! Selecione um para prosseguir...",
                        "Validação",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
            }
        }

        /// <summary>
        /// Evento do botão cancelar/excluir orçamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (canDelete) {

            } else {
                BlockFields();
                ClearFields();
                btnCancelar.Content = "Cancerlar";
                actionIndex = -1;
            }
        }

        /// <summary>
        /// Evento do botão salvar orçamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e) {
            switch (actionIndex) {

                case 1:
                    AddEstimate();
                    break;
                case 2:
                    EditEstimate();
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

        /// <summary>
        /// Evento do botão gerar pdf de orçamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGeneratePdf(object sender, System.Windows.RoutedEventArgs e) {
            if (dgEstimate.SelectedItems.Count > 0) {

            }
        }

        /// <summary>
        /// Evento do botão adicionar serviço
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddService_Click(object sender, System.Windows.RoutedEventArgs e) {
            
        }

        /// <summary>
        /// Evento do botão remover serviço
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveService_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (dgServices.SelectedItems.Count > 0) {

                // Removendo item selecionado da lista de serviços e atualizando
                aux.services.Remove(dgServices.SelectedItem as Service);
                dgServices.ItemsSource = null;
                dgServices.ItemsSource = aux.services;

            } else {
                MessageBox.Show(
                    "Selecione a tarefa a ser removida de lista",
                    "Validação",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        /// <summary>
        /// Evento do botão atualizar grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e) {

        }

        #endregion

        #region Data-Gathering

        /// <summary>
        /// Método para coletar dados ao adicionar novo orçamento
        /// </summary>
        private void AddEstimate() {
            if (IsFilledFields()) {

                // Recuperando dados do orçamento
                aux.idCustomer = (int)cbCustomer.SelectedValue;
                aux.date = dpEstimate.Text;
                aux.description = txtDescription.Text;
                aux.payCondition = cbPagto.Text;
                aux.daysExecution = cbDaysExecution.Text;
                aux.totalValue = double.Parse(txtValue.Text);

                // Inserindo registro no banco
                dao.AddEstimate(aux);

                // Registro do log de inserção
                Model.Log added = new Model.Log();
                added.employe = _employe;
                added.action = $"Orçamento para o cliente {cbCustomer.Text} foi gerado!";
                logs.Register(added);

                // Atualizando grid e limpando campos
                //RefreshGrid();
                ClearFields();
                BlockFields();
                actionIndex = -1;
                aux = null;

            } else {
                MessageBox.Show(
                    "Há Campos Vazios",
                    "Erro de Prenchimento de Formulário",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        /// <summary>
        /// Método para coletar dados ao adicionar novo orçamento
        /// </summary>
        private void EditEstimate() {
            if (IsFilledFields()) {

                // Recuperando dados do orçamento
                aux.idCustomer = (int)cbCustomer.SelectedValue;
                aux.date = dpEstimate.Text;
                aux.description = txtDescription.Text;
                aux.payCondition = cbPagto.Text;
                aux.daysExecution = cbDaysExecution.Text;
                aux.totalValue = double.Parse(txtValue.Text);

                // Inserindo registro no banco
                dao.EditEstimate(aux);

                // Registro do log de inserção
                Model.Log edited = new Model.Log();
                edited.employe = _employe;
                edited.action = $"Orçamento para o cliente {cbCustomer.Text} foi alterado!";
                logs.Register(edited);

                // Atualizando grid e limpando campos
                //RefreshGrid();
                ClearFields();
                BlockFields();
                actionIndex = -1;
                aux = null;

            } else {
                MessageBox.Show(
                    "Há Campos Vazios",
                    "Erro de Prenchimento de Formulário",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        #endregion

        #region Utils

        /// <summary>
        /// Método para verificação de  preenchimento dos campos
        /// </summary>
        /// <returns></returns>
        private bool IsFilledFields() {
            return !(
                dpEstimate.Text.Equals(string.Empty) ||
                txtDescription.Text.Equals(string.Empty) ||
                cbPagto.SelectedIndex < 0 ||
                cbDaysExecution.SelectedIndex < 0 ||
                txtValue.Text.Equals(string.Empty)
            );
        }

        /// <summary>
        /// Método para configurações iniciais datela
        /// </summary>
        private void SetDefaults() {

            // Definindo valores iniciais dos componentes
            cbCustomer.SelectedIndex = 0;
            cbDaysExecution.SelectedIndex = 0;
            cbPagto.SelectedIndex = 0;
            dpEstimate.Text = DateBr.GetDateBr();
        }

        /// <summary>
        /// Método para limpar campos
        /// </summary>
        private void ClearFields() {

            // Booleana
            canDelete = false;

            // Combo Boxes
            cbCustomer.SelectedIndex = 0;
            cbPagto.SelectedIndex = 0;
            cbDaysExecution.SelectedIndex = 0;

            // Text Inputs
            dpEstimate.Text = string.Empty;
            txtDocNo.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtValue.Text = string.Empty;

            // Grids
            dgServices.ItemsSource = null;
            btnCancelar.Content = "Cancelar";
        }

        /// <summary>
        /// Método para bloquear campos
        /// </summary>
        private void BlockFields() {

            // Combo Boxes
            cbCustomer.IsEnabled = false;
            cbPagto.IsEnabled = false;
            cbDaysExecution.IsEnabled = false;

            // Text Inputs
            dpEstimate.IsEnabled = false;
            txtDocNo.IsEnabled = false;
            txtDescription.IsEnabled = false;
            txtValue.IsEnabled = false;

            // Buttons
            btnPlus.IsEnabled = false;
            btnMinus.IsEnabled = false;
            btnCancelar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
            btnLogo.IsEnabled = false;

            // Grids
            dgServices.IsEnabled = false;
        }

        /// <summary>
        /// Método para desbloquear campos
        /// </summary>
        private void EnableFields() {

            // Combo Boxes
            cbCustomer.IsEnabled = true;
            cbPagto.IsEnabled = true;
            cbDaysExecution.IsEnabled = true;

            // Text Inputs
            dpEstimate.IsEnabled = true;
            txtDescription.IsEnabled = true;
            txtValue.IsEnabled = true;

            // Buttons
            btnPlus.IsEnabled = true;
            btnMinus.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            btnSalvar.IsEnabled = true;
            btnLogo.IsEnabled = true;

            // Grids
            dgServices.IsEnabled = true;
            dgServices.ItemsSource = aux.services;
        }

        #endregion
    }
}
