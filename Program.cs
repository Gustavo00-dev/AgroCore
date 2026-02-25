using APIAgroCoreOrquestradora.Configuracao;
using Prometheus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "000";
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = !string.IsNullOrWhiteSpace(issuer),
        ValidIssuer = issuer,
        ValidateAudience = !string.IsNullOrWhiteSpace(audience),
        ValidAudience = audience,
        ValidateLifetime = true
    };
});

//Dependencies
builder.Services.ResolveDependencies();

var app = builder.Build();

app.UseHttpMetrics();

app.MapMetrics();

app.UseHttpsRedirection();

// Enable authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
