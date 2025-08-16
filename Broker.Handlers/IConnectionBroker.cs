using RabbitMQ.Client;

namespace Broker.Handlers;

public interface IConnectionBroker : IDisposable
{
    IConnection? Connection { get; }
}