using TakingElano.Domain.Entities;

namespace TakingElano.Domain.Interfaces;

public interface IVendaRepository
{
    Task<Venda?> GetByIdAsync(int id);
    Task<IEnumerable<Venda>> GetAllAsync();
    Task AddAsync(Venda venda);
    Task UpdateAsync(Venda venda);
    Task DeleteAsync(int id);
}
