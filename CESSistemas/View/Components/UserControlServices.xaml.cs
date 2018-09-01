using Promig.Connection.Methods;
using Promig.Exceptions;
using Promig.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Promig.View.Components {
    
    public partial class UserControlServices : UserControl {

        #region Header

        private Services dao;
        private Logs logs;
        private Service aux;
        private Employe _employe;
        private int actionIndex;

        #endregion

        #region Constructors

        public UserControlServices() {

            // Inicializando componentes
            InitializeComponent();

            // Instanciando objetos
            dao = new Services();
            logs = new Logs();
            aux = null;
            _employe = new Employe();
            _employe.id = MainWindow.currentId;
            actionIndex = -1;
        }

        #endregion

        #region Events

        /// <summary>
        /// Evento ao carregar controle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void control_loaded(object sender, RoutedEventArgs e) {
            BlockFields();
            RefreshGrid();
        }

        /// <summary>
        /// Evento ao digitar em um campo numerico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueEdit_KeyDown(object sender, KeyEventArgs e) {
            KeyConverter kv = new KeyConverter();
            if ((char.IsNumber((string)kv.ConvertTo(e.Key, typeof(string)), 0) == false) && e.Key.Equals(Key.OemPeriod)) {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Evento de atualizar grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e) {
            RefreshGrid();
        }

        /// <summary>
        /// Evento ao clicar em adicionar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e) {
            actionIndex = 1;
            ClearFields();
            EnableFields();
            btnDelete.IsEnabled = false;
        }

        /// <summary>
        /// Evento ao clicar em editar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (dgServices.SelectedItems.Count > 0) {
                actionIndex = 2;
                EnableFields();
            } else {
                MessageBox.Show(
                    "Nenhum serviçofoi selecionado! Selecione algum para prosseguir...",
                    "Validação",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
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
        /// Evento ao clicar em cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e) {
                BlockFields();
                ClearFields();
                actionIndex = -1;         
        }

        /// <summary>
        /// Evento ao clicar em salvar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e) {
            switch (actionIndex) {
                case 1:
                    AddService();
                    break;
                case 2:
                    EditService();
                    break;
                case 3:
                    DeleteService();
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
        /// Evento ao selecionar serviço da grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dgServices_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            if (dgServices.SelectedItems.Count > 0) {

                // Recuperação do serviço selecionado
                aux = dgServices.SelectedItem as Service;

                // Preenchimento dos campos
                DescriptionEdit.Text = aux.Task;
                ValueEdit.Text = aux.Value.ToString();
            }
        }

        /// <summary>
        /// Evento ao digitar no campo de  pesquisa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchEdit_KeyDown(object sender, RoutedEventArgs e) {
            RefreshGrid(SearchEdit.Text);
        }

        #endregion

        #region Data-Gathering

        /// <summary>
        /// Metodo para coletar dados e adicionar serviço
        /// </summary>
        private void AddService() {
            if (IsFilledFields()) {

                try {

                    // Recuperando dados
                    Service service = new Service();
                    service.Task = DescriptionEdit.Text;
                    service.Value = double.Parse(ValueEdit.Text);

                    // Inserindo serviço no banco
                    dao.AddService(service);

                    // Registro de log - Inserção
                    Model.Log added = new Model.Log();
                    added.employe = _employe;
                    added.action = $"Serviço: {service.Task} foi cadastrado no sistema!";
                    logs.Register(added);

                    // Atualizando grid e limpando campos de texto
                    RefreshGrid();
                    ClearFields();
                    BlockFields();
                    actionIndex = -1;
                    aux = null;

                } catch (DatabaseInsertException err) {
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
        /// Metodo para coletar dados e Editar Serviço
        /// </summary>
        private void EditService() {
            if (IsFilledFields()) {

                try {

                    // Recuperando dados
                    aux.Task = DescriptionEdit.Text;
                    aux.Value = double.Parse(ValueEdit.Text);

                    // Inserindo serviço no banco
                    dao.EditService(aux);

                    // Registro de log - Edição
                    Model.Log edited = new Model.Log();
                    edited.employe = _employe;
                    edited.action = $"Serviço: {aux.Task} foi alterado no sistema!";
                    logs.Register(edited);

                    // Atualizando grid e limpando campos de texto
                    RefreshGrid();
                    ClearFields();
                    BlockFields();
                    actionIndex = -1;
                    aux = null;

                } catch (DatabaseEditException err) {
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
        /// Metodo para coletar dados e deletar serviço
        /// </summary>
        private void DeleteService() {

            try {

                // Remoção do serviço
                dao.DeleteService(aux);

                // Registro de log - Edição
                Model.Log deleted = new Model.Log();
                deleted.employe = _employe;
                deleted.action = $"Serviço: {aux.Task} foi removido no sistema!";
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

        #region Grid-Data

        /// <summary>
        /// Metodo para atualizar dados da grid
        /// </summary>
        private void RefreshGrid() {

            // Removendo valor do campo de texto
            SearchEdit.Text = string.Empty;

            try {

                // Adicionando nova fonte de dados
                dgServices.ItemsSource = dao.GetAllServices(SearchEdit.Text);

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
        /// Metodo para atualizar grid 
        /// </summary>
        /// <param name="param">parâmetro de busca</param>
        private void RefreshGrid(string param) {

            try {

                // Adicionando nova fonte de dados
                dgServices.ItemsSource = dao.GetAllServices(param);

            } catch (DatabaseAccessException err) {
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

        /// <summary>
        /// Método para verificar se campos estão preenchidos
        /// </summary>
        /// <returns>Retorna true se preenchido normalmente e false se não</returns>
        private bool IsFilledFields() {
            return !(
                DescriptionEdit.Text.Equals(string.Empty) ||
                ValueEdit.Text.Equals(string.Empty)
            );
        }

        /// <summary>
        /// Método para bloquear campos de texto
        /// </summary>
        private void BlockFields() {

            // Buttons
            btnDelete.IsEnabled = false;
            btnCancelar.IsEnabled = false;
            btnSalvar.IsEnabled = false;

            // Text Fields
            DescriptionEdit.IsEnabled = false;
            ValueEdit.IsEnabled = false;
        }

        /// <summary>
        /// Método para desbloquear campos de texto
        /// </summary>
        private void EnableFields() {

            // Buttons
            btnDelete.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            btnSalvar.IsEnabled = true;

            // Text Fields
            DescriptionEdit.IsEnabled = true;
            ValueEdit.IsEnabled = true;
        }

        /// <summary>
        /// Método para limpar campos de texto
        /// </summary>
        private void ClearFields() {
            DescriptionEdit.Text = string.Empty;
            ValueEdit.Text = string.Empty;
        }

        #endregion
    }
}
