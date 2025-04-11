using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Configuration;
using System.Data;

namespace ProjetoEcommerce.Repositorio
{
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public void Cadastrar(Produto produto)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para inserir dados na tabela 'cliente'
                MySqlCommand cmd = new MySqlCommand("insert into cliente (NomeProd,Descricao,Quantidade,Preco) values (@nomeProd, @descricao, @quantidade, @preco)", conexao); // @: PARAMETRO
                                                                                                                                                 // Adiciona um parâmetro para o nome, definindo seu tipo e valor
                cmd.Parameters.Add("@nomeProd", MySqlDbType.VarChar).Value = produto.NomeProd;
                // Adiciona um parâmetro para o telefone, definindo seu tipo e valor
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                // Adiciona um parâmetro para o email, definindo seu tipo e valor
                cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;

                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;
                // Executa o comando SQL de inserção e retorna o número de linhas afetadas
                cmd.ExecuteNonQuery();
                // Fecha explicitamente a conexão com o banco de dados (embora o 'using' já faça isso)
                conexao.Close();
            }
        }
        public bool Atualizar(Produto produto)
        {
            try
            {
                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();
                    // Cria um novo comando SQL para atualizar dados na tabela 'cliente' com base no código
                    MySqlCommand cmd = new MySqlCommand("Update produto set NomeProd=@nomeProd, Descricao=@descricao, Quantidade=@quantidade, Preco=@preco " + " where CodProd=@codigo ", conexao);
                    // Adiciona um parâmetro para o código do cliente a ser atualizado, definindo seu tipo e valor
                    cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = produto.CodProd;
                    // Adiciona um parâmetro para o novo nome, definindo seu tipo e valor
                    cmd.Parameters.Add("@nomeProd", MySqlDbType.VarChar).Value = produto.NomeProd;
                    // Adiciona um parâmetro para o novo telefone, definindo seu tipo e valor
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                    // Adiciona um parâmetro para o novo email, definindo seu tipo e valor
                    cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;
                    // Executa o comando SQL de atualização e retorna o número de linhas afetadas
                    //executa e verifica se a alteração foi realizada
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0; // Retorna true se ao menos uma linha foi atualizada

                }
            }
            catch (MySqlException ex)
            {
                // Logar a exceção (usar um framework de logging como NLog ou Serilog)
                Console.WriteLine($"Erro ao atualizar Produto: {ex.Message}");
                return false; // Retorna false em caso de erro

            }
        }
        public IEnumerable<Produto> TodosClientes()
        {
            // Cria uma nova lista para armazenar os objetos Cliente
            List<Cliente> Clientlist = new List<Cliente>();

            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para selecionar todos os registros da tabela 'cliente'
                MySqlCommand cmd = new MySqlCommand("SELECT * from cliente", conexao);

                // Cria um adaptador de dados para preencher um DataTable com os resultados da consulta
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                // Cria um novo DataTable
                DataTable dt = new DataTable();
                // metodo fill- Preenche o DataTable com os dados retornados pela consulta
                da.Fill(dt);
                // Fecha explicitamente a conexão com o banco de dados 
                conexao.Close();

                // interage sobre cada linha (DataRow) do DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    // Cria um novo objeto Cliente e preenche suas propriedades com os valores da linha atual
                    Clientlist.Add(
                                new Cliente
                                {
                                    CodCli = Convert.ToInt32(dr["CodCli"]), // Converte o valor da coluna "codigo" para inteiro
                                    NomeCli = ((string)dr["NomeCli"]), // Converte o valor da coluna "nome" para string
                                    TelCli = ((string)dr["TelCli"]), // Converte o valor da coluna "telefone" para string
                                    EmailCli = ((string)dr["EmailCli"]), // Converte o valor da coluna "email" para string
                                });
                }
                // Retorna a lista de todos os clientes
                return Clientlist;
            }
        }

    }
}