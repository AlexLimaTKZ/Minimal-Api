using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using MinimalApi.Domain.Entidades.DTOs;
using MinimalApi.Domain.Entidades;
using MinimalApi.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalApi.Tests.Api
{
    public class AuthEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AuthEndpointsTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<DbContexto>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add an in-memory database for testing
                    services.AddDbContext<DbContexto>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });

                    // Seed the database with a test admin
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<DbContexto>();
                        db.Database.EnsureCreated();

                        if (!db.Administradores.Any())
                        {
                            db.Administradores.Add(new Administrador { Email = "test@admin.com", Senha = "password", Perfil = "Admin" });
                            db.SaveChanges();
                        }
                    }
                });
            });
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkAndToken()
        {
            // Arrange
            var client = _factory.CreateClient();
            var loginDto = new LoginDTO { Email = "test@admin.com", Senha = "password" };

            // Act
            var response = await client.PostAsJsonAsync("/login", loginDto);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var token = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var client = _factory.CreateClient();
            var loginDto = new LoginDTO { Email = "wrong@admin.com", Senha = "wrongpassword" };

            // Act
            var response = await client.PostAsJsonAsync("/login", loginDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}