using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using Promig.Connection.Methods;
using Promig.Exceptions;
using Promig.Model;
using Promig.Model.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Promig.Utils;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using Promig.Connection;
using System.IO;

namespace Promig.View.Components {

    /// <summary>
    /// Interação lógica para UserControlEstimate.xam
    /// </summary>
    public partial class UserControlEstimate : UserControl {

        #region Header

        private Estimates dao;
        private Logs logs;
        private Estimate aux;
        private int actionIndex;
        string path = string.Empty;
        string imgPdf = string.Empty;
        private Employe _employe;

        #endregion

        #region Constructor
        /// <summary>
        /// Construtor padrão
        /// </summary>
        public UserControlEstimate() {
            InitializeComponent();

            //FillComboBoxCustomer();
            //BlockFields();
            dao = new Estimates();
            logs = new Logs();
            aux = new Estimate();
            _employe = new Employe();
            _employe.id = MainWindow.currentId;
            txtDocNo.IsEnabled = false;
            //RefreshDataGrid();
        }
        #endregion

        /*#region Block/Enable Fields
        private void EnableFields() {

            //Comboboxes
            cbCustomer.IsEnabled = true;
            cbDaysExecution.IsEnabled = true;
            cbPagto.IsEnabled = true;

            //DataPicker
            dpEstimate.IsEnabled = true;

            //Campos de texto
            txtDescription.IsEnabled = true;
            txtValue.IsEnabled = true;
            txtDescriptionService1.IsEnabled = true;
            txtDescriptionService2.IsEnabled = true;
            txtDescriptionService3.IsEnabled = true;
            txtDescriptionService4.IsEnabled = true;
            txtDescriptionService5.IsEnabled = true;
            txtDescriptionService6.IsEnabled = true;
            txtDescriptionService7.IsEnabled = true;

            //Botões
            btnSalvar.IsEnabled = true;
            button.IsEnabled = true;
        }

        /// <summary>
        /// Método para desabilitar campos
        /// </summary>
        private void BlockFields() {

            //Comboboxes
            cbCustomer.IsEnabled = false;
            cbDaysExecution.IsEnabled = false;
            cbPagto.IsEnabled = false;

            //DataPicker
            dpEstimate.IsEnabled = false;

            //Campos de texto
            txtDescription.IsEnabled = false;
            txtValue.IsEnabled = false;
            txtDescriptionService1.IsEnabled = false;
            txtDescriptionService2.IsEnabled = false;
            txtDescriptionService3.IsEnabled = false;
            txtDescriptionService4.IsEnabled = false;
            txtDescriptionService5.IsEnabled = false;
            txtDescriptionService6.IsEnabled = false;
            txtDescriptionService7.IsEnabled = false;

            //Botões
            btnSalvar.IsEnabled = false;
            button.IsEnabled = false;
        }
        #endregion

        // limpar campos
        private void ClearFields() {

            //Campos de texto
            txtDocNo.Text = String.Empty;
            txtDescription.Text = String.Empty;
            txtValue.Text = String.Empty;
            txtDescriptionService1.Text = String.Empty;
            txtDescriptionService2.Text = String.Empty;
            txtDescriptionService3.Text = String.Empty;
            txtDescriptionService4.Text = String.Empty;
            txtDescriptionService5.Text = String.Empty;
            txtDescriptionService6.Text = String.Empty;
            txtDescriptionService7.Text = String.Empty;

            //DatePicker
            dpEstimate.Text = string.Empty;
            image.Source = null;

            //Comboboxes
            cbCustomer.Text = string.Empty;
            cbDaysExecution.Text = string.Empty;
            cbPagto.Text = string.Empty;
        }

        // carregar comboBox clientes
        private void FillComboBoxCustomer() {

            cbCustomer.ItemsSource = null;
            cbCustomer.ItemsSource = Estimates.NameCustomerList();
        }

        // carregar logo
        private void button_Click(object sender, System.Windows.RoutedEventArgs e) {

            OpenFileDialog boxDialog = new OpenFileDialog();
            boxDialog.Title = "Escolha o logo";
            boxDialog.Filter = "Imagens suportadas|*.jpg;*.jpeg;*.png|" +
                               "JPEG(*.jpeg;*.jpg)|*.jpg;*.jpeg|" +
                               "Portable Network Graphic (*.png)|*.png";
            if (boxDialog.ShowDialog() == true)
            {
                image.Source = new BitmapImage(new System.Uri(boxDialog.FileName));
            }
            path = boxDialog.FileName;

        }

        #region Buttons
        // botao adicionar
        private void btnAdicionar_Click(object sender, RoutedEventArgs e) {
            actionIndex = 1;
            ClearFields();
            EnableFields();
            btnCancelar.IsEnabled = true;
        }
        // botao cancelar
        private void btnCancelar_Click(object sender, RoutedEventArgs e) {

            if (btnCancelar.Content.Equals("Cancelar"))
            {
                actionIndex = -1;
                ClearFields();
                BlockFields();
                dgEstimate.SelectedItem = null;
            }
            if (btnCancelar.Content.Equals("Excluir"))
            {
                MessageBoxResult result = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir",
                                                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    dao.DeleteEstimate(int.Parse(txtDocNo.Text));
                    ClearFields();
                    dgEstimate.SelectedItem = null;
                    BlockFields();
                    btnCancelar.Content = "Cancelar";
                    btnCancelar.IsEnabled = false;
                    imgPdf = string.Empty;
                    RefreshDataGrid();
                }
                else
                {
                    ClearFields();
                    return;
                }
            }
        }
        // botao salvar
        private void btnSalvar_Click(object sender, RoutedEventArgs e) {

            switch (actionIndex)
            {
                case 1:
                    AddEstimate();
                    break;
                case 2:
                    EditEstimate();
                    break;
                default:
                    MessageBox.Show(
                            "Nenhuma Operação Selecionada!", "Erro no preenchimento do formulário",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    break;
            }
        }

        // botao editar
        private void btnEditar_Click(object sender, RoutedEventArgs e) {

            if (dgEstimate.SelectedItems.Count > 0)
            {
                actionIndex = 2;
                EnableFields();
                btnCancelar.Content = "Cancelar";
            }
            else
            {
                MessageBox.Show("Nenhum orçamento foi selecionado! Selecione um para prosseguir...", "Validação",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
            }

        }
        #endregion

        #region Data Manager
        private void AddEstimate() {
            if (Validate())
            {
                ClearFields();
                return;
            }
            try
            {
                Estimate estimate = new Estimate();
                estimate.Customer = cbCustomer.Text;
                estimate.IdCustomer = GetIdPerson();
                estimate.ImgPath = path.Replace(@"\", @"\\");
                estimate.Date = DateTime.Parse(DateTime.Parse(dpEstimate.SelectedDate.ToString()).ToShortDateString());
                estimate.Description = txtDescription.Text;
                estimate.PayCondition = cbPagto.Text;
                estimate.DaysExecution = cbDaysExecution.Text;
                estimate.TotalValue = Convert.ToDouble(txtValue.Text.Replace(",", "."));
                estimate.Description1 = txtDescriptionService1.Text;
                estimate.Description2 = txtDescriptionService2.Text;
                estimate.Description3 = txtDescriptionService3.Text;
                estimate.Description4 = txtDescriptionService4.Text;
                estimate.Description5 = txtDescriptionService5.Text;
                estimate.Description6 = txtDescriptionService6.Text;
                estimate.Description7 = txtDescriptionService7.Text;

                dao.AddEstimate(estimate);

                txtDocNo.Text = getDocNo(estimate.Customer).ToString();
                path = string.Empty;

                Model.Log added = new Model.Log();
                added.employe = _employe;
                added.action = string.Format("Orçamento nº: {0} foi cadastrado no sistema!", estimate.DocNo);
                logs.Register(added);

                FillDataGrid();
                ClearFields();
                BlockFields();
                actionIndex = -1;
                aux = null;

            }
            catch (DatabaseInsertException ex)
            {

                MessageBox.Show(
                       ex.Message, "Erro ao gravar dados",
                       MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void EditEstimate() {
            try
            {
                aux.Customer = cbCustomer.Text;
                aux.Date = DateTime.Parse(DateTime.Parse(dpEstimate.SelectedDate.ToString()).ToShortDateString());
                aux.Description = txtDescription.Text;
                aux.PayCondition = cbPagto.Text;
                aux.DaysExecution = cbDaysExecution.Text;
                aux.TotalValue = Convert.ToDouble(txtValue.Text.Replace(",", "."));
                aux.Description1 = txtDescriptionService1.Text;
                aux.Description2 = txtDescriptionService2.Text;
                aux.Description3 = txtDescriptionService3.Text;
                aux.Description4 = txtDescriptionService4.Text;
                aux.Description5 = txtDescriptionService5.Text;
                aux.Description6 = txtDescriptionService6.Text;
                aux.Description7 = txtDescriptionService7.Text;
                aux.ImgPath = path.Replace(@"\", @"\\");

                //Alterando registro no banco
                dao.EditEstimate(aux);

                path = string.Empty;

                //Registrando log de alteração
                Model.Log edited = new Model.Log();
                edited.employe = _employe;
                edited.action = "Orçamento nº " + aux.DocNo + " atualizado!";
                logs.Register(edited);

                //Atualizando grid e limpando campos de texto
                RefreshDataGrid();
                ClearFields();
                BlockFields();
                actionIndex = -1;
                aux = null;

            }
            catch (DatabaseEditException err)
            {

                //Retornando mensagem de erro para usuário
                MessageBox.Show(
                    err.Message,
                    "Erro ao gravar dados",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        #endregion

        // validar campos
        private bool Validate() {

            int cont = 0;
            if (cbCustomer.Text.Equals(String.Empty))
            {
                cont = 1;
            }
            if (txtValue.Text.Equals(String.Empty))
            {
                cont = 2;
            }
            if (cont == 1)
            {
                string customer = "Cliente";
                MessageBox.Show("Informe um " + customer + ".", "Alerta",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            if (cont == 2)
            {
                string value = "Valor total dos serviços";
                MessageBox.Show("Informe o " + value + ".", "Alerta",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            if (dpEstimate.SelectedDate == null)
            {
                MessageBox.Show("Data do orçamento não pode ser vazia!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            if (txtDescription.Text != String.Empty || txtDescriptionService1.Text != String.Empty ||
               txtDescriptionService2.Text != String.Empty || txtDescriptionService3.Text != String.Empty ||
               txtDescriptionService4.Text != String.Empty || txtDescriptionService5.Text != String.Empty ||
               txtDescriptionService6.Text != String.Empty || txtDescriptionService7.Text != String.Empty)
            {

                txtDescription.Text = txtDescription.Text.Replace("\"", "");
                txtDescriptionService1.Text = txtDescriptionService1.Text.Replace("\"", "");
               

                if (txtValue.Text != String.Empty)
                {
                    txtValue.Text = txtValue.Text.Insert(txtValue.Text.Length - 2, ",");
                }

                return false;
            }
            return false;
        }

        #region Util
        private int GetIdPerson() {
            try
            {
                int id = 0;
                MySqlConnection connection = ConnectionFactory.GetConnection();
                connection.Open();
                string sql = string.Format("select p.id_pessoa from clientes c, pessoas p " +
                                           "where p.nome_pessoa ='" + cbCustomer.Text + "'");
                MySqlCommand cmd = new MySqlCommand(sql, connection)
                {
                    CommandType = CommandType.Text
                };

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = int.Parse(reader["id_pessoa"].ToString());
                }
                connection.Close();
                return id;
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        // obter o docno
        private int getDocNo(string customer) {

            try
            {
                int docno = 0;
                MySqlConnection connection = ConnectionFactory.GetConnection();
                connection.Open();

                string sql = string.Format("select no_documento from orcamentos where cliente= '" + customer + "'");
                MySqlCommand cmd = new MySqlCommand(sql, connection);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    docno = int.Parse(reader["no_documento"].ToString());
                }
                connection.Close();
                return docno;
            }
            catch (MySqlException)
            {
                throw;
            }

        }

        // carregar dataGrid
        private void FillDataGrid() {

            dgEstimate.ItemsSource = null;
            dgEstimate.ItemsSource = dao.GetAllEstimate(int.Parse(txtDocNo.Text));

        }
        // atualizar datagrid
        private void RefreshDataGrid() {

            dgEstimate.ItemsSource = null;
            dgEstimate.ItemsSource = dao.GetPartialEstimateList();
        }
        #endregion

        // selecionando orcamento na grid e preencher as informações em tela
        private void dgEstimate_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            if (dgEstimate.SelectedItems.Count > 0)
            {

                ClearFields();

                Estimate source = dgEstimate.SelectedItem as Estimate;
                if (source != null)
                {
                    btnCancelar.IsEnabled = true;
                    btnPdf.IsEnabled = true;
                    btnCancelar.Content = "Excluir";
                    aux = dao.GetEstimateData(source.DocNo);
                }
                else
                {
                    return;
                }

                cbCustomer.Text = aux.Customer;
                dpEstimate.Text = aux.Date.ToShortDateString();
                txtDescription.Text = aux.Description;
                txtDocNo.Text = aux.DocNo.ToString();
                cbDaysExecution.Text = aux.DaysExecution;
                cbPagto.Text = aux.PayCondition;
                txtValue.Text = aux.TotalValue.ToString();
                txtDescriptionService1.Text = aux.Description1;
                image.Source = (!string.IsNullOrEmpty(aux.ImgPath)) ? new BitmapImage(new System.Uri(aux.ImgPath)) : null;
                imgPdf = aux.ImgPath;

                if (Validate())
                {
                    ClearFields();
                    imgPdf = string.Empty;
                    return;
                }

                actionIndex = -1;
                BlockFields();

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {

            btnCancelar.IsEnabled = false;
            btnPdf.IsEnabled = false;
        }

        #region PDF
        private void ExportPdf() {

            // Recuperando dados da empresa
            CompanyModel model = CompanyData.GetPreferencesData();

            // Criando documento pdf
            Document doc = new Document(PageSize.A4);
            doc.SetMargins(40, 40, 40, 80);
            doc.AddCreationDate();

            SaveFileDialog save = new SaveFileDialog();
            string caminho = string.Empty;
            if (save.ShowDialog() == true)
            {
                caminho = save.FileName + ".pdf";
            }
            else
            {
                return;
            }

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
            var img = iTextSharp.text.Image.GetInstance(imgPdf);
            doc.Open();
            doc.Add(img);
            Paragraph p1 = new Paragraph("PROPOSTA COMERCIAL", new Font(Font.NORMAL, 14, (int)System.Drawing.FontStyle.Bold));
            doc.Add(p1);
            Paragraph p2 = new Paragraph("Cliente: " + cbCustomer.Text, new Font(Font.NORMAL, 12));
            doc.Add(p2);
            Paragraph p3 = new Paragraph("Data: " + dpEstimate.Text, new Font(Font.NORMAL, 12));
            doc.Add(p3);
            Paragraph p4 = new Paragraph("\n");
            doc.Add(p4);
            Paragraph p5 = new Paragraph(txtDescription.Text, new Font(Font.NORMAL, 12));
            doc.Add(p5);
            Paragraph p6 = new Paragraph("\n");
            doc.Add(p6);
            Paragraph p7 = new Paragraph("Inclusos encargos com mão de obra, material e equipamentos para execução dos serviços," +
            "encargos trabalhistas e impostos municipais, estaduais e federais." +
            "Obs: Itens não relacionados nesse documento orçamentário e escopo dos serviços e materiais" +
            "serão faturados como aditivos.", new Font(Font.NORMAL, 12));
            doc.Add(p7);
            Paragraph p8 = new Paragraph("\n");
            doc.Add(p8);
            Paragraph p9 = new Paragraph("Número do Documento: " + txtDocNo.Text, new Font(Font.NORMAL, 12));
            doc.Add(p9);
            Paragraph p10 = new Paragraph("\n");
            doc.Add(p10);
            Paragraph p11 = new Paragraph("Condição de Pagamento: " + cbPagto.Text, new Font(Font.NORMAL, 12));
            doc.Add(p11);
            Paragraph p12 = new Paragraph("Execução em até: " + cbDaysExecution.Text, new Font(Font.NORMAL, 12));
            doc.Add(p12);
            Paragraph p13 = new Paragraph("Valor Total dos serviços R$: " + txtValue.Text, new Font(Font.NORMAL, 12));
            doc.Add(p13);
            Paragraph p14 = new Paragraph("\n");
            doc.Add(p14);
            Paragraph p15 = new Paragraph("DESCRIÇÃO DOS SERVIÇOS", new Font(Font.NORMAL, 14, (int)System.Drawing.FontStyle.Bold));
            doc.Add(p15);
            Paragraph p16 = new Paragraph("\n");
            doc.Add(p16);

            List<string> lista = new List<string>();
            if (txtDescriptionService1.Text != string.Empty)
            {
                lista.Add(txtDescriptionService1.Text);
            }
            
            int cont = 1;
            foreach (String item in lista)
            {
                Paragraph p17 = new Paragraph("Descrição" + cont + ": " + item, new Font(Font.NORMAL, 10));
                doc.Add(p17);
                cont++;
            }
            Paragraph p18 = new Paragraph("\n");
            doc.Add(p18);
            Paragraph p19 = new Paragraph("\n");
            doc.Add(p19);
            Paragraph p20 = new Paragraph("\n");
            doc.Add(p20);
            Paragraph p21 = new Paragraph("Atenciosamente: " + model.name, new Font(Font.NORMAL, 9, (int)System.Drawing.FontStyle.Bold));
            doc.Add(p21);
            Paragraph p22 = new Paragraph(CompanyData.GetPdfFooterData(), new Font(Font.NORMAL, 8));
            doc.Add(p22);

            doc.Close();
            System.Diagnostics.Process.Start(caminho);
            imgPdf = string.Empty;
        }
        // botao PDF
        private void btnPdf_Click(object sender, RoutedEventArgs e) {

            ExportPdf();
            btnPdf.IsEnabled = false;
        }
        #endregion
        */
    }
}
