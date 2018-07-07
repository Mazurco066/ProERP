using System.Windows;
using System.Windows.Controls;
using Promig.Connection.Methods;
using Promig.Utils;

namespace Promig.View.Components {
    
    public partial class UserControlLogs : UserControl {

        #region Header

        //Definindo objeto de dados
        private Logs dao;

        //Definindo construtor
        public UserControlLogs() {
            InitializeComponent();
            dao = new Logs();
        }

        #endregion

        #region Events

        //Evento ao carregar
        private void control_loaded(object sender, RoutedEventArgs e) {
            datePicker.Text = DateBr.GetDateBr();
        }

        //Evento ao pressionae botão atualizar
        private void refresh(object sender, RoutedEventArgs e) {
            datePicker.Text = DateBr.GetDateBr();
            txtSearch.Text = null;
            RefreshGrid();
        }

        //Evento ao pesquisar ação
        private void key_down(object sender, RoutedEventArgs e) {
            RefreshGrid();
        }

        //Eventoaoalterar data foco
        private void date_changed(object sender, RoutedEventArgs e) {
            RefreshGrid();
        }

        #endregion

        #region Grid-Param

        private void RefreshGrid() {

            //Recuperando data e texto a ser pesquisado
            string date = datePicker.Text;
            string action = txtSearch.Text;

            //Recuperando lista de logs e inserindo na tabela
            dgLogs.ItemsSource = dao.GetAllRegisters(date, action);
        }

        #endregion
    }
}
