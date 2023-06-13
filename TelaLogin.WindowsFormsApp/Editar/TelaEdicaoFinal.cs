
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace TelaLogin.WindowsFormsApp
{
    public partial class TelaEdicaoFinal : Form
    {
        string nomePesquisa;
        public TelaEdicaoFinal(string nome)
        {
            InitializeComponent();
            nomePesquisa= nome;
        }

        private void btnConfirmar_Click(object sender, System.EventArgs e)
        {
            // Configurar a conexão com o banco de dados
            string connectionString = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=Login;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);


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

                    // Solicitar novos dados para atualizar o usuário

                    string novoNome = txtNome.Text;

                    string novaSenha = txtSenha.Text;

                    // Atualizar o usuário no banco de dados
                    query = "UPDATE dados SET Nome = @NovoNome, SENHA = @NovaSenha WHERE Nome = @Nome";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NovoNome", novoNome);
                    command.Parameters.AddWithValue("@NovaSenha", novaSenha);
                    command.Parameters.AddWithValue("@Nome", nomePesquisa);

                    reader.Close();
                    int rowsAffected = command.ExecuteNonQuery();

                    MessageBox.Show("\nUsuário atualizado com sucesso. Linhas afetadas: {0}");
                    using (StreamWriter writer = new StreamWriter("log.txt", true))
                    {
                        writer.WriteLine($"Usuário editado: {nomePesquisa}, Linhas afetadas: {rowsAffected}, Data e Hora: {DateTime.Now}");
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
    }
}
