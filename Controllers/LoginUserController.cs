using APIAgroCoreOrquestradora.Model;
using APIAgroCoreOrquestradora.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIAgroCoreOrquestradora.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginUserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginService _loginService;

        public LoginUserController(IConfiguration configuration, ILoginService loginService)
        {
            _configuration = configuration;
            _loginService = loginService;
        }

        /// <summary>
        /// Endpoint para autenticação de usuários. Recebe email e senha, valida as credenciais e retorna um token JWT se a autenticação for bem-sucedida.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
            {
                return BadRequest(new { message = "Email e Senha são obrigatórios." });
            }

            var isAuthenticated = await _loginService.Authenticate(request);
            if (!isAuthenticated)
            {
                return Unauthorized(new { message = "Credenciais inválidas." });
            }

            // Ler configurações de JWT do appsettings
            var secretKey = _configuration["JwtSettings:SecretKey"] ?? "000";
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, request.Email),
                    new Claim(ClaimTypes.Name, "Demo User")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            if (!string.IsNullOrWhiteSpace(issuer)) tokenDescriptor.Issuer = issuer;
            if (!string.IsNullOrWhiteSpace(audience)) tokenDescriptor.Audience = audience;

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new LoginResponseModel { Token = tokenString, ExpiresAt = tokenDescriptor.Expires });
        }
    }
}
