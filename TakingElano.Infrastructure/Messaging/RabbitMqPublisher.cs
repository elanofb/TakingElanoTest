using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TakingElano.Domain.Interfaces;

namespace TakingElano.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly RabbitMqConfiguration _config;

    public RabbitMqPublisher(RabbitMqConfiguration config)
    {
        _config = config;
    }

    public void Publish(object message)
    {
        var factory = new ConnectionFactory { HostName = _config.Hostname };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: _config.ExchangeName, type: ExchangeType.Direct);
        channel.QueueDeclare(queue: _config.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: _config.QueueName, exchange: _config.ExchangeName, routingKey: _config.RoutingKey);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        channel.BasicPublish(
            exchange: _config.ExchangeName, 
            routingKey: _config.RoutingKey, 
            basicProperties: null, 
            body: body
        );
    }
}
