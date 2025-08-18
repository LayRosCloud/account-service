using RabbitMQ.Client;

namespace Broker.Handlers;

public interface IConsumer<in T>
{
    Task ConsumeAsync(T entity);
}