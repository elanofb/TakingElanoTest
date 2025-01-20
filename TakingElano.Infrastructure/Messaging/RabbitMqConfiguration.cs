namespace TakingElano.Infrastructure.Messaging;

public class RabbitMqConfiguration
{
    public string Hostname { get; set; } = "localhost";
    public string QueueName { get; set; } = "vendas";
    public string ExchangeName { get; set; } = "vendas.exchange";
    public string RoutingKey { get; set; } = "venda.criada";
}
