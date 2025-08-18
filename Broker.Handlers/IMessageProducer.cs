namespace Broker.Handlers;

public interface IMessageProducer
{
    Task SendAsync<T>(T message, string queue, string routingKey, string exchange = "");
}