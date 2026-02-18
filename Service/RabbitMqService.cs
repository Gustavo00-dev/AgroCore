using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace APIAgroCoreOrquestradora.Service
{
    public interface IRabbitMqService : IAsyncDisposable
    {
        Task PublishAsync(string queueName, byte[] body, string correlationId = null, bool persistent = true);
    }

    public class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IChannel _channel;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public RabbitMqService(IConfiguration configuration)
        {
            var host = configuration["RabbitMQ:Host"] ?? "host.docker.internal";
            var user = configuration["RabbitMQ:User"] ?? "guest";
            var pass = configuration["RabbitMQ:Password"] ?? "guest";
            var port = int.TryParse(configuration["RabbitMQ:Port"], out var p) ? p : 5672;

            _factory = new ConnectionFactory()
            {
                HostName = host,
                UserName = user,
                Password = pass,
                Port = port
            };
        }

        private async Task EnsureConnectedAsync()
        {
            if (_connection != null && _connection.IsOpen &&
                _channel != null && _channel.IsOpen)
                return;

            await _lock.WaitAsync();

            try
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    _connection = await _factory.CreateConnectionAsync();
                }

                if (_channel == null || !_channel.IsOpen)
                {
                    _channel = await _connection.CreateChannelAsync();
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task PublishAsync(
            string queueName,
            byte[] body,
            string correlationId = null,
            bool persistent = true)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentNullException(nameof(queueName));

            if (body == null)
                throw new ArgumentNullException(nameof(body));

            await EnsureConnectedAsync();

            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var props = new BasicProperties
            {
                Persistent = persistent,
                CorrelationId = correlationId
            };

            await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                basicProperties: props,
                body: body);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
                await _channel.DisposeAsync();

            if (_connection != null)
                await _connection.DisposeAsync();

            _lock.Dispose();
        }
    }
}
