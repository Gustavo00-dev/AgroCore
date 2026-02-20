using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APIAgroCoreOrquestradora.Model;

namespace APIAgroCoreOrquestradora.Service
{
    public interface IDadosService
    {
        Task PublishCreatePropriedadeAsync(ProriedadeRequestModel request, string correlationId);
        Task PublishCreateTalhaoAsync(TalhaoRequestModel request, string correlationId);
    }

    public class DadosService : IDadosService
    {
        private readonly IRabbitMqService _rabbitMqService;

        public DadosService(IRabbitMqService rabbitMqService, IConfiguration configuration)
        {
            _rabbitMqService = rabbitMqService;
        }

        public async Task PublishCreatePropriedadeAsync(ProriedadeRequestModel request, string correlationId)
        {
            string queueName = "queue.propriedade.command";

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var envelope = new
            {
                command = "create",
                timestamp = DateTime.UtcNow.ToString("o"),
                correlationId = correlationId,
                data = new
                {
                    idUsers = request.IdUsers,
                    nome = request.Nome,
                    area = request.Area
                }
            };

            var message = JsonSerializer.Serialize(envelope);
            var body = Encoding.UTF8.GetBytes(message);

            await _rabbitMqService.PublishAsync(queueName, body, correlationId, persistent: true);
        }

        public async Task PublishCreateTalhaoAsync(TalhaoRequestModel request, string correlationId)
        {
            string queueName = "queue.talhao.command";

            if (request is null)
                throw new ArgumentNullException(nameof(request));
            var envelope = new
            {
                command = "create",
                timestamp = DateTime.UtcNow.ToString("o"),
                correlationId = correlationId,
                data = new
                {
                    propriedadeId = request.PropriedadeId,
                    nome = request.Nome,
                    area = request.Area,
                    descricao = request.Descricao
                }
            };
            var message = JsonSerializer.Serialize(envelope);
            var body = Encoding.UTF8.GetBytes(message);
            await _rabbitMqService.PublishAsync(queueName, body, correlationId, persistent: true);
        }
    }
}
