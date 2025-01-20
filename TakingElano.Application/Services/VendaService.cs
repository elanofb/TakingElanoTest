using TakingElano.Application.DTOs;
using TakingElano.Application.Interfaces;
using TakingElano.Domain.Entities;
using TakingElano.Domain.Interfaces;

namespace TakingElano.Application.Services;

public class VendaService : IVendaService
{
    private readonly IVendaRepository _vendaRepository;
    private readonly IMessagePublisher _messagePublisher;

     public VendaService(IVendaRepository vendaRepository)
    {
        _vendaRepository = vendaRepository;
    }

     public VendaService(IVendaRepository vendaRepository, IMessagePublisher messagePublisher)
    {
        _vendaRepository = vendaRepository;
        _messagePublisher = messagePublisher;
    }

    public async Task<VendaDto> ObterVendaPorIdAsync(int id)
    {
        var venda = await _vendaRepository.GetByIdAsync(id);
        return venda != null ? MapearParaDto(venda) : null;
    }

    public async Task<IEnumerable<VendaDto>> ObterTodasVendasAsync()
    {
        var vendas = await _vendaRepository.GetAllAsync();
        return vendas.Select(MapearParaDto);
    }

    public async Task CriarVendaAsync(VendaDto vendaDto)
    {
        var venda = MapearParaEntidade(vendaDto);
        venda.AplicarRegras();
        venda.Total = venda.Itens.Sum(i => (i.PrecoUnitario * i.Quantidade) - i.Desconto);
        await _vendaRepository.AddAsync(venda);

        // Publicar evento de venda criada
        _messagePublisher.Publish(new { venda.Id, venda.Data, Total = venda.Total });
    }

    public async Task AtualizarVendaAsync(VendaDto vendaDto)
    {
        var venda = MapearParaEntidade(vendaDto);
        venda.AplicarRegras();
        await _vendaRepository.UpdateAsync(venda);
    }

    public async Task ExcluirVendaAsync(int id)
    {
        await _vendaRepository.DeleteAsync(id);
    }

    private VendaDto MapearParaDto(Venda venda)
    {
        return new VendaDto
        {
            Id = venda.Id,
            Data = venda.Data,
            Total = venda.Total,
            Itens = venda.Itens.Select(item => new ItemDto
            {
                Id = item.Id,
                Nome = item.Nome,
                Quantidade = item.Quantidade,
                PrecoUnitario = item.PrecoUnitario,
                Desconto = item.Desconto,
                TotalComDesconto = item.TotalComDesconto
            }).ToList()
        };
    }

    private Venda MapearParaEntidade(VendaDto vendaDto)
    {
        return new Venda
        {
            Id = vendaDto.Id,
            Data = vendaDto.Data,
            Itens = vendaDto.Itens.Select(itemDto => new Item
            {
                Id = itemDto.Id,
                Nome = itemDto.Nome,
                Quantidade = itemDto.Quantidade,
                PrecoUnitario = itemDto.PrecoUnitario
            }).ToList()
        };
    }
}