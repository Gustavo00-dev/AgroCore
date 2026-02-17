using APIAgroCoreOrquestradora.Configuracao;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Dependencies
builder.Services.ResolveDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
