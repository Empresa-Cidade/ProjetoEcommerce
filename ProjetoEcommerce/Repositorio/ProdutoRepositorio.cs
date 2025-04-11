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

    }
}