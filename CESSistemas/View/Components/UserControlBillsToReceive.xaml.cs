using Promig.Connection.Methods;
using Promig.Exceptions;
using Promig.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Promig.View.AuxComponents;

namespace Promig.View.Components {

    public partial class UserControlBillsToReceive : UserControl {



        #region Header

        private Credits dao;
        private Logs logs;
        private Credit aux;
        private Employe _employe;
        private int actionIndex;
        public static string name_Customer;

        #endregion

        #region constructors

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public UserControlBillsToReceive() {
            InitializeComponent();

            dao = new Credits();
            logs = new Logs();
            aux = null;
            _employe = new Employe();
            _employe.id = MainWindow.currentId;
            actionIndex = -1;
            BlockFields();
            RefreshGrid();
        }

        #endregion constructors

        #region Data-Gathering
        private void AddCredit() {
            if (Validate()) {

                try {

                    // Recuperando dados
                    Credit credit = new Credit();
                    credit.TotalAmount = double.Parse(valorTotal_txt.Text);
                    credit.StartAmount = double.Parse(valorIncial_txt.Text);
                    credit.ReceiptDate = dataPagamento_txt.Text;
                    credit.DueDate = dataVencimento_txt.Text;
                    credit.Description = descricao_txt.Text;
                    int id_pessoa = dao.GetIdPessoaCustomer(cliente_txt.Text);
                    credit.IdCustomer = dao.GetIdCustomer(id_pessoa);

                    if (credit.IdCustomer == 0) {
                        MessageBox.Show("Cliente invalido!!!", "Aviso",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Inserindo serviço no banco
                    dao.AddCredit(credit);

                    // Registro de log - Inserção
                    Model.Log added = new Model.Log();
                    added.employe = _employe;
                    added.action = $"Conta a receber: {credit.Description} foi cadastrado no sistema!";
                    logs.Register(added);

                    // Atualizando grid e limpando campos de texto
                    RefreshGrid();
                    ClearFields();
                    BlockFields();
                    actionIndex = -1;
                    aux = null;

                }
                catch (DatabaseInsertException err) {
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
            else {
                MessageBox.Show(
                    "Há Campos Vazios",
                    "Erro de Prenchimento de Formulário",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        /// <summary>
        /// Metodo para coletar dados e Editar Serviço
        /// </summary>
        private void EditCredit() {
            if (Validate()) {

                try {

                    // Recuperando dados
                    aux.TotalAmount = double.Parse(valorTotal_txt.Text);
                    aux.StartAmount = double.Parse(valorIncial_txt.Text);
                    aux.ReceiptDate = dataPagamento_txt.Text;
                    aux.DueDate = dataVencimento_txt.Text;
                    aux.Description = descricao_txt.Text;
                    int id_pessoa = dao.GetIdPessoaCustomer(cliente_txt.Text);
                    aux.IdCustomer = dao.GetIdCustomer(id_pessoa);

                    // Inserindo serviço no banco
                    dao.EditCredit(aux);

                    // Registro de log - Edição
                    Model.Log edited = new Model.Log();
                    edited.employe = _employe;
                    edited.action = $"Contas a receber: {aux.Description} foi alterado no sistema!";
                    logs.Register(edited);

                    // Atualizando grid e limpando campos de texto
                    RefreshGrid();
                    ClearFields();
                    BlockFields();
                    actionIndex = -1;
                    aux = null;

                }
                catch (DatabaseEditException err) {
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
            else {
                MessageBox.Show(
                    "Há Campos Vazios",
                    "Erro de Prenchimento de Formulário",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        /// <summary>
        /// Metodo para coletar dados e deletar serviço
        /// </summary>
        private void DeleteCredit() {

            try {

                // Remoção do serviço
                dao.DeleteCredit(aux.IdCredit);

                // Registro de log - Edição
                Model.Log deleted = new Model.Log();
                deleted.employe = _employe;
                deleted.action = $"Contas a receber: {aux.Description} foi removido no sistema!";
                logs.Register(deleted);

                // Atualizando grid e limpando campos de texto
                RefreshGrid();
                ClearFields();
                BlockFields();
                actionIndex = -1;
                aux = null;

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

        #region Grid-Data

        /// <summary>
        /// Metodo para atualizar dados da grid
        /// </summary>
        private void RefreshGrid() {

            // Removendo valor do campo de texto
            pesquisar_txt.Text = string.Empty;

            try {

                // Adicionando nova fonte de dados
                dgReceber.ItemsSource = dao.GetAllCredit(pesquisar_txt.Text);

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

        /// <summary>
        /// Metodo para atualizar grid 
        /// </summary>
        /// <param name="param">parâmetro de busca</param>
        private void RefreshGrid(string param) {

            try {

                // Adicionando nova fonte de dados
                dgReceber.ItemsSource = dao.GetAllCredit(param);

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

        #endregion

        #region Utils

        private bool Validate() {
            return !(cliente_txt.Text.Equals(string.Empty) || valorTotal_txt.Text.Equals(string.Empty)
                     || valorIncial_txt.Text.Equals(string.Empty) || dataPagamento_txt.Text.Equals(string.Empty)
                     || dataVencimento_txt.Text.Equals(string.Empty) || descricao_txt.Text.Equals(string.Empty));
        }

        private void BlockFields() {

            // Buttons
            btnCancelar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
            btnSelect.IsEnabled = false;

            // Text Fields
            valorTotal_txt.IsEnabled = false;
            valorIncial_txt.IsEnabled = false;
            dataPagamento_txt.IsEnabled = false;
            dataVencimento_txt.IsEnabled = false;
            descricao_txt.IsEnabled = false;
        }

        private void EnableFields() {

            // Buttons
            btnCancelar.IsEnabled = true;
            btnSalvar.IsEnabled = true;
            btnSelect.IsEnabled = true;

            // Text Fields
            valorTotal_txt.IsEnabled = true;
            valorIncial_txt.IsEnabled = true;
            dataPagamento_txt.IsEnabled = true;
            dataVencimento_txt.IsEnabled = true;
            descricao_txt.IsEnabled = true;
        }

        private void ClearFields() {
            valorTotal_txt.Text = string.Empty;
            valorIncial_txt.Text = string.Empty;
            dataPagamento_txt.Text = string.Empty;
            dataVencimento_txt.Text = string.Empty;
            descricao_txt.Text = string.Empty;
            cliente_txt.Text = string.Empty;
        }

        #endregion

        #region Events

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            RefreshGrid();
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e) {
            actionIndex = 1;
            btnCancelar.Content = "Cancelar";
            aux = new Credit();
            ClearFields();
            EnableFields();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e) {
            if (dgReceber.SelectedItems.Count > 0) {
                actionIndex = 2;
                EnableFields();
            }
            else {
                MessageBox.Show(
                        "Nenhuma conta a receber foi selecionada! Selecione uma para prosseguir...",
                        "Validação",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) {
            BlockFields();
            ClearFields();
            actionIndex = -1;
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e) {
            switch (actionIndex) {

                case 1:
                    AddCredit();
                    break;
                case 2:
                    EditCredit();
                    break;
                case 3:
                    DeleteCredit();
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

        private void pesquisar_txt_KeyDown(object sender, KeyEventArgs e) {
            RefreshGrid(pesquisar_txt.Text);
        }

        private void dgReceber_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (dgReceber.SelectedItems.Count > 0) {
                try {

                    //Recuperando dados do cliente selecionado
                    Credit source = dgReceber.SelectedItem as Credit;
                    aux = dao.GetCreditData(source.IdCredit);

                    //Preenchendo campos (Textboxes)
                    cliente_txt.Text = dao.GetNameCustomer(dao.GetIdCustomer2(aux.IdCustomer));
                    valorTotal_txt.Text = aux.TotalAmount.ToString();
                    valorIncial_txt.Text = aux.StartAmount.ToString();
                    dataPagamento_txt.Text = aux.ReceiptDate;
                    dataVencimento_txt.Text = aux.DueDate;
                    descricao_txt.Text = aux.Description;


                    //Definindo ação como nula
                    actionIndex = -1;

                }
                catch (DatabaseAccessException err) {
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

        private void btnSelect_Click(object sender, RoutedEventArgs e) {
            SearchCustomer window = new SearchCustomer(this);
            window.Show();
        }

        #endregion
    }
}
