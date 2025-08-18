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
            // ReSharper disable once StringLiteralTypo
            HostName = "rabbitmq",
            UserName = "guest",
            Password = "guest",
            Port = 5672
        };
        Connection = await factory.CreateConnectionAsync();
    }

    public void Dispose()
    {
        Connection?.Dispose();
    }
}