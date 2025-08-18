using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Broker.Handlers;

public class RabbitMqProducer : IMessageProducer
{
    private readonly IConnectionBroker _connection;
    private readonly Dictionary<string, object> _args = new()
    {
        { "x-dead-letter-exchange", "dlx" },
        { "x-dead-letter-routing-key", "errors" }
    };

    public RabbitMqProducer(IConnectionBroker connection)
    {
        _connection = connection;
    }

    public async Task SendAsync<T>(T message, string queue, string routingKey, string exchange = "")
    {
        await using var channel = await _connection.Connection!.CreateChannelAsync();
        await channel.QueueDeclareAsync(queue, false, false, false, arguments: _args!);
        var json = JsonSerializer.Serialize(message);
        var bytes = Encoding.UTF8.GetBytes(json);
        var body = new ReadOnlyMemory<byte>(bytes);
        var basicProperties = new BasicProperties();
        await channel.BasicPublishAsync(
            exchange,
            routingKey,
            false,
            basicProperties,
            body);
    }
}