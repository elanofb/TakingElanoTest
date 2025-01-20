using TakingElano.Application.DTOs;

namespace TakingElano.Application.Interfaces;

public interface IVendaService
{
    Task<VendaDto?> ObterVendaPorIdAsync(int id);
    Task<IEnumerable<VendaDto>> ObterTodasVendasAsync();
    Task CriarVendaAsync(VendaDto vendaDto);
    Task AtualizarVendaAsync(VendaDto vendaDto);
    Task ExcluirVendaAsync(int id);
}
