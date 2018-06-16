using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Promig.Connections;
using Promig.Utils;
using System.Data.Entity.Migrations;

namespace Promig.Models
{
    /// <summary>
    /// Interação lógica para UserControlUsers.xam
    /// </summary>
    public partial class UserControlUsers : UserControl
    {

        USUARIOS usuario = new USUARIOS();
        List<USUARIOS> listaUsuarios = new List<USUARIOS>();
        Connection conexao = new Connection();

        private bool selected;

        public UserControlUsers()
        {
            InitializeComponent();
            carreagaComboEstados();
            carregaDataGrid();
        }

        // carregar a comboBox de estados
        private void carreagaComboEstados(){
            cbEstado.ItemsSource = null;
            cbEstado.ItemsSource = Utils.Util.ListaEstado();
        }

        // limpar campos
        private void limpaCampos(){
            txtUsuario.Clear();
            txtSenha.Clear();
            txtEndereco.Clear();
            txtBairro.Clear();
            txtCidade.Clear();
            cbEstado.Text = string.Empty;
            txtCpf.Clear();
            txtEmail.Clear();
            cbTipo.Text = string.Empty;
            txtCodigo.Clear();
        }

        // atualizar/carregar dataGrid
        private void carregaDataGrid(){
            listaUsuarios = conexao.USUARIOS.ToList();
            dgUsuarios.ItemsSource = null;
            dgUsuarios.ItemsSource = listaUsuarios.OrderBy(x => x.nome_usuario);
        }

        // inserir os dados
        private void insert(){
            usuario.nome_usuario = txtUsuario.Text;
            usuario.senha = Criptografia.Encrypt(txtSenha.Password);
            usuario.endereco = txtEndereco.Text;
            usuario.bairro = txtBairro.Text;
            usuario.cidade = txtCidade.Text;
            usuario.estado = cbEstado.Text;
            usuario.cpf = txtCpf.Text;
            usuario.email = txtEmail.Text;
            usuario.tipo = cbTipo.Text;

            conexao.USUARIOS.Add(usuario);
        }

        // atualizar os dados
        private void update(){
            usuario.nome_usuario = txtUsuario.Text;
            usuario.senha = Criptografia.Encrypt(txtSenha.Password);
            usuario.endereco = txtEndereco.Text;
            usuario.bairro = txtBairro.Text;
            usuario.cidade = txtCidade.Text;
            usuario.estado = cbEstado.Text;
            usuario.cpf = txtCpf.Text;
            usuario.email = txtEmail.Text;
            usuario.tipo = cbTipo.Text;

            conexao.USUARIOS.AddOrUpdate(usuario);
        }

        // botao salvar
        private void btnSlavar_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtCodigo.Text.Equals(string.Empty)) {
                    insert();
                    conexao.SaveChanges();
                    txtCodigo.Text = usuario.codigo.ToString();
                    MessageBox.Show("Dados salvos com sucesso!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else {
                    update();
                    conexao.SaveChanges();
                    selected = false;
                    dgUsuarios.SelectedItem = null;
                    MessageBox.Show("Dados alterados com sucesso!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    btnSalvar.Content = "Salvar";
                }
                limpaCampos();
                carregaDataGrid();
            }catch(Exception ex){
                MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.logException(ex);
                Log.logMessage(ex.Message);
            }
        }

        // botao limpar
        private void btnLimpar_Click(object sender, RoutedEventArgs e) {
            limpaCampos();
            selected = false;
            dgUsuarios.SelectedItem = null;
            btnSalvar.Content = "Salvar";
        }

        // botao excluir
        private void btnExclur_Click(object sender, RoutedEventArgs e) {
            try {
                MessageBoxResult resultado = MessageBox.Show("Tem certeza que quer excluir o registro?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultado.Equals(MessageBoxResult.Yes)) {
                    conexao.USUARIOS.Attach(usuario);
                    conexao.USUARIOS.Remove(usuario);
                    conexao.SaveChanges();
                    int? codigo = conexao.USUARIOS.Max(u => (int?)u.codigo);
                    DataBaseCommand.redefinePK_autoIncremento("USUARIOS",codigo);
                    limpaCampos();
                    carregaDataGrid();
                    MessageBox.Show("Dados excluidos com sucesso!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    btnSalvar.Content = "Salvar";
                }else{
                    limpaCampos();
                    btnSalvar.Content = "Salvar";
                    return;
                }
            }catch(Exception ex){
                MessageBox.Show("Erro ao excluir: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.logException(ex);
                Log.logMessage(ex.Message);
            }
        }

        // pesquisar dados
        private void pesquisar(){
          try{
                if(dgUsuarios.SelectedItem != null){
                    selected = true;
                }
                if(selected){
                    USUARIOS u = new USUARIOS();
                    u = dgUsuarios.SelectedItem as USUARIOS;
                    int codigo = u.codigo;

                    btnSalvar.Content = "Alterar";

                    usuario = conexao.USUARIOS.Find(codigo);
                    txtCodigo.Text = usuario.codigo.ToString();
                    txtUsuario.Text = usuario.nome_usuario;
                    txtSenha.Password = Criptografia.Decrypt(usuario.senha);
                    txtEndereco.Text = usuario.endereco;
                    txtBairro.Text = usuario.bairro;
                    txtCidade.Text = usuario.cidade;
                    cbEstado.Text = usuario.estado;
                    txtCpf.Text = usuario.cpf;
                    txtEmail.Text = usuario.email;
                    cbTipo.Text = usuario.tipo;
                }
          }catch(Exception ex){
                Log.logException(ex);
                Log.logMessage(ex.Message);
          }
        }

        // selecionar dados na grid para excluir/alterar
        private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            pesquisar();
        }

        // corrigido bug que fecha programa ao dar dois cliques na grid
        private void dgUsuarios_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            try {
                return;
            }
            catch (Exception ex) {
                Log.logException(ex);
                Log.logMessage(ex.Message);
            }
        }
    }
}
