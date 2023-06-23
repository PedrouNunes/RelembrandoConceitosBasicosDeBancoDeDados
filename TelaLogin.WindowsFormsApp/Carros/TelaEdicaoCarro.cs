using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace TelaLogin.WindowsFormsApp.Carros
{
    public partial class TelaEdicaoCarro : Form
    {
        private SqlConnection conexao;
        string placa;
        string nomeLogin;
        int novoVendedorId;
        public TelaEdicaoCarro(string placaPesquisada, string nome)
        {
            InitializeComponent();
            this.placa = placaPesquisada;
            nomeLogin = nome;
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            PaginaInicial paginaInicial = new PaginaInicial(nomeLogin);
            paginaInicial.Show();
            this.Hide();
        }

        public int ObterIdVendedorPeloNome(string nomeVendedor)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=Login;Integrated Security=True";
            conexao = new SqlConnection(connectionString);
            conexao.Open();
            string query = "SELECT Id FROM dbo.Dados WHERE Nome = @Nome";
            using (SqlCommand command = new SqlCommand(query, conexao))
            {
                command.Parameters.AddWithValue("@Nome", nomeVendedor);
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                    conexao.Close();
                }
            }

            return 0;
            conexao.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            novoVendedorId = ObterIdVendedorPeloNome(txtVendedor.Text);
            // Consultar o carros no banco de dados
            string connectionString = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=Login;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT * FROM carros WHERE Placa = @Placa";
            SqlCommand command = new SqlCommand(query, conexao);
            command.Parameters.AddWithValue("@Placa", placa);

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
                        string modelo = reader.GetString(1);
                        string placa = reader.GetString(2);
                        int vendedorId = reader.GetInt32(3);

                        Console.WriteLine("ID: {0}, Modelo: {1}, Placa: {2}, VendedorId: {3}", id, modelo, placa, vendedorId);
                    }

                    // Solicitar novos dados para atualizar o usuário

                    string novoModelo = txtModelo.Text;

                    string novaPlaca = txtPlaca.Text;


                    // Atualizar o carro no banco de dados
                    query = "UPDATE carros SET Modelo = @NovoModelo, Placa = @NovaPlaca, Vendedor_Id = @NovoVendedorId WHERE Placa = @Placa";
                    command = new SqlCommand(query, conexao);
                    command.Parameters.AddWithValue("@NovoModelo", novoModelo);
                    command.Parameters.AddWithValue("@NovaPlaca", novaPlaca);
                    command.Parameters.AddWithValue("@NovoVendedorId", novoVendedorId);
                    command.Parameters.AddWithValue("@Placa", placa);


                    reader.Close();
                    int rowsAffected = command.ExecuteNonQuery();

                    MessageBox.Show("\nCarro atualizado com sucesso. Linhas afetadas: {0}");
                    using (StreamWriter writer = new StreamWriter("log.txt", true))
                    {
                        writer.WriteLine($"Carro editado: {placa}, Linhas afetadas: {rowsAffected}, Data e Hora: {DateTime.Now}");
                    }

                }
                else
                {
                    MessageBox.Show("Carro não encontrado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
    }
}
