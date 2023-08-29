using AutoMapper;
using Domain.DTOs;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Base;
using Domain.Interfaces.Services;
using Domain.Services;
using Entities;
using Infrastructure.Configuration;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Token;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

/**
 * Base de dados
 * Registro da subclasse DbContext chamada BaseContext como um serviço 'Scoped'
 * no container de injeção de dependência do ASP.NET Core.
 * Ver sobre secrets em https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-strings
 */
string connectionString = builder.Configuration.GetConnectionString("Base");
builder.Services.AddDbContext<BaseDbContext>(opts =>
    opts.UseNpgsql(connectionString));

/**
 * Swashbuckle/Swagger
 * AddSecurityDefinition configura e exibe o botão Authorize na documentação Swagger.
 * AddSecurityRequirement injeta um cabeçalho Authorization para cada requisição.
 */
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        BearerFormat = "JWT",
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey,
        Description = $"Cabeçalho de autorização JWT usando esquema Bearer." +
        $"\r\n\r\nExemplo: Bearer 1234567890qwertyuiop",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
            },
            new List<string>()
        }
    });
});

/**
 * AutoMapper.
 * Alternativamente, as configurações poderiam estar em uma pasta própria - Profiles,
 * e aqui haveria apenas a configuração para Assembly.
 * Usage: config.CreateMap<From,To>();
 */
var config = new MapperConfiguration(config =>
{
    config.CreateMap<MessageDTO, Message>();
});
IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

// Identity
builder.Services
    .AddDefaultIdentity<User>()
    .AddEntityFrameworkStores<BaseDbContext>();

// Repositories
builder.Services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddSingleton<IMessageRepository, MessageRepository>();

// Services
builder.Services.AddSingleton<IMessageService, MessageService>();

/**
 * JWT
 */
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = JwtSecurityKey.Create("948IFIF4949FKFK4949FK1")
        };
        opts.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = (context) =>
            {
                Console.WriteLine(
                    $"[INFO] Authentication Failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = (context) =>
            {
                Console.WriteLine(
                $"[INFO] Token Validated: {context.SecurityToken}");
                return Task.CompletedTask;
            }
        };
    });

// Policies

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
