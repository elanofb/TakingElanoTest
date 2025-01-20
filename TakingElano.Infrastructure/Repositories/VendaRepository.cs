using TakingElano.Domain.Entities;
using TakingElano.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TakingElano.Infrastructure.Data;

namespace TakingElano.Infrastructure.Repositories;

public class VendaRepository : IVendaRepository
{
    private readonly ApplicationDbContext _context;

    public VendaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Venda?> GetByIdAsync(int id)
    {
        return await _context.Vendas.Include(v => v.Itens).FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<Venda>> GetAllAsync()
    {
        return await _context.Vendas.Include(v => v.Itens).ToListAsync();
    }

    public async Task AddAsync(Venda venda)
    {
        await _context.Vendas.AddAsync(venda);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Venda venda)
    {
        _context.Vendas.Update(venda);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var venda = await _context.Vendas.FindAsync(id);
        if (venda != null)
        {
            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();
        }
    }
}
