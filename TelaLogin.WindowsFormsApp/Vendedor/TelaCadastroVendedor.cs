﻿using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace TelaLogin.WindowsFormsApp
{
    public partial class TelaCadastroVendedor : Form
    {
        string nomeLogin;
        public TelaCadastroVendedor(string nome)
        {
            InitializeComponent();
            this.nomeLogin = nome;
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {

            string usuario = txtNome.Text;
            string senha = txtSenha.Text;

            string connectionString = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=Login;Integrated Security=True";

            SqlConnection conexao = new SqlConnection(connectionString);

            try
            {
                conexao.Open();
                string insertQuery = "INSERT INTO dados (nome, senha) VALUES (@Nome, @Senha)";

                using (SqlCommand command = new SqlCommand(insertQuery, conexao))
                 {
                    command.Parameters.AddWithValue("@Nome", usuario);
                    command.Parameters.AddWithValue("@Senha", senha);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Registro Adicionado");
                using (StreamWriter writer = new StreamWriter("log.txt", true))
                {
                    writer.WriteLine($"Usuário Adicionado: {usuario}, Data e Hora: {DateTime.Now}");
                }

            }
            catch (Exception ex)
            {   
                Console.WriteLine("Erro ao conectar ao banco de dados: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }
           
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            PaginaInicial paginaInicial = new PaginaInicial(nomeLogin);
            paginaInicial.Show();
            this.Hide();
        }
    }
}
