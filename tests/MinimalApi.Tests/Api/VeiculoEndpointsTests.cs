using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Domain.Entidades;
using MinimalApi.Domain.Entidades.DTOs;
using MinimalApi.Infrastructure.DB;
using Xunit;

namespace MinimalApi.Tests.Api;

public class VeiculoEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private string _adminToken = string.Empty;
    private string _editorToken = string.Empty;

    public VeiculoEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
            TestWebHost.Configure(builder, $"VeiculoEndpointsTests-{Guid.NewGuid()}"));

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DbContexto>();
        db.Database.EnsureCreated();

        if (!db.Administradores.Any(a => a.Email == "admin@test.com"))
        {
            db.Administradores.AddRange(
                new Administrador
                {
                    Email = "admin@test.com",
                    Senha = "password",
                    Perfil = "Admin"
                },
                new Administrador
                {
                    Email = "editor@test.com",
                    Senha = "password",
                    Perfil = "Editor"
                });
            db.SaveChanges();
        }
    }

    private async Task<string> GetToken(string email, string password)
    {
        var client = _factory.CreateClient();
        var loginDto = new LoginDTO { Email = email, Senha = password };
        var response = await client.PostAsJsonAsync("/login", loginDto);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(body);
        return body.Token;
    }

    private async Task EnsureTokensAreAvailable()
    {
        if (string.IsNullOrEmpty(_adminToken))
            _adminToken = await GetToken("admin@test.com", "password");

        if (string.IsNullOrEmpty(_editorToken))
            _editorToken = await GetToken("editor@test.com", "password");
    }

    [Fact]
    public async Task PostVeiculo_Admin_ReturnsCreated()
    {
        await EnsureTokensAreAvailable();
        var client = CreateAuthorizedClient(_adminToken);
        var veiculo = NovoVeiculo("Ford", "Focus", 2020, "Preto");

        var response = await client.PostAsJsonAsync("/veiculos", veiculo);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<Veiculo>();
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task PostVeiculo_Editor_ReturnsForbidden()
    {
        await EnsureTokensAreAvailable();
        var client = CreateAuthorizedClient(_editorToken);

        var response = await client.PostAsJsonAsync(
            "/veiculos",
            NovoVeiculo("Ford", "Focus", 2020, "Preto"));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetVeiculos_Editor_ReturnsOk()
    {
        await EnsureTokensAreAvailable();
        var client = CreateAuthorizedClient(_editorToken);

        var response = await client.GetAsync("/veiculos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(await response.Content.ReadFromJsonAsync<List<Veiculo>>());
    }

    [Fact]
    public async Task GetVeiculoById_Editor_ReturnsOk()
    {
        await EnsureTokensAreAvailable();
        var adminClient = CreateAuthorizedClient(_adminToken);
        var postResponse = await adminClient.PostAsJsonAsync(
            "/veiculos",
            NovoVeiculo("VW", "Gol", 2022, "Branco"));
        var created = await postResponse.Content.ReadFromJsonAsync<Veiculo>();
        Assert.NotNull(created);

        var editorClient = CreateAuthorizedClient(_editorToken);
        var response = await editorClient.GetAsync($"/veiculos/{created.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var retrieved = await response.Content.ReadFromJsonAsync<Veiculo>();
        Assert.NotNull(retrieved);
        Assert.Equal(created.Id, retrieved.Id);
    }

    [Fact]
    public async Task GetVeiculoById_Inexistente_ReturnsNotFound()
    {
        await EnsureTokensAreAvailable();
        var client = CreateAuthorizedClient(_editorToken);

        var response = await client.GetAsync("/veiculos/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetVeiculos_SemToken_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/veiculos");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PutVeiculo_Admin_ReturnsNoContent()
    {
        await EnsureTokensAreAvailable();
        var client = CreateAuthorizedClient(_adminToken);
        var postResponse = await client.PostAsJsonAsync(
            "/veiculos",
            NovoVeiculo("Fiat", "Uno", 2015, "Vermelho"));
        var created = await postResponse.Content.ReadFromJsonAsync<Veiculo>();
        Assert.NotNull(created);

        created.Cor = "Azul";
        var response = await client.PutAsJsonAsync($"/veiculos/{created.Id}", created);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task PutVeiculo_Editor_ReturnsForbidden()
    {
        await EnsureTokensAreAvailable();
        var adminClient = CreateAuthorizedClient(_adminToken);
        var postResponse = await adminClient.PostAsJsonAsync(
            "/veiculos",
            NovoVeiculo("Chevrolet", "Onix", 2023, "Cinza"));
        var created = await postResponse.Content.ReadFromJsonAsync<Veiculo>();
        Assert.NotNull(created);

        var editorClient = CreateAuthorizedClient(_editorToken);
        created.Cor = "Preto";
        var response = await editorClient.PutAsJsonAsync($"/veiculos/{created.Id}", created);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task DeleteVeiculo_Admin_ReturnsNoContent()
    {
        await EnsureTokensAreAvailable();
        var client = CreateAuthorizedClient(_adminToken);
        var postResponse = await client.PostAsJsonAsync(
            "/veiculos",
            NovoVeiculo("Hyundai", "HB20", 2021, "Prata"));
        var created = await postResponse.Content.ReadFromJsonAsync<Veiculo>();
        Assert.NotNull(created);

        var response = await client.DeleteAsync($"/veiculos/{created.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteVeiculo_Editor_ReturnsForbidden()
    {
        await EnsureTokensAreAvailable();
        var adminClient = CreateAuthorizedClient(_adminToken);
        var postResponse = await adminClient.PostAsJsonAsync(
            "/veiculos",
            NovoVeiculo("Renault", "Kwid", 2019, "Laranja"));
        var created = await postResponse.Content.ReadFromJsonAsync<Veiculo>();
        Assert.NotNull(created);

        var editorClient = CreateAuthorizedClient(_editorToken);
        var response = await editorClient.DeleteAsync($"/veiculos/{created.Id}");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    private HttpClient CreateAuthorizedClient(string token)
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    private static Veiculo NovoVeiculo(
        string marca,
        string modelo,
        int ano,
        string cor)
    {
        return new Veiculo
        {
            Marca = marca,
            Modelo = modelo,
            Ano = ano,
            Cor = cor
        };
    }
}
