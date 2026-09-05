
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;
using MinhaApi.Models;
using MinhaApi.Repositories;
using MySqlConnector;

public class ClienteRepository : IClienteRepository
{
    private readonly string _connectionString; 
    public ClienteRepository(IConfiguration config) => _connectionString = config.GetConnectionString("DefaultConnection")!;
    private static List<Cliente> _db = new()
    {
        new Cliente { Id=1, Nome="João", Email="joao@email.com", CPF="12345678900", Ativo=true },
        new Cliente { Id=2, Nome="Maria", Email="maria@email.com", CPF="09876543210", Ativo=true }
    
    };

    public IEnumerable<Cliente> GetAll()
    {
       
        var lista = new List<Cliente>();
        using var conn = new MySqlConnection(_connectionString); conn.Open();
        string sql = "Select id, nome, email, cpf, ativo From clientes";
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while(reader.Read()) {
            lista.Add(new Cliente
            { Id = reader.GetInt32("id"),
            Nome = reader.GetString("nome"),
            Email = reader.GetString("email"),
            CPF = reader.GetString("cpf"),
            Ativo = reader.GetBoolean("ativo")
            });
        }
        
        return lista;
        
    } 

    public Cliente? GetById(int id) => _db.FirstOrDefault(c => c.Id == id);

    public void Add(Cliente c)
    
    {
        using var conn = new MySqlConnection(_connectionString); conn.Open();
        string sql = "Insert Into clientes (nome, email, cpf, ativo) Values (@nome, @email, @cpf, @ativo); SELECT LAST_INSERT_ID();";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@nome", c.Nome);
        cmd.Parameters.AddWithValue("@email", c.Email);
        cmd.Parameters.AddWithValue("@cpf", c.CPF);
        cmd.Parameters.AddWithValue("@ativo", c.Ativo);
       var idGerado = cmd.ExecuteNonQuery();
       c .Id = Convert.ToInt32(idGerado);
       
    }

    public void Update(Cliente c)
    {
        using var conn = new MySqlConnection(_connectionString); conn.Open();
        String sql = "Update clientes Set nome=@nome, email=@email, cpf=@cpf, ativo=@ativo Where id=@id";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", c.Id);
        cmd.Parameters.AddWithValue("@nome", c.Nome);
        cmd.Parameters.AddWithValue("@email", c.Email);
        cmd.Parameters.AddWithValue("@cpf", c.CPF);
        cmd.Parameters.AddWithValue("@ativo", c.Ativo);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id) 
    {
        using var conn = new MySqlConnection(_connectionString); conn.Open();
        String sql = "Delete From clientes Where id=@id";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
    
}
