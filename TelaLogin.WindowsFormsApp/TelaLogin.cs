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
    public partial class TelaLogin : Form
    {
        public TelaLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=Login;Integrated Security=True";

            SqlConnection conexao = new SqlConnection(connectionString);

            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;

            try
            {
                conexao.Open();

                string query = "SELECT COUNT(*) FROM dbo.dados WHERE nome = @nome AND senha = @senha";
                SqlCommand comando = new SqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@nome", usuario);
                comando.Parameters.AddWithValue("@senha", senha);

                int resultado = Convert.ToInt32(comando.ExecuteScalar());

                if (resultado == 1)
                {
                    TelaRequisitos telaRequisitos = new TelaRequisitos();
                    telaRequisitos.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuário ou senha errados");
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

        private void button1_Click(object sender, EventArgs e)
        {
            TelaRequisitos telaRequisitos = new TelaRequisitos();
            telaRequisitos.Show();
            this.Hide();
        }
    }
    }

