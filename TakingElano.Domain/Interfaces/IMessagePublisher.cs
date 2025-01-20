namespace TakingElano.Domain.Interfaces;

public interface IMessagePublisher
{
    void Publish(object message);
}
