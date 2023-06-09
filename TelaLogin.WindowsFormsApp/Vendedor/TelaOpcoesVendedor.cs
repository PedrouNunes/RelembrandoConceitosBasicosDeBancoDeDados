﻿
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace TelaLogin.WindowsFormsApp
{
    public partial class TelaOpcoesVendedor : Form
    {
        string nome;
        public TelaOpcoesVendedor(string nomeLogin)
        {
            InitializeComponent();
            nome = nomeLogin;
        }

        private void btnEditar_Click(object sender, System.EventArgs e)
        {
            string nome = txtNovoNome.Text;

            TelaEdicaoVendedor telaEdicaoFinal = new TelaEdicaoVendedor(nome);
            telaEdicaoFinal.Show();
            this.Hide();
        }

        private void btnExcluir_Click(object sender, System.EventArgs e)
        {
            // Configurar a conexão com o banco de dados
            string connectionString = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=Login;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);

            // Solicitar o nome do usuário a ser pesquisado
            string nomePesquisa = txtNovoNome.Text;

            // Consultar o usuário no banco de dados
            string query = "SELECT * FROM dados WHERE Nome = @Nome";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Nome", nomePesquisa);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    // Mostrar os detalhes do usuário encontrado
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string nome = reader.GetString(1);
                        string senha = reader.GetString(2);

                        Console.WriteLine("ID: {0}, Nome: {1}, Senha: {2}", id, nome, senha);
                    }

                    reader.Close();

                 // Excluir o usuário do banco de dados
                        query = "DELETE FROM dados WHERE Nome = @Nome";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Nome", nomePesquisa);

                        int rowsAffected = command.ExecuteNonQuery();

                        MessageBox.Show("\nUsuário excluído com sucesso. Linhas afetadas: {0}");

                    using (StreamWriter writer = new StreamWriter("log.txt", true))
                    {
                        writer.WriteLine($"Usuário excluído: {nomePesquisa}, Linhas afetadas: {rowsAffected}, Data e Hora: {DateTime.Now}");
                    }

                }
                else
                {
                    MessageBox.Show("Usuário não encontrado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            PaginaInicial paginaInicial = new PaginaInicial(nome);
            paginaInicial.Show();
            this.Hide();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            string nomePesquisado = txtNovoNome.Text;

            string connectionString = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=Login;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT * FROM dados WHERE Nome = @Nome";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Nome", nomePesquisado);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    // Mostrar os detalhes do carro encontrado
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string nome = reader.GetString(1);
                        string senha = reader.GetString(2);

                        Console.WriteLine("ID: {0}, Nome: {1}, Senha: {2}", id, nome, senha);

                        MessageBox.Show("ID: " + id + "\n" +
                        "Nome: " + nome + "\n" +
                        "Senha: " + senha + "\n");
                    }
                }
                else
                {
                    MessageBox.Show("Vendedor não encontrado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
