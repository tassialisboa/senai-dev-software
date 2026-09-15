
using MinhaApi.Models;
using MinhaApi.Repositories;
using MySqlConnector;

public class VendaRepository : IVendasRepository{

    public Venda? GetById(int id) => _db.FirstOrDefault(v => v.Id == id);

    public void Add(Venda v)
    
    {
        using var conn = new MySqlConnection(_connectionString); conn.Open();
        string sql = "Insert Into vendas (ValorTotal, Quantidade, Data_Venda, ClienteId, ProdutoId) Values (@ValorTotal, @Quantidade, @Data_Venda, @ClienteId, @ProdutoId); SELECT LAST_INSERT_ID();";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@valortotal", v.valortotal);
        cmd.Parameters.AddWithValue("@quantidade", v.quantidade);
        cmd.Parameters.AddWithValue("@data_venda", v.data_venda);
        cmd.Parameters.AddWithValue("@clienteid", v.clienteid);
        cmd.Parameters.AddWithValue("@produtoid", v.produtoid);
       var idGerado = cmd.ExecuteNonQuery();
       v.Id = Convert.ToInt32(idGerado);
       
    }
    }