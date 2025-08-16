using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Broker.Handlers;

public class RabbitMqProducer : IMessageProducer
{
    private readonly IConnectionBroker _connection;

    public RabbitMqProducer(IConnectionBroker connection)
    {
        _connection = connection;
    }

    public async Task SendAsync<T>(T message, string topic, string exchange = "")
    {
        await using var channel = await _connection.Connection!.CreateChannelAsync()!;
        await channel.QueueDeclareAsync(topic, false, false, false);
        var json = JsonSerializer.Serialize(message);
        var bytes = Encoding.UTF8.GetBytes(json);
        var body = new ReadOnlyMemory<byte>(bytes);
        var basicProperties = new BasicProperties();
        await channel.BasicPublishAsync(
            exchange,
            topic,
            false,
            basicProperties,
            body);
    }
}