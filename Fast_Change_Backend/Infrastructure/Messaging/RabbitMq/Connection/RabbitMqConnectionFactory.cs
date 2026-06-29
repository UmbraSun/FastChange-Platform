using Infrastructure.Messaging.RabbitMq.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infrastructure.Messaging.RabbitMq.Connection;

public sealed class RabbitMqConnectionFactory 
    : IAsyncDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private IConnection? _connection;
    private bool _disposed;

    public RabbitMqConnectionFactory(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;
    }

    public async Task<IConnection> GetConnectionAsync()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(RabbitMqConnectionFactory));

        if (_connection is not null && _connection.IsOpen)
            return _connection;

        await _lock.WaitAsync();
        try
        {
            if (_connection is not null && _connection.IsOpen)
                return _connection;

            if (_connection is not null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }

            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,

                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            };

            _connection = await factory.CreateConnectionAsync();
            return _connection;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        if (_connection is not null)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }

        _lock.Dispose();
    }
}
