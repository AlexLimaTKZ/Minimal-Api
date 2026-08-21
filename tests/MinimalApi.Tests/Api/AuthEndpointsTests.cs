using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Domain.Entidades;
using MinimalApi.Domain.Entidades.DTOs;
using MinimalApi.Infrastructure.DB;
using Xunit;

namespace MinimalApi.Tests.Api;

public class AuthEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            TestWebHost.Configure(builder);

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    service => service.ServiceType == typeof(DbContextOptions<DbContexto>));

                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddDbContext<DbContexto>(options =>
                    options.UseInMemoryDatabase("AuthEndpointsTests"));

                using var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DbContexto>();
                db.Database.EnsureCreated();

                if (!db.Administradores.Any())
                {
                    db.Administradores.Add(new Administrador
                    {
                        Email = "test@admin.com",
                        Senha = "password",
                        Perfil = "Admin"
                    });
                    db.SaveChanges();
                }
            });
        });
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkAndToken()
    {
        var client = _factory.CreateClient();
        var loginDto = new LoginDTO
        {
            Email = "test@admin.com",
            Senha = "password"
        };

        var response = await client.PostAsJsonAsync("/login", loginDto);

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();

        Assert.NotNull(body);
        Assert.False(string.IsNullOrWhiteSpace(body.Token));
        Assert.Equal(5, body.ExpiresInMinutes);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();
        var loginDto = new LoginDTO
        {
            Email = "wrong@admin.com",
            Senha = "wrongpassword"
        };

        var response = await client.PostAsJsonAsync("/login", loginDto);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
