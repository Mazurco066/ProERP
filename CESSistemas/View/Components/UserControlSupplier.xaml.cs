using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Promig.Connection.Methods;
using Promig.Exceptions;
using Promig.Model;
using Promig.Utils;

namespace Promig.View.Components {
    
    public partial class UserControlSupplier : UserControl {

        #region Header

        private Suppliers dao;
        private Logs logs;
        private Supplier aux;
        private Employe _employe;
        private int actionIndex;

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor de uso padrão
        /// </summary>
        public UserControlSupplier() {

            //Iniciando os componentes
            InitializeComponent();

            //Instanciando objetos
            actionIndex = -1;
            _employe = new Employe();
            _employe.id = MainWindow.currentId;
            aux = null;
            logs = new Logs();
            dao = new Suppliers();
        }

        #endregion

        #region Events

        /// <summary>
        /// Evento ao carregar controle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void control_loaded(object sender, RoutedEventArgs e) {
            SetDefaults();
            BlockFields();
            RefreshGrid();
        }

        /// <summary>
        /// Evento ao alternar parametro de pesquisa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void cbSearch_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            RefreshGrid();
        }

        /// <summary>
        /// Eventoao digitar no campo de pesquisa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_KeyDown(object sender, RoutedEventArgs e) {
            RefreshGrid(txtSearch.Text);
        }

        /// <summary>
        /// Evento aopressionar botão de visualização em mapa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowInMap_Click(object sender, RoutedEventArgs e) {

            //Verificando se ha um cliente selecionado para exibição
            if (dgSuppliers.SelectedItems.Count > 0) {

                //Verificando conexão com internet
                if (!isNetWorkConnection()) {
                    MessageBox.Show(
                        "Você precisa estar conectado a internet para usar esse recurso!",
                        "Erro de Conexão!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }

                //Recuperando dados do cliente selecionado
                Supplier source = dgSuppliers.SelectedItem as Supplier;
                Supplier supplier = dao.GetSupplierData(source.id);

                //Construindo string de endereço employe
                string location = $"{supplier.adress.street}, {supplier.adress.number} {supplier.adress.neighborhood}, {supplier.adress.city}-{supplier.adress.UF}";

                //Enviando dados e iniciando exibição em mapa
                MapWindow window = new MapWindow(location);
                window.Show();
            }
            else {
                MessageBox.Show(
                    "Selecione um registro para visualizar no mapa!",
                    "Aviso!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        /// <summary>
        /// Evento ao pressionar botão de atualização do conteúdo da grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            RefreshGrid();
        }

        /// <summary>
        /// Evento ao pressionar enter no campo de CEP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Evento ao selecionar algum registro na grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dgSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs args) {

            //Verificando se ha seleção feita
            if (dgSuppliers.SelectedItems.Count > 0) {

                //Recuperando dados do cliente selecionado
                Supplier source = dgSuppliers.SelectedItem as Supplier;
                aux = dao.GetSupplierData(source.id);

                //Preenchendo campos (Comboboxes)
                if (aux.IsActive()) cbActive.SelectedIndex = 0; else cbActive.SelectedIndex = 1;

                //Recuperando Estado
                int index = -1;
                foreach (ComboBoxItem item in cbState.Items) {
                    index++;
                    if (item.Content.Equals(aux.adress.UF)) break;
                }
                cbState.SelectedIndex = index;

                //Preenchendo campos (Textboxes)
                NameEdit.Text = aux.name;
                cnpjEdit.Text = aux.cnpj;
                AdressEdit.Text = aux.adress.street;
                NeighboorhoodEdit.Text = aux.adress.neighborhood;
                cepEdit.Text = aux.adress.CEP;
                NumberEdit.Text = aux.adress.number;
                CityEdit.Text = aux.adress.city;
                phone1Edit.Text = aux.resPhone;
                phone2Edit.Text = aux.cellPhone;

                //Definindo ação como nula
                actionIndex = -1;

                //Desabiilitando campos e preenchendo com dados
                BlockFields();
            }
        }

        /// <summary>
        /// Evento para bloquear entrada de caracteres no campo de numero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberEdit_KeyDown(object sender, KeyEventArgs e) {
            KeyConverter kv = new KeyConverter();
            if ((char.IsNumber((string)kv.ConvertTo(e.Key, typeof(string)), 0) == false)) {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Evento ao pressionar botão de adicionar registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdicionar_Click(object sender, RoutedEventArgs e) {
            //Habilitando campos para inserção
            actionIndex = 1;
            ClearFields();
            EnableFields();
        }

        /// <summary>
        /// Evento ao pressioar botão de alterar registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditar_Click(object sender, RoutedEventArgs e) {

            //Verificando se ha funcionario selecionada
            if (dgSuppliers.SelectedItems.Count > 0) {

                //Desbloqueando os campos
                actionIndex = 2;
                EnableFields();
            }
            else {
                MessageBox.Show(
                        "Nenhum fornecedor foi selecionado! Selecione um para prosseguir...",
                        "Validação",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
            }
        }

        /// <summary>
        /// Evento ao pressionar botão cancelar edição
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e) {

            //Bloqueando campos e resetando transação
            BlockFields();
            ClearFields();
            actionIndex = -1;
        }

        /// <summary>
        /// Evento ao pressionar botão salvar registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalvar_Click(object sender, RoutedEventArgs e) {

            //Verificando qual ação sera realizada
            switch (actionIndex) {

                case 1:
                    AddSupplier();
                    break;

                case 2:
                    EditSupplier();
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

        /// <summary>
        /// Método para recuperar dados do registro e realizar interação com banco
        /// </summary>
        private void AddSupplier() {
            if (IsValidFields()) {
                string cnpj = cnpjEdit.Text.Replace(".", "").Replace("-", "").Replace("_", ".").Replace("/", "");
                string cep = cepEdit.Text.Replace("-", "").Replace("_", "");
                if (Validator.IsCnpj(cnpj)) {  //Validando documento
                    try {
                        //Recuperando dados do funcionário
                        Supplier sup = new Supplier();
                        ComboBoxItem selected = cbState.Items[cbState.SelectedIndex] as ComboBoxItem;
                        sup.adress.street = AdressEdit.Text;
                        sup.adress.city = CityEdit.Text;
                        sup.adress.neighborhood = NeighboorhoodEdit.Text;
                        sup.adress.number = NumberEdit.Text;
                        sup.adress.UF = selected.Content.ToString();
                        sup.adress.CEP = cep;
                        sup.name = NameEdit.Text;
                        sup.cnpj = cnpj;
                        sup.cellPhone = phone2Edit.Text;
                        sup.resPhone = phone1Edit.Text;
                        if (cbActive.SelectedIndex == 1) sup.Inactivate();  

                        //Inserindo registro no banco
                        dao.AddSupplier(sup);

                        //Registrando log de alteração
                        Model.Log added = new Model.Log();
                        added.employe = _employe;
                        added.action = $"Fornecedor {sup.name} foi cadastrado no sistema!";
                        logs.Register(added);

                        //Atualizando grid e limpando campos de texto
                        RefreshGrid();
                        ClearFields();
                        BlockFields();
                        actionIndex = -1;
                        aux = null;

                    }
                    catch (DatabaseInsertException err) {
                        //Retornando mensagem de erro para usuário
                        MessageBox.Show(
                        err.Message,
                        "Erro ao gravar dados",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    }
                }
                else {
                    //Retornando mensagem de validação
                    MessageBox.Show(
                        "CNPJ Inválido",
                        "Dados incorretos!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
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

        /// <summary>
        /// Método para recuperar dados do registro e fazer interação com banco
        /// </summary>
        private void EditSupplier() {

            //Verificando se campos estão preenchidos
            if (IsValidFields()) {
                string cnpj = cnpjEdit.Text.Replace(".", "").Replace("-", "").Replace("_", ".").Replace("/","");
                string cep = cepEdit.Text.Replace("-", "").Replace("_", "");
                if (Validator.IsCnpj(cnpj)) {  //Validando documentos
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
                        aux.cnpj = cnpj;
                        aux.cellPhone = phone2Edit.Text;
                        aux.resPhone = phone1Edit.Text;
                        if (cbActive.SelectedIndex == 1) aux.Inactivate(); else aux.Activate();
                      
                        //Alterando registro no banco
                        dao.EditSupplier(aux);

                        //Registrando log de alteração
                        Model.Log edited = new Model.Log();
                        edited.employe = _employe;
                        edited.action = $"Fornecedor {aux.name} com ID = {aux.id} sofreu alteração no sistema!";
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
                        MessageBoxImage.Error);
                    }

                }
                else {
                    //Retornando mensagem de validação
                    MessageBox.Show(
                        "CNPJ Inválido(s)",
                        "Dados incorretos!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
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

        /// <summary>
        /// Método para atualizar conteúdo da grid
        /// </summary>
        private void RefreshGrid() {

            //Limpando campo de pesquisa
            txtSearch.Text = null;

            try {

                //Filtros de busca
                switch (cbSearch.SelectedIndex) {

                    case 0: //Ativo - nome
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliers(txtSearch.Text);
                        break;

                    case 1: //Todos - nome
                        dgSuppliers.ItemsSource = dao.GetAllSuppliers(txtSearch.Text);
                        break;

                    case 2: //Ativo - Cidade
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByCity(txtSearch.Text);
                        break;

                    case 3: //Ativo - CPF
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByDocument(txtSearch.Text);
                        break;
                }
            }
            catch(DatabaseAccessException err) {
                MessageBox.Show(
                    err.Message,
                    "Problemas ao acessar o banco!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        /// <summary>
        /// Método para atualizar conteúdo da grid que recebe parametros de busca
        /// </summary>
        /// <param name="param">Conteúdo a ser buscado nos registros</param>
        private void RefreshGrid(string param) {

            try {

                //Filtros de busca
                switch (cbSearch.SelectedIndex) {

                    case 0: //Ativo - nome
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliers(param);
                        break;

                    case 1: //Todos - nome
                        dgSuppliers.ItemsSource = dao.GetAllSuppliers(param);
                        break;

                    case 2: //Ativo - Cidade
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByCity(param);
                        break;

                    case 3: //Ativo - CPF
                        dgSuppliers.ItemsSource = dao.GetAllActiveSuppliersByDocument(param);
                        break;
                }
            }
            catch(DatabaseAccessException err) {
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

        /// <summary>
        /// Método para verificar conexão com internet
        /// </summary>
        /// <returns></returns>
        private bool isNetWorkConnection() {
            if (NetworkInterface.GetIsNetworkAvailable()) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Método para definir valores padrão nos campos
        /// </summary>
        private void SetDefaults() {

            //Definindo valor inicial de combobox
            cbActive.SelectedIndex = 0;
            cbState.SelectedIndex = 0;
            cbSearch.SelectedIndex = 0;
        }

        /// <summary>
        /// Método para verificar se campos estao devidamente preenchidos
        /// </summary>
        /// <returns></returns>
        private bool IsValidFields() {
            string doc = cnpjEdit.Text.Replace(".", "").Replace("-", "").Replace("_", "").Replace("/", "").Replace(" ", "");
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

        /// <summary>
        /// Método para limpar campos de texto
        /// </summary>
        private void ClearFields() {

            //Comboboxes
            cbActive.SelectedIndex = 0;
            cbState.SelectedIndex = 0;

            //Campos de texto
            NameEdit.Text = null;
            cnpjEdit.Text = null;
            AdressEdit.Text = null;
            NeighboorhoodEdit.Text = null;
            NumberEdit.Text = null;
            cepEdit.Text = null;
            CityEdit.Text = null;
            phone1Edit.Text = null;
            phone2Edit.Text = null;
        }

        /// <summary>
        /// Método para habilitar campos
        /// </summary>
        private void EnableFields() {

            //Comboboxes
            cbActive.IsEnabled = true;
            cbState.IsEnabled = true;

            //Campos de texto
            NameEdit.IsEnabled = true;
            cnpjEdit.IsEnabled = true;
            AdressEdit.IsEnabled = true;
            NeighboorhoodEdit.IsEnabled = true;
            NumberEdit.IsEnabled = true;
            cepEdit.IsEnabled = true;
            CityEdit.IsEnabled = true;
            phone1Edit.IsEnabled = true;
            phone2Edit.IsEnabled = true;

            //Botões
            btnCancelar.IsEnabled = true;
            btnSalvar.IsEnabled = true;
        }

        /// <summary>
        /// Método para bloquear campos
        /// </summary>
        private void BlockFields() {

            //Comboboxes
            cbActive.IsEnabled = false;
            cbState.IsEnabled = false;

            //Campos de texto
            NameEdit.IsEnabled = false;
            cnpjEdit.IsEnabled = false;
            AdressEdit.IsEnabled = false;
            NeighboorhoodEdit.IsEnabled = false;
            NumberEdit.IsEnabled = false;
            cepEdit.IsEnabled = false;
            CityEdit.IsEnabled = false;
            phone1Edit.IsEnabled = false;
            phone2Edit.IsEnabled = false;

            //Botões
            btnCancelar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
        }

        #endregion

    }
}
