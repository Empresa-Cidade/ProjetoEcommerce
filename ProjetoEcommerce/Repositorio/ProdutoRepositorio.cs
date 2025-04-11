using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Configuration;

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
        public bool Atualizar(Cliente cliente)
        {
            try
            {
                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();
                    // Cria um novo comando SQL para atualizar dados na tabela 'cliente' com base no código
                    MySqlCommand cmd = new MySqlCommand("Update cliente set NomeCli=@nome, TelCli=@telefone, EmailCli=@email " + " where CodCli=@codigo ", conexao);
                    // Adiciona um parâmetro para o código do cliente a ser atualizado, definindo seu tipo e valor
                    cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = cliente.CodCli;
                    // Adiciona um parâmetro para o novo nome, definindo seu tipo e valor
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = cliente.NomeCli;
                    // Adiciona um parâmetro para o novo telefone, definindo seu tipo e valor
                    cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = cliente.TelCli;
                    // Adiciona um parâmetro para o novo email, definindo seu tipo e valor
                    cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = cliente.EmailCli;
                    // Executa o comando SQL de atualização e retorna o número de linhas afetadas
                    //executa e verifica se a alteração foi realizada
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0; // Retorna true se ao menos uma linha foi atualizada

                }
            }
            catch (MySqlException ex)
            {
                // Logar a exceção (usar um framework de logging como NLog ou Serilog)
                Console.WriteLine($"Erro ao atualizar cliente: {ex.Message}");
                return false; // Retorna false em caso de erro

            }
        }

    }
}