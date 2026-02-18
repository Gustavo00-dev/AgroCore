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
            if (request is null || string.IsNullOrWhiteSpace(request.Nome))
                return BadRequest(new { message = "Nome da propriedade é obrigatório." });

            var correlationId = Guid.NewGuid().ToString();

            await _dadosService.PublishCreatePropriedadeAsync(request, correlationId);

            return Ok(new { message = "Propriedade publicada na fila com sucesso.", correlationId });
        }

        [HttpGet("GetPropriedades")]
        public IActionResult GetPropriedades()
        {
            


        }

    }
}
