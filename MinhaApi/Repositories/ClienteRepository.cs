using MinhaApi.Models;
using MinhaApi.Repositories;
using MySqlConnector;

public class ClienteRepository : IClienteRepository
{
    private readonly string _connectionString;

    public ClienteRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    public IEnumerable<Cliente> GetAll()
    {
        var lista = new List<Cliente>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = "SELECT id, nome, email, cpf, ativo FROM clientes";

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Cliente
            {
                Id = reader.GetInt32("id"),
                Nome = reader.GetString("nome"),
                Email = reader.GetString("email"),
                CPF = reader.GetString("cpf"),
                Ativo = reader.GetBoolean("ativo")
            });
        }

        return lista;
    }

    public Cliente? GetById(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = "SELECT id, nome, email, cpf, ativo FROM clientes WHERE id=@id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Cliente
            {
                Id = reader.GetInt32("id"),
                Nome = reader.GetString("nome"),
                Email = reader.GetString("email"),
                CPF = reader.GetString("cpf"),
                Ativo = reader.GetBoolean("ativo")
            };
        }

        return null;
    }

    public void Add(Cliente c)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"INSERT INTO clientes (nome, email, cpf, ativo)
                       VALUES (@nome, @email, @cpf, @ativo);
                       SELECT LAST_INSERT_ID();";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@nome", c.Nome);
        cmd.Parameters.AddWithValue("@email", c.Email);
        cmd.Parameters.AddWithValue("@cpf", c.CPF);
        cmd.Parameters.AddWithValue("@ativo", c.Ativo);

        var idGerado = cmd.ExecuteScalar();
        c.Id = Convert.ToInt32(idGerado);
    }

    public void Update(Cliente c)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"UPDATE clientes
                       SET nome=@nome,
                           email=@email,
                           cpf=@cpf,
                           ativo=@ativo
                       WHERE id=@id";

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
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = "DELETE FROM clientes WHERE id=@id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
    }
}