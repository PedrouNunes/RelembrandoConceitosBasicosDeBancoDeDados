using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace TelaLogin.WindowsFormsApp.Carros
{
    public partial class TelaCadastroCarro : Form
    {
        private SqlConnection conexao; // Adiciona uma propriedade para a conexão

        string nomeLogin;
        public TelaCadastroCarro(string nome)
        {
            InitializeComponent();
            this.nomeLogin = nome;
            string connectionString = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=Login;Integrated Security=True";
            conexao = new SqlConnection(connectionString);
        }

        public int ObterIdVendedorPeloNome(string nomeVendedor)
        {
            string query = "SELECT Id FROM dbo.Dados WHERE Nome = @Nome";
            using (SqlCommand command = new SqlCommand(query, conexao))
            {
                command.Parameters.AddWithValue("@Nome", nomeVendedor);
                conexao.Open();
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

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            string modelo = txtModelo.Text;
            string placa = txtPlaca.Text;

            int vendedorId = ObterIdVendedorPeloNome(txtVendedor.Text);


            try
            {
                string insertQuery = "INSERT INTO carros (modelo, placa, vendedor_Id) VALUES (@Modelo, @Placa, @VendedorId)";

                using (SqlCommand command = new SqlCommand(insertQuery, conexao))
                {
                    command.Parameters.AddWithValue("@Modelo", modelo);
                    command.Parameters.AddWithValue("@Placa", placa);
                    command.Parameters.AddWithValue("@VendedorId", vendedorId);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Carro Adicionado");
                using (StreamWriter writer = new StreamWriter("log.txt", true))
                {
                    writer.WriteLine($"Carro Adicionado: {modelo}, Data e Hora: {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message);
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
