using FluentAssertions;
using NSubstitute;
using TakingElano.Application.DTOs;
using TakingElano.Application.Services;
using TakingElano.Domain.Entities;
using TakingElano.Domain.Interfaces;
using Bogus;
using Moq;
using Xunit;

namespace TakingElano.UnitTests.Services;

public class VendaServiceTests
{
    private readonly IVendaRepository _vendaRepository;
    private readonly VendaService _vendaService;
    private readonly Mock<IVendaRepository> _vendaRepositoryMock;
    private readonly Mock<IMessagePublisher> _messagePublisherMock;

    public VendaServiceTests()
    {
        // _vendaRepository = Substitute.For<IVendaRepository>();
        // _vendaService = new VendaService(_vendaRepository);

        _vendaRepositoryMock = new Mock<IVendaRepository>();
        _messagePublisherMock = new Mock<IMessagePublisher>();
        _vendaService = new VendaService(_vendaRepositoryMock.Object, _messagePublisherMock.Object);
    }

    [Fact]
    public async Task CriarVendaAsync_DeveSalvarVendaCorretamente()
    {
        // Arrange
        var vendaDto = new VendaDto
        {
            Data = DateTime.UtcNow,
            Itens = new List<ItemDto>
            {
                new ItemDto { Nome = "Produto A", Quantidade = 2, PrecoUnitario = 10 }
            },
            Total = 20
        };

        // Mock do comportamento do repositório
        _vendaRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Venda>()))
            .Returns(Task.CompletedTask);

        // Configuração do mock do publisher
        _messagePublisherMock
            .Setup(m => m.Publish(It.IsAny<string>()))
            .Verifiable();
        await _vendaService.CriarVendaAsync(vendaDto);

        // Assert
        _vendaRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Venda>()), Times.Once);
        _messagePublisherMock.Verify(m => m.Publish(It.Is<string>(s => s.Contains("Produto A"))), Times.Once);

    }
}
