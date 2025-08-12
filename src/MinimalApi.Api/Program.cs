using MinimalApi.Infrastructure.DB;
using MinimalApi.Domain.Entidades.DTOs;
using MinimalApi.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MinimalApi.Application.Services;
using MinimalApi.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EditorPolicy", policy => policy.RequireRole("Editor", "Admin"));
});

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal API v1");
        options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Olá Mundo!");

app.MapPost("/login", async (LoginDTO loginDTO, IAdministradorServico administradorServico, IConfiguration configuration) =>
{
    // IMPORTANT SECURITY NOTE: In a real-world application, passwords MUST be hashed and salted.
    // This example uses plain text passwords for simplicity, but this is NOT secure for production.
    var admin = await administradorServico.GetAdministradorByEmailAndSenha(loginDTO.Email, loginDTO.Senha);

    if (admin == null)
    {
        return Results.Unauthorized();
    }

    var issuer = configuration["Jwt:Issuer"];
    var audience = configuration["Jwt:Audience"];
    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

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
        Issuer = issuer,
        Audience = audience,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var jwtToken = tokenHandler.WriteToken(token);

    return Results.Ok(jwtToken);
});

// Endpoints for Veiculo
app.MapPost("/veiculos", async (Veiculo veiculo, DbContexto db) =>
{
    var validationContext = new ValidationContext(veiculo, serviceProvider: null, items: null);
    var validationResults = new List<ValidationResult>();

    if (!Validator.TryValidateObject(veiculo, validationContext, validationResults, validateAllProperties: true))
    {
        return Results.ValidationProblem(validationResults.ToDictionary(vr => vr.MemberNames.First(), vr => new string[] { vr.ErrorMessage ?? "Invalid" }));
    }

    db.Veiculos.Add(veiculo);
    await db.SaveChangesAsync();
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
}).RequireAuthorization("AdminPolicy");

app.MapGet("/veiculos", async (DbContexto db) =>
{
    return Results.Ok(await db.Veiculos.ToListAsync());
}).RequireAuthorization("EditorPolicy");

app.MapGet("/veiculos/{id}", async (int id, DbContexto db) =>
{
    return await db.Veiculos.FindAsync(id)
        is Veiculo veiculo
            ? Results.Ok(veiculo)
            : Results.NotFound();
}).RequireAuthorization("EditorPolicy");

app.MapPut("/veiculos/{id}", async (int id, Veiculo inputVeiculo, DbContexto db) =>
{
    var validationContext = new ValidationContext(inputVeiculo, serviceProvider: null, items: null);
    var validationResults = new List<ValidationResult>();

    if (!Validator.TryValidateObject(inputVeiculo, validationContext, validationResults, validateAllProperties: true))
    {
        return Results.ValidationProblem(validationResults.ToDictionary(vr => vr.MemberNames.First(), vr => new string[] { vr.ErrorMessage ?? "Invalid" }));
    }

    var veiculo = await db.Veiculos.FindAsync(id);

    if (veiculo is null) return Results.NotFound();

    veiculo.Marca = inputVeiculo.Marca;
    veiculo.Modelo = inputVeiculo.Modelo;
    veiculo.Ano = inputVeiculo.Ano;
    veiculo.Cor = inputVeiculo.Cor;

    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization("AdminPolicy");

app.MapDelete("/veiculos/{id}", async (int id, DbContexto db) =>
{
    if (await db.Veiculos.FindAsync(id) is Veiculo veiculo)
    {
        db.Veiculos.Remove(veiculo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
}).RequireAuthorization("AdminPolicy");

app.Run();