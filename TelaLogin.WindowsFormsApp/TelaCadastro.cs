using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TelaLogin.WindowsFormsApp
{
    public partial class TelaCadastro : Form
    {
        public TelaCadastro()
        {
            InitializeComponent();
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
    }
}
