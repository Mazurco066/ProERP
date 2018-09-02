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
using System.Windows.Input;

namespace Promig.View.Components {

    public partial class UserControlEstimate : UserControl {

        #region Header

        private string imgDirectoryPath;
        private string estimateDirectoryPath;
        private string logoPath;
        private int actionIndex = -1;
        private Estimates dao;
        private Services services;
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
            estimateDirectoryPath = "C:\\ProERP\\Generated-Documents\\Orçamentos\\";
            logoPath = string.Empty;

            // Inicializando objetos
            aux = null;
            dao = new Estimates();
            services = new Services();
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

            try {

                // Alimentando items do combo box de clientes
                cbCustomer.ItemsSource = dao.NameCustomerList();
                cbCustomer.DisplayMemberPath = "name";
                cbCustomer.SelectedValuePath = "id";

                // Alimentando items do combo box de serviços
                cbServices.ItemsSource = services.ComboBoxSource();
                cbServices.DisplayMemberPath = "description";
                cbServices.SelectedValuePath = "id";

            } catch (DatabaseAccessException err) {
                MessageBox.Show(
                    err.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }

            // Definindo configurações iniciais da tela
            SetDefaults();
            BlockFields();
            RefreshGrid();
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
                logoPath = destPath;
            }
        }

        /// <summary>
        /// Evento do botão adicionar fornecedor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e) {
            actionIndex = 1;
            btnCancelar.Content = "Cancelar";
            aux = new Estimate();
            ClearFields();
            EnableFields();
            btnDelete.IsEnabled = false;
            btnPdf.IsEnabled = false;
        }

        /// <summary>
        /// Evento do botão alterar orçamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (dgEstimate.SelectedItems.Count > 0) {            
                actionIndex = 2;
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
                BlockFields();
                ClearFields();
                actionIndex = -1;  
        }

        /// <summary>
        /// Evento do botão deletar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e) {
            actionIndex = 3;
            ClearFields();
            MessageBox.Show(
                "Salve as alterações para confirmar a exclusão!",
                "Alerta",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
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
                case 3:
                    DeleteEstimate();
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
                ExportPdf();
                btnPdf.IsEnabled = false;
            }
        }

        /// <summary>
        /// Evento do botão adicionar serviço
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddService_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (cbServices.SelectedIndex != -1 && !txtAmount.Text.Equals(string.Empty)) {

                // Instanciando item
                ItemEstimate item = new ItemEstimate();
                item.Service = services.GetServiceData((int)cbServices.SelectedValue);
                item.Amount = int.Parse(txtAmount.Text);

                // Adicionando item
                aux.Items.Add(item);
                dgServices.ItemsSource = null;
                dgServices.ItemsSource = aux.Items;
                txtValue.Text = aux.TotalValue.ToString();                        

                // Limpando campos
                cbServices.SelectedIndex = 0;
                txtAmount.Text = string.Empty;
            }
        }

        /// <summary>
        /// Evento do botão remover serviço
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveService_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (dgServices.SelectedItems.Count > 0) {

                // Removendo item selecionado da lista de serviços e atualizando
                aux.Items.Remove(dgServices.SelectedItem as ItemEstimate);
                dgServices.ItemsSource = null;
                dgServices.ItemsSource = aux.Items;
                txtValue.Text = aux.TotalValue.ToString();

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
            RefreshGrid();
        }

        /// <summary>
        /// Evento ao digitar no campo de  pesquisa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_KeyDown(object sender, RoutedEventArgs e) {
            RefreshGrid(txtSearch.Text);
        }

        /// <summary>
        /// Evento ao selecionar algum registro na grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dgEstimate_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            if (dgEstimate.SelectedItems.Count > 0) {
                try {

                    //Recuperando dados do cliente selecionado
                    Estimate source = dgEstimate.SelectedItem as Estimate;
                    aux = dao.GetEstimateData(source.DocNo);
                    logoPath = aux.ImgPath;

                    //Preenchendo campos (Textboxes)
                    txtDescription.Text = aux.Description;
                    txtDocNo.Text = aux.DocNo.ToString();
                    dpEstimate.Text = aux.Date;
                    txtValue.Text = aux.TotalValue.ToString();

                    // Preenchendo campos (Comboboxes)
                    int index = -1;
                    foreach (Customer item in cbCustomer.Items) {
                        index++;
                        if (item.name.Equals(aux.NameCustomer)) break;
                    }
                    cbCustomer.SelectedIndex = index;

                    index = -1;
                    foreach (ComboBoxItem item in cbDaysExecution.Items) {
                        index++;
                        if (item.Content.Equals(aux.DaysExecution)) break;
                    }
                    cbDaysExecution.SelectedIndex = index;

                    index = -1;
                    foreach (ComboBoxItem item in cbPagto.Items) {
                        index++;
                        if (item.Content.Equals(aux.PayCondition)) break;
                    }
                    cbPagto.SelectedIndex = index;

                    // Preenchendo imagem
                    image.Source = new BitmapImage(new Uri(aux.ImgPath));

                    // Preenchendo lista de itens
                    dgServices.ItemsSource = aux.Items;

                    //Definindo ação como nula
                    actionIndex = -1;
                    
                } catch (DatabaseAccessException err) {
                    MessageBox.Show(
                        err.Message,
                        "Erro",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }

                //Desabiilitando campos e preenchendo com dados
                BlockFields();
            }
        }

        /// <summary>
        /// Evento ao digitar em um campo numerico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAmount_KeyDown(object sender, KeyEventArgs e) {
            KeyConverter kv = new KeyConverter();
            if ((char.IsNumber((string)kv.ConvertTo(e.Key, typeof(string)), 0) == false)) {
                e.Handled = true;
            }
        }

        #endregion

        #region Data-Gathering

        /// <summary>
        /// Método para coletar dados ao adicionar novo orçamento
        /// </summary>
        private void AddEstimate() {
            if (IsFilledFields()) {

                // Recuperando dados do orçamento
                aux.IdCustomer = (int)cbCustomer.SelectedValue;
                aux.Date = dpEstimate.Text;
                aux.Description = txtDescription.Text;
                aux.PayCondition = cbPagto.Text;
                aux.DaysExecution = cbDaysExecution.Text;
                aux.ImgPath = logoPath;

                // Inserindo registro no banco
                dao.AddEstimate(aux);

                // Registro do log de inserção
                Model.Log added = new Model.Log();
                added.employe = _employe;
                added.action = $"Orçamento para o cliente {cbCustomer.Text} foi gerado!";
                logs.Register(added);

                // Atualizando grid e limpando campos
                RefreshGrid();
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
        /// Método para coletar dados ao editar novo orçamento
        /// </summary>
        private void EditEstimate() {
            if (IsFilledFields()) {

                // Recuperando dados do orçamento
                aux.IdCustomer = (int)cbCustomer.SelectedValue;
                aux.Date = dpEstimate.Text;
                aux.Description = txtDescription.Text;
                aux.PayCondition = cbPagto.Text;
                aux.DaysExecution = cbDaysExecution.Text;
                aux.ImgPath = logoPath;

                // Inserindo registro no banco
                dao.EditEstimate(aux);

                // Registro do log de inserção
                Model.Log edited = new Model.Log();
                edited.employe = _employe;
                edited.action = $"Orçamento para o cliente {cbCustomer.Text} foi alterado!";
                logs.Register(edited);

                // Atualizando grid e limpando campos
                RefreshGrid();
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
        /// Método para coletar dados e deletar orçamento
        /// </summary>
        private void DeleteEstimate() {
            try {

                // Remoção do serviço
                dao.DeleteEstimate(aux.DocNo);

                // Registro de log - Edição
                Model.Log deleted = new Model.Log();
                deleted.employe = _employe;
                deleted.action = $"Orçamento: {aux.Description} foi removido no sistema!";
                logs.Register(deleted);

                // Atualizando grid e limpando campos de texto
                RefreshGrid();
                ClearFields();
                BlockFields();
                actionIndex = -1;
                aux = null;

            } catch (DatabaseDeleteException err) {
                MessageBox.Show(
                    err.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            } catch (Exception err) {
                MessageBox.Show(
                    err.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        #endregion

        #region Grid-Param

        /// <summary>
        /// Método para limpar campo de busca e atualizar grid
        /// </summary>
        private void RefreshGrid() {

            // Limpando campo de busca
            txtSearch.Text = string.Empty;

            try {

                // Recuperando todos orçamentos em modo de exibição
                dgEstimate.ItemsSource = dao.GetAllEstimates(txtSearch.Text);

            } catch (DatabaseAccessException err) {
                MessageBox.Show(
                    err.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        /// <summary>
        /// Método para atualizar grid com base em consulta
        /// </summary>
        /// <param name="param"></param>
        private void RefreshGrid(string param) {
            try { dgEstimate.ItemsSource = dao.GetAllEstimates(param); } 
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

        #region PDF

        /// <summary>
        /// Método para exportar para pdf
        /// </summary>
        private void ExportPdf() {

            // Definição das margens do documento
            Document doc = new Document(PageSize.A4);
            doc.SetMargins(40, 40, 40, 80);
            doc.AddCreationDate();

            // Criação do diiretório se não existir
            if (!Directory.Exists(estimateDirectoryPath)) Directory.CreateDirectory(estimateDirectoryPath);
            string oldPath = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(estimateDirectoryPath);

            //Configurando arquivo a ser salvo
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"Orçamento-{DateTime.Now.ToString("ddMMyyyyhhmmss")}.pdf");

            // Definição de escrita do documento
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            writer.CompressionLevel = PdfStream.NO_COMPRESSION;

            // Adicionando paragráfos
            var img = iTextSharp.text.Image.GetInstance(aux.ImgPath);
            doc.Open();
            doc.Add(img);
            Paragraph p1 = new Paragraph("PROPOSTA COMERCIAL", new Font(Font.NORMAL, 14, (int)System.Drawing.FontStyle.Bold));
            doc.Add(p1);
            Paragraph p2 = new Paragraph($"Cliente: {aux.NameCustomer}", new Font(Font.NORMAL, 12));
            doc.Add(p2);
            Paragraph p3 = new Paragraph($"Data: {aux.Date}", new Font(Font.NORMAL, 12));
            doc.Add(p3);
            Paragraph p4 = new Paragraph("\n");
            doc.Add(p4);
            Paragraph p5 = new Paragraph(aux.Description, new Font(Font.NORMAL, 12));
            doc.Add(p5);
            Paragraph p6 = new Paragraph("\n");
            doc.Add(p6);
            Paragraph p7 = new Paragraph("Inclusos encargos com mão de obra, material e equipamentos para execução dos serviços," +
            "encargos trabalhistas e impostos municipais, estaduais e federais." +
            "Obs: Itens não relacionados nesse documento orçamentário e escopo dos serviços e materiais " +
            "serão faturados como aditivos.", new Font(Font.NORMAL, 12));
            doc.Add(p7);
            Paragraph p8 = new Paragraph("\n");
            doc.Add(p8);
            Paragraph p9 = new Paragraph($"Número do Documento: {aux.DocNo}", new Font(Font.NORMAL, 12));
            doc.Add(p9);
            Paragraph p10 = new Paragraph("\n");
            doc.Add(p10);
            Paragraph p11 = new Paragraph($"Condição de Pagamento: {aux.PayCondition}", new Font(Font.NORMAL, 12));
            doc.Add(p11);
            Paragraph p12 = new Paragraph($"Execução em até: {aux.DaysExecution}", new Font(Font.NORMAL, 12));
            doc.Add(p12);
            Paragraph p13 = new Paragraph($"Valor Total dos serviços R${aux.TotalValue}", new Font(Font.NORMAL, 12));
            doc.Add(p13);
            Paragraph p14 = new Paragraph("\n");
            doc.Add(p14);
            Paragraph p15 = new Paragraph("DESCRIÇÃO DOS SERVIÇOS", new Font(Font.NORMAL, 14, (int)System.Drawing.FontStyle.Bold));
            doc.Add(p15);
            Paragraph p16 = new Paragraph("\n");
            doc.Add(p16);
            int cont = 1;
            foreach (ItemEstimate item in aux.Items) {
                Paragraph p17 = new Paragraph($"Descrição {cont}: {item.Service.Task} - SUBTOTAL: R${item.SubTotal}", new Font(Font.NORMAL, 10));
                doc.Add(p17);
                cont++;
            }
            Paragraph p18 = new Paragraph("\n");
            doc.Add(p18);
            Paragraph p19 = new Paragraph("\n");
            doc.Add(p19);
            Paragraph p20 = new Paragraph("\n");
            doc.Add(p20);
            Model.Json.CompanyModel data = CompanyData.GetPreferencesData();
            Paragraph p21 = new Paragraph($"Atenciosamente: {data.name}", new Font(Font.NORMAL, 9, (int)System.Drawing.FontStyle.Bold));
            doc.Add(p21);
            Paragraph p22 = new Paragraph(CompanyData.GetPdfFooterData(), new Font(Font.NORMAL, 8));
            doc.Add(p22);

            // Processando documento
            doc.Close();
            System.Diagnostics.Process.Start(path);
            btnPdf.IsEnabled = true;

            // Mensagem de sucesso
            MessageBox.Show(
                "PDF fo orçamento gerado",
                "Sucesso",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );

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
                txtValue.Text.Equals(string.Empty) ||
                aux.Items.Count <= 0 ||
                logoPath.Equals(string.Empty)
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
            cbServices.SelectedItem = 0;
            dpEstimate.Text = DateBr.GetDateBr();
        }

        /// <summary>
        /// Método para limpar campos
        /// </summary>
        private void ClearFields() {

            // Combo Boxes
            cbCustomer.SelectedIndex = 0;
            cbPagto.SelectedIndex = 0;
            cbDaysExecution.SelectedIndex = 0;
            cbServices.SelectedItem = 0;

            // Text Inputs
            dpEstimate.Text = string.Empty;
            txtDocNo.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtAmount.Text = string.Empty;

            // Grids
            dgServices.ItemsSource = null;
            image.Source = null;
            logoPath = string.Empty;
        }

        /// <summary>
        /// Método para bloquear campos
        /// </summary>
        private void BlockFields() {

            // Combo Boxes
            cbCustomer.IsEnabled = false;
            cbPagto.IsEnabled = false;
            cbDaysExecution.IsEnabled = false;
            cbServices.IsEnabled = false;

            // Text Inputs
            dpEstimate.IsEnabled = false;
            txtDocNo.IsEnabled = false;
            txtDescription.IsEnabled = false;
            txtValue.IsEnabled = false;
            txtAmount.IsEnabled = false;

            // Buttons
            btnPlus.IsEnabled = false;
            btnMinus.IsEnabled = false;
            btnCancelar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
            btnLogo.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnPdf.IsEnabled = false;

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
            cbServices.IsEnabled = true;

            // Text Inputs
            dpEstimate.IsEnabled = true;
            txtDescription.IsEnabled = true;
            txtAmount.IsEnabled = true;

            // Buttons
            btnPlus.IsEnabled = true;
            btnMinus.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            btnSalvar.IsEnabled = true;
            btnLogo.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnPdf.IsEnabled = true;

            // Grids
            dgServices.IsEnabled = true;
        }

        #endregion
    }
}
