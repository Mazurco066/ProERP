using System.Collections.Generic;
using Promig.Model;
using Promig.Exceptions;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using Promig.Connection;
using System;

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

    public void AddEstimate(Estimate estimate) {

        try {

            // Abertura da conexão com banco de dados
            conn.Open();

            // Definindo comando de interção
            string command = $"BEGIN; insert into {Refs.TABLE_ESTIMATES}(id_cliente, data_orcamento, " +
                             $"caminho_imagem, descricao, condicao_pagto, execucao_dias, valor_total, " +
                             $") VALUES (@id_customer, @date, @path, @description, @payment, @execution, " +
                             $"@total_value);";
            command += $"insert into {Refs.TABLE_SERVICES}(no_documento, descricao) VALUES";
            
            // Relacionando serviços com o orçamento
            foreach (string s in estimate.services) {
                command += $"(last_inserted_id(), {s}),";
            }
            command += "COMMIT;";

        } catch (MySqlException) {

            // Fechamento da conexão e lançamento de exceção
            conn.Close();
            throw new DatabaseInsertException();
        }
    }

    public void EditEstimate(Estimate estimate) {

        try {

            // Abertura da conexão com banco de dados
            conn.Open();

        } catch (MySqlException) {

            // Fechamento da conexão e lançamento de exceção
            conn.Close();
            throw new DatabaseInsertException();
        }
    }

    public List<string> NameCustomerList() {

        try {

            // Abertura da conexão com o banco
            conn.Open();

            // Definindo comando de query
            string command = $"select c.id_cliente, p.nome_pessoa from " +
                             $"{Refs.TABLE_PEOPLE} p, " +
                             $"{Refs.TABLE_CLIENTS} c " +
                             $"where p.status = @status";

            // Definindo comando e resultados
            List<string> names = new List<string>();
            MySqlDataReader reader;
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
                names.Add(reader["nome_pessoa"].ToString());
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
            return new List<string>();
        }
    }

    #endregion
}