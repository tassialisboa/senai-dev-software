using MinhaApi.Models;
using MinhaApi.Repositories;
using MinhaApi.Services;

public class VendaService : IVendasService
{
    private readonly IVendasRepository _vendaRepo;
    private readonly IClienteRepository _clienteRepo;
    private readonly IProdutoRepository _produtoRepo;

    public VendaService(
        IVendasRepository vendaRepo,
        IClienteRepository clienteRepo,
        IProdutoRepository produtoRepo)
    {
        _vendaRepo = vendaRepo;
        _clienteRepo = clienteRepo;
        _produtoRepo = produtoRepo;
    }

    public Vendas Create(Vendas venda)
    {

        var cliente = _clienteRepo.GetById(venda.ClienteId);
        if (cliente == null || !cliente.Ativo)
            throw new ArgumentException("Cliente inválido ou inativo!");

 
        var produto = _produtoRepo.GetById(venda.ProdutoId);
        if (produto == null || !produto.Ativo)
            throw new ArgumentException("Produto inválido ou inativo!");

        if (produto.Estoque < venda.Quantidade)
            throw new ArgumentException("Estoque insuficiente!");

        venda.ValorTotal = produto.Preco * venda.Quantidade;
        venda.DataVenda = DateTime.Now;

   
        produto.Estoque -= venda.Quantidade;
        _produtoRepo.Update(produto);

        _vendaRepo.Add(venda);

        return venda;
    }
}