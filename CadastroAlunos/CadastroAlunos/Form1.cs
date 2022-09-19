using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Adicionar bibliotecas do SQLite
using System.Data.SQLite;
using System.Data.SqlClient;
//Bibliotecas para texto



namespace CadastroAlunos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Função para retornar uma conexão ao banco de dados
        //Utilizar sempre que desejar executar um comando no banco de dados
        private static SQLiteConnection ConexaoDB()
        {
            //Criar uma conexão ao banco
            SQLiteConnection Conexao;
            Conexao = new SQLiteConnection("Data Source=Alunos.db");
            Conexao.Open();
            return Conexao;
        }
        //Procedimento para atualizar a Grade de dados
        //Utilizar sempre que quiser apresentar a situação atual dos dados
        private void AtualizaGrid()
        {
            //Instrução SQL que será executada
            string SQL = "Select id, nome, sobrenome, endereco, cidade, uf, cep from Alunos Order By nome";
            //SQLiteCommand Comando = new SQLiteCommand(SQL, ConexaoDB());
            try
            {
                //Componente que recebe os dados da consulta no banco 
                // e adpta para enviar a um data table
                SQLiteDataAdapter da = new SQLiteDataAdapter(SQL, ConexaoDB());
                DataTable dt = new DataTable();
                da.Fill(dt);
                //Ligando a grade ao data table já preparado para apresentação
                dgwDados.DataSource = dt;
                //Acertar o títula das colunas (deixar mais correto)
                dgwDados.Columns[0].HeaderText = "Id";
                dgwDados.Columns[1].HeaderText = "Nome";
                dgwDados.Columns[2].HeaderText = "Sobrenome";
                dgwDados.Columns[3].HeaderText = "Endereco";
                dgwDados.Columns[3].HeaderText = "Cidade";
                dgwDados.Columns[3].HeaderText = "UF";
                dgwDados.Columns[3].HeaderText = "CEP";
            }
            catch (SQLiteException ex)
            {
                throw ex;
            }
            finally
            {
                //Importante fechar a conexão para não ter problemas
                ConexaoDB().Close();
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Carregar os dados do banco na grade
            AtualizaGrid();

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
             //Simplesmente limpar os campos
            txtId.Clear();
            txtNome.Clear();
            txtSobrenome.Clear();
            txtEndereco.Clear();
            txtCidade.Clear();
            txtCEP.Clear();
            txtNome.Focus();

        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            //Criar um comando (instrução SQL usando a Conexão existente)
            using (var comando = ConexaoDB().CreateCommand())
            {
                //Instrução SQL para inserir dados
                comando.CommandText = "INSERT INTO Alunos (nome,sobrenome,endereco,cidade,uf,cep ) VALUES (@nome, @sobrenome, @endereco, @cidade, @uf, @cep)";
                //Quando utilizado parâmetros é necessário preparar a execução
                comando.Prepare();
                //Passar valor para cada parâmetro
                comando.Parameters.AddWithValue("@nome", txtNome.Text);
                comando.Parameters.AddWithValue("@sobrenome", txtSobrenome.Text);
                comando.Parameters.AddWithValue("@endereco", txtEndereco.Text);
                comando.Parameters.AddWithValue("@cidade", txtCidade.Text);
                comando.Parameters.AddWithValue("@uf", cbxUF.Text);
                comando.Parameters.AddWithValue("@cep", txtCEP.Text);
                try
                {
                    //Executar comando sem retorno de dados
                    comando.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    throw ex;
                }
                finally
                {

                    //Importante fechar a conexão para não ter problemas
                    ConexaoDB().Close();
                    //Atualizar a grade de dados
                    AtualizaGrid();
                }
            }

        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            //Criar um comando (instrução SQL usando a Conexão existente)
            using (var comando = ConexaoDB().CreateCommand())
            {
                //Instrução SQL para alterar dados
                comando.CommandText = "Update Alunos Set nome=@nome, sobrenome=@sobrenome, endereco=@endereco, cidade=@cidade, uf=@uf, cep=@cep Where id=@id";
                //Quando utilizado parâmetros é necessário preparar a execução
                comando.Prepare();
                //Passar valor para cada parâmetro
                comando.Parameters.AddWithValue("@nome", txtNome.Text);
                comando.Parameters.AddWithValue("@sobrenome", txtSobrenome.Text);
                comando.Parameters.AddWithValue("@endereco", txtEndereco.Text);
                comando.Parameters.AddWithValue("@cidade", txtCidade.Text);
                comando.Parameters.AddWithValue("@uf", cbxUF.Text);
                comando.Parameters.AddWithValue("@cep", txtCEP.Text);
                comando.Parameters.AddWithValue("@id", txtId.Text);
                try
                {
                    //Executar comando sem retorno de dados
                    comando.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    throw ex;
                }
                finally
                {
                    //Importante fechar a conexão para não ter problemas
                    ConexaoDB().Close();
                    //Atualizar a grade de dados
                    AtualizaGrid();
                }
            }

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            //Fazer uma pergunta via caixa de mensagem
            var result = MessageBox.Show("Tem certeza que deseja excluir?",
                                         "Confirmação",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);
            //Se o usuário clicou no botão SIM...
            if (result == DialogResult.Yes)
            {
                //Criar um comando (instrução SQL usando a Conexão existente)
                using (var comando = ConexaoDB().CreateCommand())
                {
                    //Instrução SQL para deletar dados
                    comando.CommandText = "Delete from Alunos Where id=@id";
                    //Quando utilizado parâmetros é necessário preparar a execução
                    comando.Prepare();
                    //Passar valor para cada parâmetro
                    comando.Parameters.AddWithValue("@id", txtId.Text);
                    try
                    {
                        //Executar comando sem retorno de dados
                        comando.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        //Importante fechar a conexão para não ter problemas
                        ConexaoDB().Close();
                        //Atualizar a grade de dados
                        AtualizaGrid();
                    }
                }
            }

        }

        private void dgwDados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Quando navegar na grade copiar os dados para os campos
            txtId.Text = dgwDados.CurrentRow.Cells[0].Value.ToString();
            txtNome.Text = dgwDados.CurrentRow.Cells[1].Value.ToString();
            txtSobrenome.Text = dgwDados.CurrentRow.Cells[2].Value.ToString();
            txtEndereco.Text = dgwDados.CurrentRow.Cells[3].Value.ToString();
            txtCidade.Text = dgwDados.CurrentRow.Cells[4].Value.ToString();
            cbxUF.Text = dgwDados.CurrentRow.Cells[5].Value.ToString();
            txtCEP.Text = dgwDados.CurrentRow.Cells[6].Value.ToString();

        }

        private void cbxUF_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
