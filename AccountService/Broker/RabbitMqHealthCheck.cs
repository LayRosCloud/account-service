using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace AccountService.Broker;

public class RabbitMqHealthCheck : IHealthCheck
{
    private readonly IConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqHealthCheck(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (context.Registration.Name == "liveness")
            {
                return _connection?.IsOpen == true
                    ? HealthCheckResult.Healthy("RabbitMQ connection is alive")
                    : HealthCheckResult.Unhealthy("RabbitMQ connection is not alive");
            }

            // ReSharper disable once InvertIf
            if (context.Registration.Name == "readiness")
            {
                if (_connection is not { IsOpen: true })
                {
                    _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken: cancellationToken);
                }

                _channel ??= await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

                return HealthCheckResult.Healthy("RabbitMQ is ready to accept messages");
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("RabbitMQ health check failed", ex);
        }
    }
}