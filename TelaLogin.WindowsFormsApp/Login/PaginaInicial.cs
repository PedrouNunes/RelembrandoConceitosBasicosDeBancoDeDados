using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelaLogin.WindowsFormsApp
{
    public partial class PaginaInicial : Form
    {
        string nomeLogin;
        public PaginaInicial(string nome)
        {
            InitializeComponent();
            nomeLogin = nome;

        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            TelaCadastro telaRequisitos = new TelaCadastro();
            telaRequisitos.Show();
            this.Hide();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=Login;Integrated Security=True";

            string username = nomeLogin;

            string query = $"ALTER DATABASE LOGIN SET SINGLE_USER WITH ROLLBACK IMMEDIATE; ALTER DATABASE LOGIN SET MULTI_USER;";

            try
            {
                using (SqlConnection conexao = new SqlConnection(connectionString))
                {
                    conexao.Open();

                    using (SqlCommand command = new SqlCommand(query, conexao))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Usuário desconectado");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao desconectar o usuário: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Configurar a conexão com o banco de dados
            string connectionString = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=Login;Integrated Security=True";

            SqlConnection connection = new SqlConnection(connectionString);

            // Solicitar o nome do usuário a ser pesquisado
            Console.Write("Digite o nome do usuário a ser pesquisado: ");
            string nomePesquisa = Console.ReadLine();

            // Consultar o usuário no banco de dados
            string query = "SELECT * FROM Usuarios WHERE Nome = @Nome";
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
                        string email = reader.GetString(2);

                        Console.WriteLine("ID: {0}, Nome: {1}, Email: {2}", id, nome, email);
                    }

                    // Solicitar novos dados para atualizar o usuário
                    Console.WriteLine("\nDigite os novos dados do usuário:");

                    Console.Write("Novo nome: ");
                    string novoNome = Console.ReadLine();

                    Console.Write("Novo email: ");
                    string novoEmail = Console.ReadLine();

                    // Atualizar o usuário no banco de dados
                    query = "UPDATE Usuarios SET Nome = @NovoNome, Email = @NovoEmail WHERE Nome = @Nome";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NovoNome", novoNome);
                    command.Parameters.AddWithValue("@NovoEmail", novoEmail);
                    command.Parameters.AddWithValue("@Nome", nomePesquisa);

                    reader.Close();
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine("\nUsuário atualizado com sucesso. Linhas afetadas: {0}", rowsAffected);
                }
                else
                {
                    Console.WriteLine("Usuário não encontrado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            Console.WriteLine("\nPressione qualquer tecla para sair.");
            Console.ReadKey();
        }
    }
}


    
   

