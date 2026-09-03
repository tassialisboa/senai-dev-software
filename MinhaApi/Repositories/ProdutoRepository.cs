using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;
using MinhaApi.Models;
using MinhaApi.Repositories;
using MySqlConnector;

public class ProdutoRepository : IProdutoRepository
{
    private readonly string _connectionString; 
    public ProdutoRepository(IConfiguration config) => _connectionString = config.GetConnectionString("DefaultConnection")!;
    private static List<Produto> _db = new()
    {
        new Produto { Id=1, Nome="Notebook", Preco=2500m, Estoque=10 },
        new Produto { Id=2, Nome="Mouse", Preco=89.90m, Estoque=50 }
    
    };

    public IEnumerable<Produto> GetAll()
    {
       
        var lista = new List<Produto>();
        using var conn = new MySqlConnection(_connectionString); conn.Open();
        string sql = "Select id, nome, preco, estoque, ativo From produtos";
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while(reader.Read()) {
            lista.Add(new Produto
            { Id = reader.GetInt32("id"),
            Nome = reader.GetString("nome"),
            Preco = reader.GetDecimal("preco"),
            Estoque = reader.GetInt32("estoque"),
                Ativo = reader.GetBoolean("ativo")
            });
        }
        
        return lista;
        
    } 

    public Produto? GetById(int id) => _db.FirstOrDefault(p => p.Id == id);

    public void Add(Produto p)
    
    {
        using var conn = new MySqlConnection(_connectionString); conn.Open();
        string sql = "Insert Into produtos (nome, preco, estoque, ativo) Values (@nome, @preco, @estoque, @ativo) VALUES (@Nome, @Preco, @Estoque, @Ativo); SELECT LAST_INSERT_ID();";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@nome", p.Nome);
        cmd.Parameters.AddWithValue("@preco", p.Preco);
        cmd.Parameters.AddWithValue("@estoque", p.Estoque);
        cmd.Parameters.AddWithValue("@ativo", p.Ativo);
       var idGerado = cmd.ExecuteNonQuery();
       p.Id = Convert.ToInt32(idGerado);
       
    }

    public void Update(Produto p)
    {
        using var conn = new MySqlConnection(_connectionString); conn.Open();
        String sql = "Update produtos Set nome=@nome, preco=@preco, estoque=@estoque, ativo=@ativo Where id=@id";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", p.Id);
        cmd.Parameters.AddWithValue("@nome", p.Nome);
        cmd.Parameters.AddWithValue("@preco", p.Preco);
        cmd.Parameters.AddWithValue("@estoque", p.Estoque);
        cmd.Parameters.AddWithValue("@ativo", p.Ativo);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id) 
    {
        using var conn = new MySqlConnection(_connectionString); conn.Open();
        String sql = "Delete From produtos Where id=@id";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
    
}

