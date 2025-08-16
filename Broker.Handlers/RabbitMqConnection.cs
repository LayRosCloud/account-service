using RabbitMQ.Client;

namespace Broker.Handlers;

public class RabbitMqConnection : IConnectionBroker
{
    public RabbitMqConnection()
    {
        Initialize().Wait();
    }

    public IConnection? Connection { get; private set; }

    private async Task Initialize()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };
        Connection = await factory.CreateConnectionAsync();
    }

    public void Dispose()
    {
        Connection?.Dispose();
    }
}