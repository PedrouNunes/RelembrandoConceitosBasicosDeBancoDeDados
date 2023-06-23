using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelaLogin.WindowsFormsApp.Carros
{
    public partial class TelaOpcoesCarro : Form
    {
        public TelaOpcoesCarro()
        {
            InitializeComponent();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            string placa = txtPlaca.Text;

            TelaEdicaoCarro telaEdicaoCarro = new TelaEdicaoCarro(placa);
            telaEdicaoCarro.Show();
            this.Hide();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // Configurar a conexão com o banco de dados
            string connectionString = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=Login;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);

            // Solicitar o nome do usuário a ser pesquisado
            string placaPesquisa = txtPlaca.Text;

            // Consultar o usuário no banco de dados
            string query = "SELECT * FROM carros WHERE Placa = @Placa";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Placa", placaPesquisa);

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

                    reader.Close();

                    // Excluir o usuário do banco de dados
                    query = "DELETE FROM carros WHERE Placa = @Placa";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Placa", placaPesquisa);

                    int rowsAffected = command.ExecuteNonQuery();

                    MessageBox.Show("\nCarro excluído com sucesso. Linhas afetadas: {0}");

                    using (StreamWriter writer = new StreamWriter("log.txt", true))
                    {
                        writer.WriteLine($"Carro excluído: {placaPesquisa}, Linhas afetadas: {rowsAffected}, Data e Hora: {DateTime.Now}");
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
                connection.Close();
            }
        }
    }
}
