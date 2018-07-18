using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Promig.Connection.Methods;
using Promig.Exceptions;
using Promig.Model;
using Promig.Utils; 

namespace Promig.View.Components {
    
    public partial class UserControlClient : UserControl {

        #region Header

        private Clients dao;
        private Logs logs;
        private Client aux;
        private Employe _employe;
        private int actionIndex;

        #endregion

        #region constructors

        public UserControlClient() {

            //Iniciando componentes
            InitializeComponent();

            //Instanciando objetos
            actionIndex = -1;
            _employe = new Employe();
            _employe.id = MainWindow.currentId;
            aux = null;
            logs = new Logs();
            dao = new Clients();
        }

        #endregion constructors

        #region Events

        //Evento ao carregar componente
        private void control_loaded(object sender, RoutedEventArgs e) {
            SetDefaults();
            BlockFields();
            RefreshGrid();
        }

        //Evento ao alternar parametro de pesquisa
        private void cbSearch_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            RefreshGrid();
        }

        //Evento ao alternar tipo de cliente
        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            if (cbtype.SelectedIndex == 0) {
                docEdit.Mask = "999,999,999-99";
                stateEdit.IsEnabled = false;
            }
            else {
                docEdit.Mask = "99,999,999/9999-99";
                stateEdit.IsEnabled = true;
            }
        }

        //Evento ao pesquisar na caixa de pesquisa
        private void txtSearch_KeyDown(object sender, RoutedEventArgs e) {
            RefreshGrid(txtSearch.Text);
        }

        //Evento para atualizar grid manualmente
        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            RefreshGrid();
        }

        //Evento de autopreencher cep
        private void cepEdit_PreviewKeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                string cep = cepEdit.Text.Replace("-", "").Replace("_", "");
                Adress adress = WSAdress.GetAdress(cep);
                //Recuperando dados gerais
                AdressEdit.Text = adress.street;
                NeighboorhoodEdit.Text = adress.neighborhood;
                CityEdit.Text = adress.city;
                //Recuperando Estado
                int index = -1;
                foreach (ComboBoxItem item in cbState.Items) {
                    index++;
                    if (item.Content.Equals(adress.UF)) break;
                }
                cbState.SelectedIndex = index;
            }
        }

        private void dgClients_SelectionChanged(object sender, SelectionChangedEventArgs args) {

            //Verificando se ha seleção feita
            if (dgClients.SelectedItems.Count > 0) {

                //Limpando campos (para garantir)
                ClearFields();

                //Recuperando dados do cliente selecionado
                Client source = dgClients.SelectedItem as Client;
                aux = dao.GetClientData(source.id);

                //Preenchendo campos (Comboboxes)
                if (aux.IsActive()) cbActive.SelectedIndex = 0; else cbActive.SelectedIndex = 1;
                if (aux.IsPhysical()) cbtype.SelectedIndex = 0; else cbtype.SelectedIndex = 1;

                //Recuperando Estado
                int index = -1;
                foreach (ComboBoxItem item in cbState.Items) {
                    index++;
                    if (item.Content.Equals(aux.adress.UF)) break;
                }
                cbState.SelectedIndex = index;

                //Preenchendo campos (Textboxes)
                NameEdit.Text = aux.name;
                docEdit.Text = aux.docNumber;
                AdressEdit.Text = aux.adress.street;
                NeighboorhoodEdit.Text = aux.adress.neighborhood;
                cepEdit.Text = aux.adress.CEP;
                NumberEdit.Text = aux.adress.number;
                CityEdit.Text = aux.adress.city;
                phone1Edit.Text = aux.residenceNumber;
                phone2Edit.Text = aux.cellNumber;
                descEdit.Text = aux.description;
                stateEdit.Text = aux.stateId;

                //Definindo ação como nula
                actionIndex = -1;

                //Desabiilitando campos e preenchendo com dados
                BlockFields();
            }
        }

        private void numberEdit_KeyDown(object sender, KeyEventArgs e) {
            KeyConverter kv = new KeyConverter();
            if ((char.IsNumber((string)kv.ConvertTo(e.Key, typeof(string)), 0) == false)) {
                e.Handled = true;
            }
        }

        //Evento para botão adicionar
        private void btnAdicionar_Click(object sender, RoutedEventArgs e) {
            //Habilitando campos para inserção
            actionIndex = 1;
            ClearFields();
            EnableFields();
        }

        //Evento para botão editar
        private void btnEditar_Click(object sender, RoutedEventArgs e) {

            //Verificando se ha funcionario selecionada
            if (dgClients.SelectedItems.Count > 0) {

                //Desbloqueando os campos
                actionIndex = 2;
                EnableFields();
            }
            else {
                MessageBox.Show(
                        "Nenhum cliente foi selecionado! Selecione um para prosseguir...",
                        "Validação",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
            }
        }

        //Evento para botão cancelar
        private void btnCancelar_Click(object sender, RoutedEventArgs e) {

            //Bloqueando campos e resetando transação
            BlockFields();
            ClearFields();
            actionIndex = -1;
        }

        //Evento para botão salvar
        private void btnSalvar_Click(object sender, RoutedEventArgs e) {

            //Verificando qual ação sera realizada
            switch (actionIndex) {

                case 1:
                    AddClient();
                    break;

                case 2:
                    EditClient();
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

        private void AddClient() {
            if (IsValidFields()) {

                //Recuperando documentos no formato correto
                string doc = docEdit.Text.Replace(".", "").Replace("-", "").Replace("_", ".").Replace("/", "");
                string cep = cepEdit.Text.Replace("-", "").Replace("_", "");

                //Validando os documentos
                if (cbtype.SelectedIndex == 0) {
                    if (!Validator.IsCpf(doc)) {
                        MessageBox.Show(
                        "CPF Inválido",
                        "Dados incorretos!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                        return;
                    }
                }
                else {
                    if (!Validator.IsCnpj(doc)) {
                        MessageBox.Show(
                        "CNPJ Inválido",
                        "Dados incorretos!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                        return;
                    }
                }

                try {

                    //Recuperando dados do cliente
                    Client cli = new Client();
                    ComboBoxItem selected = cbState.Items[cbState.SelectedIndex] as ComboBoxItem;
                    cli.adress.street = AdressEdit.Text;
                    cli.adress.city = CityEdit.Text;
                    cli.adress.neighborhood = NeighboorhoodEdit.Text;
                    cli.adress.number = NumberEdit.Text;
                    cli.adress.UF = selected.Content.ToString();
                    cli.adress.CEP = cep;
                    cli.name = NameEdit.Text;
                    cli.docNumber = doc;
                    cli.cellNumber = phone2Edit.Text;
                    cli.residenceNumber = phone1Edit.Text;
                    cli.description = descEdit.Text;
                    cli.stateId = stateEdit.Text;
                    if (cbActive.SelectedIndex == 1) cli.Inactivate();
                    if (cbtype.SelectedIndex == 1) cli.SetPhysical(false);

                    //Inserindo registro no banco
                    dao.AddClient(cli);

                    //Registrando log de alteração
                    Model.Log added = new Model.Log();
                    added.employe = _employe;
                    added.action = $"Cliente {cli.name} foi cadastrado no sistema!";
                    logs.Register(added);

                    //Atualizando grid e limpando campos de texto
                    RefreshGrid();
                    ClearFields();
                    BlockFields();
                    actionIndex = -1;
                    aux = null;
                }
                catch (DatabaseInsertException err) {
                    MessageBox.Show(
                        err.Message,
                        "Erro ao gravar dados",
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

        private void EditClient() {

            //Verificando se campos estão preenchidos
            if (IsValidFields()) {

                //Recuperando documentos no formato correto
                string doc = docEdit.Text.Replace(".", "").Replace("-", "").Replace("_", ".").Replace("/", "");
                string cep = cepEdit.Text.Replace("-", "").Replace("_", "");

                //Validando os documentos
                if (cbtype.SelectedIndex == 0) {
                    if (!Validator.IsCpf(doc)) {
                        MessageBox.Show(
                        "CPF Inválido",
                        "Dados incorretos!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                        return;
                    }
                }
                else {
                    if (!Validator.IsCnpj(doc)) {
                        MessageBox.Show(
                        "CNPJ Inválido",
                        "Dados incorretos!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                        return;
                    }
                }

                try {

                    //Recuperando dados do funcionário
                    ComboBoxItem selected = cbState.Items[cbState.SelectedIndex] as ComboBoxItem;
                    aux.adress.street = AdressEdit.Text;
                    aux.adress.city = CityEdit.Text;
                    aux.adress.neighborhood = NeighboorhoodEdit.Text;
                    aux.adress.number = NumberEdit.Text;
                    aux.adress.UF = selected.Content.ToString();
                    aux.adress.CEP = cep;
                    aux.name = NameEdit.Text;
                    aux.docNumber = doc;
                    aux.cellNumber = phone2Edit.Text;
                    aux.residenceNumber = phone1Edit.Text;
                    aux.description = descEdit.Text;
                    aux.stateId = stateEdit.Text;
                    if (cbActive.SelectedIndex == 1) aux.Inactivate(); else aux.Activate();
                    if (cbtype.SelectedIndex == 1) aux.SetPhysical(false); else aux.SetPhysical(true);

                    //Alterando registro no banco
                    dao.EditClient(aux);

                    //Registrando log de alteração
                    Model.Log edited = new Model.Log();
                    edited.employe = _employe;
                    edited.action = $"Cliente {aux.name} com ID = {aux.id} sofreu alteração no sistema!";
                    logs.Register(edited);

                    //Atualizando grid e limpando campos de texto
                    RefreshGrid();
                    ClearFields();
                    BlockFields();
                    actionIndex = -1;
                    aux = null;

                }
                catch (DatabaseEditException err) {

                    //Retornando mensagem de erro para usuário
                    MessageBox.Show(
                        err.Message,
                        "Erro ao gravar dados",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
            else {
                //Mostrando alerta de validação
                MessageBox.Show(
                    "Há Campos Vazios",
                    "Erro de Prenchimento de Formulário",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        #endregion

        #region Grid-Param

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

        #region Utils

        private void SetDefaults() {

            //Definindo valor inicial de combobox
            cbActive.SelectedIndex = 0;
            cbtype.SelectedIndex = 0;
            cbState.SelectedIndex = 0;
            cbSearch.SelectedIndex = 0;
        }

        private bool IsValidFields() {
            string doc = docEdit.Text.Replace(".", "").Replace("-", "").Replace("_", ".").Replace("/", "").Replace(" ","");
            string cep = cepEdit.Text.Replace("-", "").Replace("_", "").Replace(" ", "");
            string phone = phone1Edit.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace(" ", "");
            return !(NameEdit.Text.Equals("") ||
                    doc.Equals("") ||
                    AdressEdit.Text.Equals("") ||
                    NeighboorhoodEdit.Text.Equals("") ||
                    CityEdit.Text.Equals("") ||
                    NumberEdit.Text.Equals("") ||
                    cep.Equals("") ||
                    phone.Equals("")
            );
        }

        private void ClearFields() {

            //Campos de texto
            NameEdit.Text = null;
            docEdit.Text = null;
            AdressEdit.Text = null;
            NeighboorhoodEdit.Text = null;
            NumberEdit.Text = null;
            cepEdit.Text = null;
            CityEdit.Text = null;
            phone1Edit.Text = null;
            phone2Edit.Text = null;
            descEdit.Text = null;
            stateEdit.Text = null;

            //Comboboxes
            cbActive.SelectedIndex = 0;
            cbtype.SelectedIndex = 0;
            cbState.SelectedIndex = 0;
        }

        private void EnableFields() {

            //Comboboxes
            cbActive.IsEnabled = true;
            cbtype.IsEnabled = true;
            cbState.IsEnabled = true;

            //Campos de texto
            NameEdit.IsEnabled = true;
            docEdit.IsEnabled = true;
            AdressEdit.IsEnabled = true;
            NeighboorhoodEdit.IsEnabled = true;
            NumberEdit.IsEnabled = true;
            cepEdit.IsEnabled = true;
            CityEdit.IsEnabled = true;
            phone1Edit.IsEnabled = true;
            phone2Edit.IsEnabled = true;
            descEdit.IsEnabled = true;
            if (cbtype.SelectedIndex == 1) stateEdit.IsEnabled = true;
            else stateEdit.IsEnabled = false;

            //Botões
            btnCancelar.IsEnabled = true;
            btnSalvar.IsEnabled = true;
        }

        private void BlockFields() {

            //Comboboxes
            cbActive.IsEnabled = false;
            cbtype.IsEnabled = false;
            cbState.IsEnabled = false;

            //Campos de texto
            NameEdit.IsEnabled = false;
            docEdit.IsEnabled = false;
            AdressEdit.IsEnabled = false;
            NeighboorhoodEdit.IsEnabled = false;
            NumberEdit.IsEnabled = false;
            cepEdit.IsEnabled = false;
            CityEdit.IsEnabled = false;
            phone1Edit.IsEnabled = false;
            phone2Edit.IsEnabled = false;
            descEdit.IsEnabled = false;
            stateEdit.IsEnabled = false;

            //Botões
            btnCancelar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
        }

        #endregion
    }
}
