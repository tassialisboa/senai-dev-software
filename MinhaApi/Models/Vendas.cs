namespace MinhaApi.Models;

public class Vendas
{
    public int Id { get; set; }

    public decimal ValorTotal { get; set; }

    public DateTime data_venda { get; set; }

    public int  quantidade { get; set; }

    public int ClienteId  { get; set; }

    public int ProdutoId  { get; set; }
}
  