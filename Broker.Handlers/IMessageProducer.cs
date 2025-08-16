namespace Broker.Handlers;

public interface IMessageProducer
{
    Task SendAsync<T>(T message, string topic, string exchange = "");
}