using Promig.Connection.Methods;
using Promig.Model;
using System.Windows;
using System.Windows.Controls;

namespace Promig.View.Components {
    
    public partial class UserControlServices : UserControl {

        #region Header

        private Services dao;
        private Logs logs;
        private Service aux;
        private Employe _employe;
        private bool deletable;
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
            deletable = false;
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
            //RefreshGrid();
        }

        /// <summary>
        /// Evento de atualizar grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e) {

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
        /// Evento ao clicar em cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (deletable) {
                
            } else {
                BlockFields();
                ClearFields();
                actionIndex = -1;
            }
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

        #endregion

        #region Data-Gathering

        private void AddService() {
            if (IsFilledFields()) {

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

        private void EditService() {
            if (IsFilledFields()) {

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
