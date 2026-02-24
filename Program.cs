using APIAgroCoreOrquestradora.Configuracao;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Dependencies
builder.Services.ResolveDependencies();

var app = builder.Build();

app.UseHttpMetrics();

app.MapMetrics();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
