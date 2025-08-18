using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Broker.Handlers;

public class BrokerConsumer : IAsyncDisposable
{
    private readonly IChannel _channel;

    public BrokerConsumer(IChannel channel)
    {
        _channel = channel;
    }

    public Task ConsumeAsync<T>(IConsumer<T> consumerObject, CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var bodyBytes = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(bodyBytes);
            var entity = JsonSerializer.Deserialize<T>(message);
            if (entity == null)
            {
                return;
            }
            await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(eventArgs.DeliveryTag, false, cancellationToken);
            await consumerObject.ConsumeAsync(entity);
        };
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
    }
}