namespace MinhaApi.Models;

public class Venda
{
    public int Id { get; set; }

    public decimal ValorTotal { get; set; }

    public DateTime Data_Venda { get; set; }

    public int  quantidade { get; set; }

    public int clientes_id  { get; set; }

    public int produtos_id  { get; set; }
}
  