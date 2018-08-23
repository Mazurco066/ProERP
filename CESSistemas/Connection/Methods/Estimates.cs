using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using Promig.Connection;
using Promig.Exceptions;
using Promig.Model;
using Promig.Model.CbModel;

class Estimates {

    #region header

    //Atributos de conexão
    private MySqlConnection conn;

    /// <summary>
    /// Construtor padrão
    /// </summary>
    public Estimates() { conn = ConnectionFactory.GetConnection(); }

    #endregion header

    #region Data-Access

    /// <summary>
    /// Metodo para adicionar orçamento ao banco de dados
    /// </summary>
    /// <param name="estimate">Orçamento a ser inserido no banco</param>
    public void AddEstimate(Estimate estimate) {

        try {

            // Abertura da conexão com o banco de dados
            conn.Open();

            // Definição do comando de inserção
            string command = "CALL AddOrcamento(@id_cliente, @)";

            // Definição do comando instanciado
            MySqlCommand cmd = new MySqlCommand(command, conn) {
                CommandType = CommandType.Text
            };

            // Definição dos valores dos parametros

            // Preparando comando com os parametros
            cmd.Prepare();

            // Executando inserção
            cmd.ExecuteNonQuery();

            // Fechando conexão com banco
            conn.Close();

            // Mensagem de sucesso
            MessageBox.Show(
                "Orçamento Inserido!",
                "Sucesso",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );

        } catch (MySqlException) {

            // Fechando conexão com banco e disparando exceção
            conn.Close();
            throw new DatabaseInsertException();
        }
    }

    /// <summary>
    /// Metodo para alterar orçamento no banco de dados
    /// </summary>
    /// <param name="estimate">Orçamento a ser alterado no banco</param>
    public void EditEstimate(Estimate estimate) {

        try {

            // Abertura da conexão com o banco de dados
            conn.Open();

        } catch (MySqlException) {

            // Fechando conexão com banco de disparando exceção
            conn.Close();
            throw new DatabaseEditException();
        }
    }

    /// <summary>
    /// Metodo para retornar relação de nomes de todos clientes com seus 
    /// respecgivos ID's
    /// </summary>
    /// <returns>Retorna uma lista no modelo Customer com nomes e ids de clientes
    /// presentes no banco de dados</returns>
    public List<Customer> NameCustomerList() {

        try {

            // Abertura da conexão com o banco
            conn.Open();

            // Definindo comando de query
            string command = $"select c.id_cliente, p.nome_pessoa from " +
                             $"{Refs.TABLE_PEOPLE} p, " +
                             $"{Refs.TABLE_CLIENTS} c " +
                             $"where c.id_pessoa = p.id_pessoa and " +
                             $"p.status = @status";

            // Definindo comando e resultados
            MySqlDataReader reader;
            List<Customer> names = new List<Customer>();
            MySqlCommand cmd = new MySqlCommand(command, conn) {
                CommandType = CommandType.Text
            };

            // Definindo parametros de busca
            cmd.Parameters.Add(new MySqlParameter("@status", true));
            cmd.Prepare();

            // Realização da consulta
            reader = cmd.ExecuteReader();

            // Preenchendo lista de nomes
            while (reader.Read()) {
                names.Add(new Customer(
                    (int)reader["id_cliente"],
                    reader["nome_pessoa"].ToString()
                ));
            }

            // Fechamento da conexão
            conn.Close();

            // Retorno da lista de nomes
            return names;

        } catch (MySqlException) {

            // Fechando conexão e retrnando mensagem de erro
            conn.Close();
            MessageBox.Show(
                "Erro ao tentar se conectar ao banco! Tente novamente mais tarde...",
                "Erro de Conexão!",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            return new List<Customer>();
        }
    }

    #endregion
}