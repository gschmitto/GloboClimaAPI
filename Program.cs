using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using GloboClimaAPI.Data;
using GloboClimaAPI.Infrastructure.Data;
using GloboClimaAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços necessários para o controlador
builder.Services.AddControllers();

// Registrar o DynamoDBClient
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonDynamoDB>();

// Registrar o DynamoDBContext
builder.Services.AddScoped<DynamoDBContext>();

// Registrando repositórios e serviços
builder.Services.AddScoped<IFavoritesService, FavoritesService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserFavoritesRepository, UserFavoritesRepository>();

// Adicionar o DynamoDBInitializer para inicializar tabelas
builder.Services.AddSingleton<DynamoDBInitializer>();

// Configuração para usar autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is not configured.")))
        };
    });

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

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT no formato **Bearer {token}**",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
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

// Ativar autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Inicializar as tabelas DynamoDB
var dynamoDBInitializer = app.Services.GetRequiredService<DynamoDBInitializer>();
await dynamoDBInitializer.InitializeTablesAsync();

// Configura o pipeline de requisições
app.MapControllers(); // Mapeia os controladores da API

// Roda a aplicação
app.Run();
