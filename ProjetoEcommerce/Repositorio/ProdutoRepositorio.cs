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
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("insert into produto (NomeProd,Descricao,Quantidade,Preco) values (@nomeProd, @descricao, @quantidade, @preco)", conexao);                                                                                                                                           // Adiciona um parâmetro para o nome, definindo seu tipo e valor
                cmd.Parameters.Add("@nomeProd", MySqlDbType.VarChar).Value = produto.NomeProd;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public bool Atualizar(Produto produto)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("Update produto set NomeProd=@nomeProd, Descricao=@descricao, Quantidade=@quantidade, Preco=@preco " + " where CodProd=@codigo ", conexao);
                    cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = produto.CodProd;
                    cmd.Parameters.Add("@nomeProd", MySqlDbType.VarChar).Value = produto.NomeProd;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                    cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0; 

                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar Produto: {ex.Message}");
                return false;
            }
        }
        public IEnumerable<Produto> TodosProdutos()
        {
            List<Produto> Productlist = new List<Produto>();        
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {          
                conexao.Open();  
                MySqlCommand cmd = new MySqlCommand("SELECT * from produto", conexao);    
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);               
                DataTable dt = new DataTable();               
                da.Fill(dt);
                conexao.Close();             
                foreach (DataRow dr in dt.Rows)
                {                  
                    Productlist.Add(
                                new Produto
                                {
                                    CodProd = Convert.ToInt32(dr["CodProd"]), 
                                    NomeProd = ((string)dr["NomeProd"]),
                                    Descricao = ((string)dr["Descricao"]), 
                                    Quantidade = Convert.ToInt32(dr["Quantidade"]),
                                    Preco = ((decimal)dr["Preco"])
                                });
                }
                return Productlist;
            }
        }
        public Produto ObterProduto(int Codigo)
        {          
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {              
                conexao.Open();               
                MySqlCommand cmd = new MySqlCommand("SELECT * from produto where CodProd=@codigo ", conexao);              
                cmd.Parameters.AddWithValue("@codigo", Codigo);              
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);               
                MySqlDataReader dr;            
                Produto produto = new Produto();             
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);              
                while (dr.Read())
                {
                   
                    produto.CodProd = Convert.ToInt32(dr["CodProd"]);
                    produto.NomeProd = (string)(dr["NomeProd"]); 
                    produto.Descricao = (string)(dr["Descricao"]); 
                    produto.Quantidade = Convert.ToInt32(dr["Quantidade"]);
                    produto.Preco = (decimal)(dr["Preco"]);
                }  
             return produto;
            }
        }
        public void ExcluirProduto(int Id)
        {    
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {               
                conexao.Open();           
                MySqlCommand cmd = new MySqlCommand("delete from produto where CodProd=@codigo", conexao);              
                cmd.Parameters.AddWithValue("@codigo", Id);              
                int i = cmd.ExecuteNonQuery();
                conexao.Close(); 
            }
        }

    }
}