using System.Windows;
using System.Windows.Controls;
using Promig.Connection.Methods;
using Promig.Utils;

namespace Promig.View.Components {
    
    public partial class UserControlLogs : UserControl {

        #region Header

        //Definindo objeto de dados
        private Logs dao;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public UserControlLogs() {
            InitializeComponent();
            dao = new Logs();
        }

        #endregion

        #region Events

        /// <summary>
        /// Evento ao carregar controle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void control_loaded(object sender, RoutedEventArgs e) {
            datePicker.Text = DateBr.GetDateBr();
        }

        /// <summary>
        /// Evento ao clicar no botão de voltar a parametros padrão
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh(object sender, RoutedEventArgs e) {
            datePicker.Text = DateBr.GetDateBr();
            txtSearch.Text = null;
            RefreshGrid();
        }

        /// <summary>
        /// Evento ao digitar no campo de pesquisa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void key_down(object sender, RoutedEventArgs e) {
            RefreshGrid();
        }

        /// <summary>
        /// Evento ao alterar data de foco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void date_changed(object sender, RoutedEventArgs e) {
            RefreshGrid();
        }

        #endregion

        #region Grid-Param

        /// <summary>
        /// Método para atualizar conteúdo da grid view
        /// </summary>
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
