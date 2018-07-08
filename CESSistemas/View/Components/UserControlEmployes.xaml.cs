using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Promig.Connection.Methods;
using Promig.Exceptions;
using Promig.Model;
using Promig.Utils;

namespace Promig.View.Components {
    
    public partial class UserControlEmployes : UserControl {

        #region Header

        private Employes dao;
        private Logs logs;
        private Employe aux;
        private Employe _employe;
        private int actionIndex;

        #endregion

        #region Constructors

        public UserControlEmployes() {

            //Carregando os componentes
            InitializeComponent();

            //Instanciando objetos
            actionIndex = -1;
            _employe = new Employe();
            _employe.id = MainWindow.currentId;
            aux = null;
            logs = new Logs();
            dao = new Employes();
        }

        #endregion 

        #region Events

        //Evento ao carregar componente
        private void control_loaded(object sender, RoutedEventArgs e) {
            SetDefaults();
            BlockFields();
            RefreshGrid();
        }

        //Evento ao alternar tipo de usuário
        private void cbHasUser_SelectionChanged(object sender, SelectionChangedEventArgs args) {

            if (cbHasUser.SelectedIndex > 1)
                gridUser.Opacity = 0;
            else
                gridUser.Opacity = 100;
            
        }

        //Evento ao alternar parametro de pesquisa
        private void cbSearch_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            RefreshGrid();
        }

        //Evento ao pesquisar na caixa de pesquisa
        private void txtSearch_KeyDown(object sender, RoutedEventArgs e) {
            RefreshGrid(txtSearch.Text);
        }

