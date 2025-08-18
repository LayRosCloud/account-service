using Broker.Handlers;
using RabbitMQ.Client;

namespace NotificationService.Extensions;

public static class BrokerListenExtensions
{
    public static async Task ListenAsync<T>(this IServiceProvider provider, IConnection connection, Func<IChannel, Task<IConsumer<T>>> createAsync, CancellationToken token = default)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: token);
        try
        {
            var broker = new BrokerConsumer(channel);
            var consumer = await createAsync(channel);
            await broker.ConsumeAsync(consumer, token);
        }
        catch
        {
            await channel.CloseAsync(cancellationToken: token);
            throw;
        }
    }
}