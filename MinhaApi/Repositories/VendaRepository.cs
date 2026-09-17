
using MinhaApi.Models;
using MinhaApi.Repositories;
using MySqlConnector;

public class VendaRepository : IVendaRepository{

    //public Venda? GetById(int id) => _db.FirstOrDefault(v => v.Id == id);

    private readonly string _connectionString; 
    public VendaRepository(IConfiguration config) => _connectionString = config.GetConnectionString("DefaultConnection")!;
    public void Add(Venda v)
    
    {
        using var conn = new MySqlConnection(_connectionString); 
        conn.Open();
        string sql = "Insert Into vendas (ValorTotal, Quantidade, Data_Venda, clientes_id, produtos_id) Values (@ValorTotal, @Quantidade, @Data_Venda, @clientes_id, @produtos_id); SELECT LAST_INSERT_ID();";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@valortotal", v.ValorTotal);
        cmd.Parameters.AddWithValue("@quantidade", v.quantidade);
        cmd.Parameters.AddWithValue("@data_venda", v.Data_Venda);
        cmd.Parameters.AddWithValue("@clientes_id", v.clientes_id);
        cmd.Parameters.AddWithValue("@produtos_id", v.produtos_id);
       var idGerado = cmd.ExecuteNonQuery();
       v.Id = Convert.ToInt32(idGerado);
       
    }
    }