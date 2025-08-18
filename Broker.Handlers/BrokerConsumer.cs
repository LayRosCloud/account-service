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
            try
            {
                await HandleAsync(consumerObject, ((AsyncEventingBasicConsumer)sender), eventArgs,
                    cancellationToken);
            }
            catch
            {
                await _channel.BasicNackAsync(eventArgs.DeliveryTag, false, false, cancellationToken);
            }
            
        };
        return Task.CompletedTask;
    }

    private static async Task HandleAsync<T>(IConsumer<T> consumerObject, IAsyncBasicConsumer sender, BasicDeliverEventArgs eventArgs, CancellationToken cancellationToken)
    {
        var bodyBytes = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(bodyBytes);
        var entity = JsonSerializer.Deserialize<T>(message);
        if (entity == null)
        {
            return;
        }
        await sender.Channel!.BasicAckAsync(eventArgs.DeliveryTag, false, cancellationToken);
        await consumerObject.ConsumeAsync(entity);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
    }
}