using MinhaApi.Models;
using MinhaApi.Repositories;
using MinhaApi.Services;

public class VendaService : IVendaService
{
    private readonly IVendaRepository _vendaRepo;
    private readonly IClienteRepository _clienteRepo;
    private readonly IProdutoRepository _produtoRepo;

    public VendaService(
        IVendaRepository vendaRepo,
        IClienteRepository clienteRepo,
        IProdutoRepository produtoRepo)
    {
        _vendaRepo = vendaRepo;
        _clienteRepo = clienteRepo;
        _produtoRepo = produtoRepo;
    }

    public Venda Create(Venda venda)
    {

        var cliente = _clienteRepo.GetById(venda.clientes_id);
        if (cliente == null || !cliente.Ativo)
            throw new ArgumentException("Cliente inválido ou inativo!");

 
        var produto = _produtoRepo.GetById(venda.produtos_id);
        if (produto == null || !produto.Ativo)
            throw new ArgumentException("Produto inválido ou inativo!");

        if (produto.Estoque < venda.quantidade)
            throw new ArgumentException("Estoque insuficiente!");

        venda.ValorTotal = produto.Preco * venda.quantidade;
        venda.Data_Venda = DateTime.Now;

   
        produto.Estoque -= venda.quantidade;
        _produtoRepo.Update(produto);

        _vendaRepo.Add(venda);

        return venda;
    }
}