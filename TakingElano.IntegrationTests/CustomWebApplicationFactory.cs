using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TakingElano.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TakingElano.API;

namespace TakingElano.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
{
        protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove o DbContext existente
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Banco de dados InMemory
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryTestDb"));

            // Limpar e inicializar o banco 
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }
}
