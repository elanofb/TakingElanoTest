using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using TakingElano.API;
using Testcontainers.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TakingElano.Application.Services;
using TakingElano.Infrastructure.Data;
using TakingElano.Application.DTOs;
using RabbitMQ.Client;

namespace TakingElano.IntegrationTests;

public class VendaIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly RabbitMqContainer _rabbitMqContainer;
    private readonly CustomWebApplicationFactory _factory;

    public VendaIntegrationTests()
    {
        // Configuração do RabbitMQ Test Container
        _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            //.WithUser("guest", "guest")
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();        

        _rabbitMqContainer.StartAsync().Wait();            

        // Configuração do WebApplicationFactory para rodar o API
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        // Iniciar RabbitMQ antes dos testes
        await _rabbitMqContainer.StartAsync();

        // Configurar o contexto de banco de dados (SQLite In-Memory)
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        // Parar RabbitMQ após os testes
        await _rabbitMqContainer.StopAsync();
    }

    public VendaIntegrationTests(CustomWebApplicationFactory factory)
    {

        
        // Criar cliente HTTP a partir do CustomWebApplicationFactory
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostVenda_DeveRetornarStatus200_QuandoDadosValidos()
    {
        // Arrange
        var vendaDto = new
        {
            Data = DateTime.UtcNow,
            Itens = new[]
            {
                new { Nome = "Produto A", Quantidade = 2, PrecoUnitario = 50.0m },
                new { Nome = "Produto B", Quantidade = 1, PrecoUnitario = 100.0m }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/vendas", vendaDto);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro na API: {response.StatusCode}, Conteúdo: {errorContent}");
        }

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Deve_Criar_Venda_Enviar_Para_RabbitMQ()
    {
        // Arrange
        var vendaDto = new
        {
            Data = DateTime.UtcNow,
            Itens = new[]
            {
                new ItemDto { Nome = "Produto A", Quantidade = 5, PrecoUnitario = 10.0m, Desconto = 0 },
                new ItemDto { Nome = "Produto B", Quantidade = 2, PrecoUnitario = 20.0m, Desconto = 5.0m }
            },
            Total = 90.0m
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/vendas", vendaDto);

        // Assert
        response.EnsureSuccessStatusCode();

        // Verificar se a mensagem foi publicada no RabbitMQ
        //var channel = _rabbitMqContainer.CreateConnection().CreateModel();
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqContainer.Hostname,
            Port = _rabbitMqContainer.GetMappedPublicPort(5672),
            UserName = "guest",
            Password = "guest"
        };

        //ConnectionFactory
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "vendas_queue", durable: true, exclusive: false, autoDelete: false);
        var result = channel.BasicGet("vendas_queue", true);

        result.Should().NotBeNull();
        var message = System.Text.Encoding.UTF8.GetString(result.Body.ToArray());
        message.Should().Contain("Produto A");
    }
}
