using Microsoft.EntityFrameworkCore;
using TakingElano.Domain.Entities;

namespace TakingElano.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Venda> Vendas { get; set; }
    public DbSet<Item> Itens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Venda>().HasKey(v => v.Id);
        modelBuilder.Entity<Item>().HasKey(i => i.Id);

        modelBuilder.Entity<Venda>()
            .HasMany(v => v.Itens)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
