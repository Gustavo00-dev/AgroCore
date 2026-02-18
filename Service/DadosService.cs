using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APIAgroCoreOrquestradora.Model;
using Microsoft.Extensions.Configuration;

namespace APIAgroCoreOrquestradora.Service
{
    public interface IDadosService
    {
        Task PublishPropriedadeAsync(ProriedadeRequestModel request, string correlationId);
    }

    public class DadosService : IDadosService
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly string _queueName = "queue.propriedade.command";

        public DadosService(IRabbitMqService rabbitMqService, IConfiguration configuration)
        {
            _rabbitMqService = rabbitMqService;
        }

        public async Task PublishPropriedadeAsync(ProriedadeRequestModel request, string correlationId)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var envelope = new
            {
                command = "create",
                timestamp = DateTime.UtcNow.ToString("o"),
                correlationId = correlationId,
                data = new
                {
                    nome = request.Nome,
                    area = request.Area
                }
            };

            var message = JsonSerializer.Serialize(envelope);
            var body = Encoding.UTF8.GetBytes(message);

            await _rabbitMqService.PublishAsync(_queueName, body, correlationId, persistent: true);
        }
    }
}