        //Evento para atualizar grid manualmente
        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            RefreshGrid();
        }

        //Evento ao clicar em algum funcionário
        private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            
            //Verificando se ha seleção feita
            if (dgUsuarios.SelectedItems.Count > 0) {

                //Recuperando dados do cliente selecionado
                Employe source = dgUsuarios.SelectedItem as Employe;
                aux = dao.GetEmployeData(source.id);

                //Preenchendo campos (Comboboxes)
                if (aux.IsActive()) cbActive.SelectedIndex = 0; else cbActive.SelectedIndex = 1;
                if (aux.role.Equals("none")) cbHasUser.SelectedIndex = 2;
                else if (aux.role.Equals("User")) cbHasUser.SelectedIndex = 1;
                else cbHasUser.SelectedIndex = 0;

                //Recuperando Estado
                int index = -1;
                foreach(ComboBoxItem item in cbState.Items) {
                    index++;
                    if (item.Content.Equals(aux.adress.UF)) break;
                }
                cbState.SelectedIndex = index;

                //Preenchendo campos (Textboxes)
                NameEdit.Text = aux.name;
                cpfEdit.Text = aux.cpf;
                AdressEdit.Text = aux.adress.street;
                NeighboorhoodEdit.Text = aux.adress.neighborhood;
                cepEdit.Text = aux.adress.CEP;
                NumberEdit.Text = aux.adress.number;
                CityEdit.Text = aux.adress.city;
                admissionEdit.Text = aux.admission;
                RoleEdit.Text = aux.job;

                //Dados de usuário
                if (aux.user != null) {
                    //Preenchendo dados do usuário
                    usernameEdit.Text = aux.user.GetLogin();
                    passwordEdit.Password = aux.user.GetPassword();
                }

                //Definindo ação como nula
                actionIndex = -1;

                //Desabiilitando campos e preenchendo com dados
                BlockFields();
            }
        }

        //Evento de autopreencher cep
        private void  cepEdit_PreviewKeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                string cep = cepEdit.Text.Replace("-","").Replace("_","");
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
            if (dgUsuarios.SelectedItems.Count > 0) {

                //Desbloqueando os campos
                actionIndex = 2;
                EnableFields();
            }
            else {
                MessageBox.Show(
                        "Nenhum funcionário foi selecionado! Selecione um para prosseguir...",
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
                    AddEmploye();
                    break;

                case 2:
                    EditEmploye();
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

        private void AddEmploye() {
            if (IsValidFields()) {
                string cpf = cpfEdit.Text.Replace(".", "").Replace("-", "").Replace("_", ".");
                string cep = cepEdit.Text.Replace("-", "").Replace("_", "");
                if (Validator.IsCpf(cpf)) {  //Validando documento
                    try {
                        //Recuperando dados do funcionário
                        Employe emp = new Employe();
                        ComboBoxItem selected = cbState.Items[cbState.SelectedIndex] as ComboBoxItem;
                        emp.adress.street = AdressEdit.Text;
                        emp.adress.city = CityEdit.Text;
                        emp.adress.neighborhood = NeighboorhoodEdit.Text;
                        emp.adress.number = NumberEdit.Text;
                        emp.adress.UF = selected.Content.ToString();
                        emp.adress.CEP = cep;
                        emp.name = NameEdit.Text;
                        emp.cpf = cpf;
                        emp.admission = admissionEdit.Text;
                        emp.job = RoleEdit.Text;
                        if (cbActive.SelectedIndex == 1) emp.Inactivate();
                        if (cbHasUser.SelectedIndex == 2) {
                            emp.SetRole("none");
                            emp.SetUser(null);
                        }
                        else {
                            if (cbHasUser.SelectedIndex == 1) emp.SetRole("User"); else emp.SetRole("Admin");
                            emp.SetUser(new User(usernameEdit.Text, passwordEdit.Password));
                        }

                        //Inserindo registro no banco
                        dao.AddEmploye(emp);

                        //Registrando log de alteração
                        Model.Log added = new Model.Log();
                        added.employe = _employe;
                        added.action = "Funcionário " + emp.name + " foi cadastrado no sistema!";
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
                        "CPF Inválido",
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

        private void EditEmploye() {

            //Verificando se campos estão preenchidos
            if (IsValidFields()) {
                string cpf = cpfEdit.Text.Replace(".", "").Replace("-", "").Replace("_", ".");
                string cep = cepEdit.Text.Replace("-", "").Replace("_", "");
                if (Validator.IsCpf(cpf)) {  //Validando documentos
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
                        aux.cpf = cpf;
                        aux.admission = admissionEdit.Text;
                        aux.job = RoleEdit.Text;
                        if (cbActive.SelectedIndex == 1) aux.Inactivate(); else aux.Activate();
                        if (cbHasUser.SelectedIndex == 2) {
                            aux.SetRole("none");
                            aux.SetUser(null);
                        }
                        else {
                            if (cbHasUser.SelectedIndex == 1) aux.SetRole("User"); else aux.SetRole("Admin");
                            aux.SetUser(new User(usernameEdit.Text, passwordEdit.Password));
                        }

                        //Alterando registro no banco
                        dao.EditEmploye(aux);

                        //Registrando log de alteração
                        Model.Log edited = new Model.Log();
                        edited.employe = _employe;
                        edited.action = "Funcionário " + aux.GetName() + " com ID = " + aux.id + " sofreu alteração no sistema!";
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
                        "CPF ou RG Inválido(s)",
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

            //Filtros de busca
            switch (cbSearch.SelectedIndex) {

                case 0: //Ativo - nome
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployes(txtSearch.Text);
                    break;

                case 1: //Todos - nome
                    dgUsuarios.ItemsSource = dao.GetAllEmployes(txtSearch.Text);
                    break;

                case 2: //Ativo - Cidade
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployesByCity(txtSearch.Text);
                    break;

                case 3: //Ativo - CPF
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployesByDocument(txtSearch.Text);
                    break;
            }
        }

        private void RefreshGrid(string param) {

            //Filtros de busca
            switch (cbSearch.SelectedIndex) {

                case 0: //Ativo - nome
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployes(param);
                    break;

                case 1: //Todos - nome
                    dgUsuarios.ItemsSource = dao.GetAllEmployes(param);
                    break;

                case 2: //Ativo - Cidade
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployesByCity(param);
                    break;

                case 3: //Ativo - CPF
                    dgUsuarios.ItemsSource = dao.GetAllActiveEmployesByDocument(param);
                    break;
            }
        }

        #endregion

        #region Utils

        private void SetDefaults() {

            //Definindo valor inicial de combobox
            cbActive.SelectedIndex = 0;
            cbHasUser.SelectedIndex = 2;
            cbState.SelectedIndex = 0;
            cbSearch.SelectedIndex = 0;

            //Definindo data inicial
            admissionEdit.Text = DateBr.GetDateBr();
        }

        private bool IsValidFields() {
            //Verificando se ha ou n usuário
            if (cbHasUser.SelectedIndex != 2) {
                return !(NameEdit.Text.Equals("") ||
                        cpfEdit.Text.Equals("") ||
                        AdressEdit.Text.Equals("") ||
                        NeighboorhoodEdit.Text.Equals("") ||
                        CityEdit.Text.Equals("") ||
                        NumberEdit.Text.Equals("") ||
                        cepEdit.Text.Equals("") ||
                        admissionEdit.Text.Equals("") ||
                        RoleEdit.Text.Equals("") ||
                        usernameEdit.Text.Equals("") ||
                        passwordEdit.Password.Equals("")
                );
            }
            else {
                return !(NameEdit.Text.Equals("") ||
                        cpfEdit.Text.Equals("") ||
                        AdressEdit.Text.Equals("") ||
                        NeighboorhoodEdit.Text.Equals("") ||
                        CityEdit.Text.Equals("") ||
                        NumberEdit.Text.Equals("") ||
                        cepEdit.Text.Equals("") ||
                        admissionEdit.Text.Equals("") ||
                        RoleEdit.Text.Equals("")
                );
            }
            
        }

        private void ClearFields() {

            //Comboboxes
            cbHasUser.SelectedIndex = 2;
            cbActive.SelectedIndex = 0;
            cbState.SelectedIndex = 0;

            //Campos de texto
            NameEdit.Text = null;
            cpfEdit.Text = null;
            AdressEdit.Text = null;
            NeighboorhoodEdit.Text = null;
            NumberEdit.Text = null;
            cepEdit.Text = null;
            CityEdit.Text = null;
            admissionEdit.Text = null;
            RoleEdit.Text = null;
            usernameEdit.Text = null;
            passwordEdit.Password = null;
        }

        private void EnableFields() {

            //Comboboxes
            cbHasUser.IsEnabled = true;
            cbActive.IsEnabled = true;
            cbState.IsEnabled = true;

            //Campos de texto
            NameEdit.IsEnabled = true;
            cpfEdit.IsEnabled = true;
            AdressEdit.IsEnabled = true;
            NeighboorhoodEdit.IsEnabled = true;
            NumberEdit.IsEnabled = true;
            cepEdit.IsEnabled = true;
            CityEdit.IsEnabled = true;
            admissionEdit.IsEnabled = true;
            RoleEdit.IsEnabled = true;
            usernameEdit.IsEnabled = true;
            passwordEdit.IsEnabled = true;

            //Botões
            btnCancelar.IsEnabled = true;
            btnSalvar.IsEnabled = true;
        }

        private void BlockFields() {

            //Comboboxes
            cbHasUser.IsEnabled = false;
            cbActive.IsEnabled = false;
            cbState.IsEnabled = false;

            //Campos de texto
            NameEdit.IsEnabled = false;
            cpfEdit.IsEnabled = false;
            AdressEdit.IsEnabled = false;
            NeighboorhoodEdit.IsEnabled = false;
            NumberEdit.IsEnabled = false;
            cepEdit.IsEnabled = false;
            CityEdit.IsEnabled = false;
            admissionEdit.IsEnabled = false;
            RoleEdit.IsEnabled = false;
            usernameEdit.IsEnabled = false;
            passwordEdit.IsEnabled = false;

            //Botões
            btnCancelar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
        }

        #endregion 
    }
}
