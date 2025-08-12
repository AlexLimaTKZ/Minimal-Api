using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using MinimalApi.Domain.Entidades;
using MinimalApi.Domain.Entidades.DTOs;
using MinimalApi.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalApi.Tests.Api
{
    public class VeiculoEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private string _adminToken = string.Empty;
        private string _editorToken = string.Empty;

        public VeiculoEndpointsTests(WebApplicationFactory<Program> factory)
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
                        options.UseInMemoryDatabase("TestDbForVeiculos");
                    });

                    // Seed the database with test admins
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<DbContexto>();
                        db.Database.EnsureCreated();

                        if (!db.Administradores.Any())
                        {
                            db.Administradores.Add(new Administrador { Email = "admin@test.com", Senha = "password", Perfil = "Admin" });
                            db.Administradores.Add(new Administrador { Email = "editor@test.com", Senha = "password", Perfil = "Editor" });
                            db.SaveChanges();
                        }
                    }
                });
            });
        }

        private async Task<string> GetToken(string email, string password)
        {
            var client = _factory.CreateClient();
            var loginDto = new LoginDTO { Email = email, Senha = password };
            var response = await client.PostAsJsonAsync("/login", loginDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private async Task EnsureTokensAreAvailable()
        {
            if (string.IsNullOrEmpty(_adminToken))
            {
                _adminToken = await GetToken("admin@test.com", "password");
            }
            if (string.IsNullOrEmpty(_editorToken))
            {
                _editorToken = await GetToken("editor@test.com", "password");
            }
        }

        [Fact]
        public async Task PostVeiculo_Admin_ReturnsCreated()
        {
            // Arrange
            await EnsureTokensAreAvailable();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
            var veiculo = new Veiculo { Marca = "Ford", Modelo = "Focus", Ano = 2020, Cor = "Preto" };

            // Act
            var response = await client.PostAsJsonAsync("/veiculos", veiculo);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdVeiculo = await response.Content.ReadFromJsonAsync<Veiculo>();
            Assert.NotNull(createdVeiculo);
            Assert.True(createdVeiculo.Id > 0);
        }

        [Fact]
        public async Task PostVeiculo_Editor_ReturnsForbidden()
        {
            // Arrange
            await EnsureTokensAreAvailable();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _editorToken);
            var veiculo = new Veiculo { Marca = "Ford", Modelo = "Focus", Ano = 2020, Cor = "Preto" };

            // Act
            var response = await client.PostAsJsonAsync("/veiculos", veiculo);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetVeiculos_Editor_ReturnsOkAndVeiculos()
        {
            // Arrange
            await EnsureTokensAreAvailable();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _editorToken);

            // Act
            var response = await client.GetAsync("/veiculos");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var veiculos = await response.Content.ReadFromJsonAsync<List<Veiculo>>();
            Assert.NotNull(veiculos);
        }

        [Fact]
        public async Task GetVeiculoById_Editor_ReturnsOkAndVeiculo()
        {
            // Arrange
            await EnsureTokensAreAvailable();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
            var veiculo = new Veiculo { Marca = "VW", Modelo = "Gol", Ano = 2022, Cor = "Branco" };
            var postResponse = await client.PostAsJsonAsync("/veiculos", veiculo);
            postResponse.EnsureSuccessStatusCode();
            var createdVeiculo = await postResponse.Content.ReadFromJsonAsync<Veiculo>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _editorToken);

            // Act
            var getResponse = await client.GetAsync($"/veiculos/{createdVeiculo.Id}");

            // Assert
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var retrievedVeiculo = await getResponse.Content.ReadFromJsonAsync<Veiculo>();
            Assert.NotNull(retrievedVeiculo);
            Assert.Equal(createdVeiculo.Id, retrievedVeiculo.Id);
        }

        [Fact]
        public async Task PutVeiculo_Admin_ReturnsNoContent()
        {
            // Arrange
            await EnsureTokensAreAvailable();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
            var veiculo = new Veiculo { Marca = "Fiat", Modelo = "Uno", Ano = 2015, Cor = "Vermelho" };
            var postResponse = await client.PostAsJsonAsync("/veiculos", veiculo);
            postResponse.EnsureSuccessStatusCode();
            var createdVeiculo = await postResponse.Content.ReadFromJsonAsync<Veiculo>();

            createdVeiculo.Cor = "Azul";

            // Act
            var putResponse = await client.PutAsJsonAsync($"/veiculos/{createdVeiculo.Id}", createdVeiculo);

            // Assert
            putResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
        }

        [Fact]
        public async Task PutVeiculo_Editor_ReturnsForbidden()
        {
            // Arrange
            await EnsureTokensAreAvailable();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
            var veiculo = new Veiculo { Marca = "Chevrolet", Modelo = "Onix", Ano = 2023, Cor = "Cinza" };
            var postResponse = await client.PostAsJsonAsync("/veiculos", veiculo);
            postResponse.EnsureSuccessStatusCode();
            var createdVeiculo = await postResponse.Content.ReadFromJsonAsync<Veiculo>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _editorToken);
            createdVeiculo.Cor = "Preto";

            // Act
            var putResponse = await client.PutAsJsonAsync($"/veiculos/{createdVeiculo.Id}", createdVeiculo);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, putResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteVeiculo_Admin_ReturnsNoContent()
        {
            // Arrange
            await EnsureTokensAreAvailable();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
            var veiculo = new Veiculo { Marca = "Hyundai", Modelo = "HB20", Ano = 2021, Cor = "Prata" };
            var postResponse = await client.PostAsJsonAsync("/veiculos", veiculo);
            postResponse.EnsureSuccessStatusCode();
            var createdVeiculo = await postResponse.Content.ReadFromJsonAsync<Veiculo>();

            // Act
            var deleteResponse = await client.DeleteAsync($"/veiculos/{createdVeiculo.Id}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteVeiculo_Editor_ReturnsForbidden()
        {
            // Arrange
            await EnsureTokensAreAvailable();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
            var veiculo = new Veiculo { Marca = "Renault", Modelo = "Kwid", Ano = 2019, Cor = "Laranja" };
            var postResponse = await client.PostAsJsonAsync("/veiculos", veiculo);
            postResponse.EnsureSuccessStatusCode();
            var createdVeiculo = await postResponse.Content.ReadFromJsonAsync<Veiculo>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _editorToken);

            // Act
            var deleteResponse = await client.DeleteAsync($"/veiculos/{createdVeiculo.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, deleteResponse.StatusCode);
        }
    }
}