using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using GloboClimaAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços necessários para o controlador
builder.Services.AddControllers();

builder.Services.AddScoped<IFavoritesService, FavoritesService>();

builder.Services.AddHttpClient();

// Adiciona o Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "GloboClima API",
        Version = "v1",
        Description = "Uma API para clima e informações de cidades"
    });
});

var app = builder.Build();

// Ativa o middleware para Swagger e Swagger UI
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GloboClima API V1");
        c.RoutePrefix = string.Empty; // Define a página inicial como o Swagger UI
    });
}

// Configura o pipeline de requisições
app.MapControllers(); // Mapeia os controladores da API

// Roda a aplicação
app.Run();
