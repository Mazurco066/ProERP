using Promig.Connection.Methods;
using Promig.Exceptions;
using Promig.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Promig.View.AuxComponents;

namespace Promig.View.Components {
    
    public partial class UserControlBillsToPay : UserControl {

        #region Header

        private Debits dao;
        private Logs logs;
        private Debit aux;
        private Employe _employe;
        private int actionIndex;
        public static string name_supplier;

        #endregion

        #region constructors
        public UserControlBillsToPay() {
            InitializeComponent();

            // Instanciando objetos
            dao = new Debits();
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
        private void AddDebit() {
            if (Validate()) {

                try {

                    // Recuperando dados
                    Debit debit = new Debit();
                    debit.TotalAmount = double.Parse(valorTotal_txt.Text);
                    debit.StartAmount = double.Parse(valorIncial_txt.Text);
                    debit.PaymentDate = dataPagamento_txt.Text;
                    debit.DueDate = dataVencimento_txt.Text;
                    debit.Description = descricao_txt.Text;
                    int id_pessoa = dao.GetIdPessoaSuppliier(fornecedor_txt.Text);
                    debit.IdSupplier = dao.GetIdSupplier(id_pessoa);

                    if(debit.IdSupplier == 0){
                        MessageBox.Show("Fornecedor invalido!!!", "Aviso",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Inserindo serviço no banco
                    dao.AddDebit(debit);

                    // Registro de log - Inserção
                    Model.Log added = new Model.Log();
                    added.employe = _employe;
                    added.action = $"Conta a pagar: {debit.Description} foi cadastrado no sistema!";
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
        private void EditDebit() {
            if (Validate()) {

                try {

                    // Recuperando dados
                    aux.TotalAmount = double.Parse(valorTotal_txt.Text);
                    aux.StartAmount = double.Parse(valorIncial_txt.Text);
                    aux.PaymentDate = dataPagamento_txt.Text;
                    aux.DueDate = dataVencimento_txt.Text;
                    aux.Description = descricao_txt.Text;
                    int id_pessoa = dao.GetIdPessoaSuppliier(fornecedor_txt.Text);
                    aux.IdSupplier = dao.GetIdSupplier(id_pessoa);

                    // Inserindo serviço no banco
                    dao.EditDebit(aux);

                    // Registro de log - Edição
                    Model.Log edited = new Model.Log();
                    edited.employe = _employe;
                    edited.action = $"Contas a pagar: {aux.Description} foi alterado no sistema!";
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
        private void DeleteDebit() {

            try {

                // Remoção do serviço
                dao.DeleteDebit(aux.IdDebit);

                // Registro de log - Edição
                Model.Log deleted = new Model.Log();
                deleted.employe = _employe;
                deleted.action = $"Contas a pagar: {aux.Description} foi removido no sistema!";
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
                dgPagar.ItemsSource = dao.GetAllDebit(pesquisar_txt.Text);

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
                dgPagar.ItemsSource = dao.GetAllDebit(param);

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

        private bool Validate(){
            return !(fornecedor_txt.Text.Equals(string.Empty) || valorTotal_txt.Text.Equals(string.Empty)
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
            fornecedor_txt.Text = string.Empty;
        }

        #endregion

        #region Events

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            RefreshGrid();
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e) {
            actionIndex = 1;
            btnCancelar.Content = "Cancelar";
            aux = new Debit();
            ClearFields();
            EnableFields();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e) {
            if (dgPagar.SelectedItems.Count > 0) {
                actionIndex = 2;
                EnableFields();
            }
            else {
                MessageBox.Show(
                        "Nenhuma conta a pagar foi selecionada! Selecione uma para prosseguir...",
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
                    AddDebit();
                    break;
                case 2:
                    EditDebit();
                    break;
                case 3:
                    DeleteDebit();
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

        private void dgPagar_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            if (dgPagar.SelectedItems.Count > 0) {
                try {

                    //Recuperando dados do cliente selecionado
                    Debit source = dgPagar.SelectedItem as Debit;
                    aux = dao.GetDebitData(source.IdDebit);

                    //Preenchendo campos (Textboxes)
                    fornecedor_txt.Text = dao.GetNameSupplier(dao.GetIdSupplier2(aux.IdSupplier));
                    valorTotal_txt.Text = aux.TotalAmount.ToString();
                    valorIncial_txt.Text = aux.StartAmount.ToString();
                    dataPagamento_txt.Text = aux.PaymentDate;
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
            SearchSupplier window = new SearchSupplier(this);
            window.Show();
        }

        #endregion

    }
}
