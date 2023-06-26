using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using TelaLogin.WindowsFormsApp.Carros;

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
            TelaCadastroVendedor telaRequisitos = new TelaCadastroVendedor(nomeLogin);
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
            TelaOpcoesVendedor telaEditar = new TelaOpcoesVendedor(nomeLogin);
            telaEditar.Show();
            this.Hide();

        }

        private void btnCadastrarCarros_Click(object sender, EventArgs e)
        {
            TelaCadastroCarro telacadastroCarros = new TelaCadastroCarro(nomeLogin);
            telacadastroCarros.Show();
            this.Hide();
        }

        private void btnOpcoes_Click(object sender, EventArgs e)
        {
            TelaOpcoesCarro telaOpcoesCarros = new TelaOpcoesCarro(nomeLogin);
            telaOpcoesCarros.Show();
            this.Hide();
        }
    }
}


    
   

