using APIAgroCoreOrquestradora.Model;
using APIAgroCoreOrquestradora.Service;
using Microsoft.AspNetCore.Mvc;

namespace APIAgroCoreOrquestradora.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DadosController : ControllerBase
    {
        private readonly IDadosService _dadosService;

        public DadosController(IDadosService dadosService)
        {
            _dadosService = dadosService;
        }

        [HttpPost("CadastrarPropriedade")]
        public async Task<IActionResult> CadastrarPropriedade([FromBody] ProriedadeRequestModel request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Nome) || request.IdUsers <= 0)
                return BadRequest(new { message = "Nome da propriedade e ID do Proprietario são obrigatórios." });

            var correlationId = Guid.NewGuid().ToString();

            await _dadosService.PublishCreatePropriedadeAsync(request, correlationId);

            return Ok(new { message = "Propriedade publicada na fila com sucesso.", correlationId });
        }

        [HttpPost("CadastrarTalhao")]
        public async Task<IActionResult> CadastrarTalhao([FromBody] TalhaoRequestModel request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Nome) || request.PropriedadeId <= 0)
                return BadRequest(new { message = "Nome do talhão e ID da propriedade são obrigatórios." });

            var correlationId = Guid.NewGuid().ToString();

            await _dadosService.PublishCreateTalhaoAsync(request, correlationId);

            return Ok(new { message = "Talhão publicado na fila com sucesso.", correlationId });
        }
    }
}
