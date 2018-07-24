using System;
using System.Runtime.InteropServices;
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

        [DllImport("wininet.dll")]
        private extern static Boolean InternetGetConnectedState(out int Description, int ReservedValue);

        private Suppliers dao;
        private Logs logs;
        private Supplier aux;
        private Employe _employe;
        private int actionIndex;

        #endregion

        #region Constructors

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

        //Evento ao pesquisar na caixa de pesquisa
        private void txtSearch_KeyDown(object sender, RoutedEventArgs e) {
            RefreshGrid(txtSearch.Text);
        }

        //Evento para exibir em mapa
        private void btnShowInMap_Click(object sender, RoutedEventArgs e) {

            //Verificando se ha um cliente selecionado para exibição
            if (dgSuppliers.SelectedItems.Count > 0) {

                //Verificando conexão com internet
                if (!IsConnected()) {
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

        //Evento ao clicar em algum funcionário
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

        //Evento para bloquear números
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

        public static Boolean IsConnected() {
            int Description;
            return InternetGetConnectedState(out Description, 0);
        }


        private void SetDefaults() {

            //Definindo valor inicial de combobox
            cbActive.SelectedIndex = 0;
            cbState.SelectedIndex = 0;
            cbSearch.SelectedIndex = 0;
        }

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
