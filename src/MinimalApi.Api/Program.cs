using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.Application.Services;
using MinimalApi.Domain.Entidades;
using MinimalApi.Domain.Entidades.DTOs;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Infrastructure.DB;
using MinimalApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("mysql");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configure 'ConnectionStrings:mysql' com User Secrets ou variável de ambiente. Consulte aulas/10-mysql-e-migrations.md.");
}

var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "Configure 'Jwt:Key' com User Secrets ou variável de ambiente. Nunca versione uma chave real.");
}

var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "MinimalApi";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "MinimalApi";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin", "Adm"));
    options.AddPolicy("EditorPolicy", policy => policy.RequireRole("Editor", "Admin", "Adm"));
});

builder.Services.AddScoped<IAdministradorRepositorio, AdministradorRepositorio>();
builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 0)));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Ok(new
{
    mensagem = "Minimal API em execução",
    documentacao = "/swagger"
}));

app.MapPost("/login", async (
    LoginDTO loginDTO,
    IAdministradorServico administradorServico) =>
{
    // ATENÇÃO: o projeto original usa comparação de senha em texto puro
    // para manter o exemplo pequeno. Veja SECURITY.md antes de usar em produção.
    var admin = await administradorServico.GetAdministradorByEmailAndSenha(
        loginDTO.Email,
        loginDTO.Senha);

    if (admin is null)
        return Results.Unauthorized();

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim("Id", admin.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, admin.Email),
            new Claim(JwtRegisteredClaimNames.Email, admin.Email),
            new Claim(ClaimTypes.Role, admin.Perfil),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }),
        Expires = DateTime.UtcNow.AddMinutes(5),
        Issuer = jwtIssuer,
        Audience = jwtAudience,
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            SecurityAlgorithms.HmacSha512Signature)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

    return Results.Ok(new
    {
        token = tokenHandler.WriteToken(token),
        expiresInMinutes = 5
    });
});

app.MapPost("/veiculos", async (Veiculo veiculo, DbContexto db) =>
{
    var validationContext = new ValidationContext(veiculo);
    var validationResults = new List<ValidationResult>();

    if (!Validator.TryValidateObject(
            veiculo,
            validationContext,
            validationResults,
            validateAllProperties: true))
    {
        return Results.ValidationProblem(CriarErrosDeValidacao(validationResults));
    }

    db.Veiculos.Add(veiculo);
    await db.SaveChangesAsync();

    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
}).RequireAuthorization("AdminPolicy");

app.MapGet("/veiculos", async (DbContexto db) =>
    Results.Ok(await db.Veiculos.ToListAsync()))
    .RequireAuthorization("EditorPolicy");

app.MapGet("/veiculos/{id:int}", async (int id, DbContexto db) =>
{
    var veiculo = await db.Veiculos.FindAsync(id);
    return veiculo is null ? Results.NotFound() : Results.Ok(veiculo);
}).RequireAuthorization("EditorPolicy");

app.MapPut("/veiculos/{id:int}", async (int id, Veiculo inputVeiculo, DbContexto db) =>
{
    var veiculo = await db.Veiculos.FindAsync(id);
    if (veiculo is null)
        return Results.NotFound();

    var validationContext = new ValidationContext(inputVeiculo);
    var validationResults = new List<ValidationResult>();

    if (!Validator.TryValidateObject(
            inputVeiculo,
            validationContext,
            validationResults,
            validateAllProperties: true))
    {
        return Results.ValidationProblem(CriarErrosDeValidacao(validationResults));
    }

    veiculo.Marca = inputVeiculo.Marca;
    veiculo.Modelo = inputVeiculo.Modelo;
    veiculo.Ano = inputVeiculo.Ano;
    veiculo.Cor = inputVeiculo.Cor;

    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization("AdminPolicy");

app.MapDelete("/veiculos/{id:int}", async (int id, DbContexto db) =>
{
    var veiculo = await db.Veiculos.FindAsync(id);
    if (veiculo is null)
        return Results.NotFound();

    db.Veiculos.Remove(veiculo);
    await db.SaveChangesAsync();

    return Results.NoContent();
}).RequireAuthorization("AdminPolicy");

app.Run();

static Dictionary<string, string[]> CriarErrosDeValidacao(
    IEnumerable<ValidationResult> validationResults)
{
    return validationResults
        .SelectMany(result => result.MemberNames.DefaultIfEmpty("geral")
            .Select(member => new
            {
                Member = member,
                Message = result.ErrorMessage ?? "Valor inválido"
            }))
        .GroupBy(error => error.Member)
        .ToDictionary(
            group => group.Key,
            group => group.Select(error => error.Message).ToArray());
}

public partial class Program;
