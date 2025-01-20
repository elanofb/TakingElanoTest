using TakingElano.Application.Interfaces;
using TakingElano.Application.Services;
using TakingElano.Domain.Interfaces;
using TakingElano.Infrastructure.Data;
using TakingElano.Infrastructure.Messaging;
using TakingElano.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TakingElano.API;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configurar banco de dados
        // Configurar banco de dados
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));        
          
        // Log.Logger = new LoggerConfiguration()
        //     .ReadFrom.Configuration(_configuration) // Lê do appsettings.json
        //     .WriteTo.Console() // Escreve no console
        //     .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Grava logs em arquivos diários
        //     .CreateLogger();

        // builder.Host.UseSerilog(); // Configura o Serilog como o provedor de logs
        
        // Registrar serviços
        services.AddScoped<IVendaRepository, VendaRepository>();
        services.AddScoped<IVendaService, VendaService>();
        services .AddScoped<IMessagePublisher, RabbitMqPublisher>();

        // Configurar RabbitMQ
        var rabbitMqConfig = new RabbitMqConfiguration();
        _configuration.GetSection("RabbitMq").Bind(rabbitMqConfig);
        services.AddSingleton(rabbitMqConfig);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
