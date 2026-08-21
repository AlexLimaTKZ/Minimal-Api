using Microsoft.AspNetCore.Hosting;

namespace MinimalApi.Tests.Api;

internal static class TestWebHost
{
    internal static void Configure(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting(
            "ConnectionStrings:mysql",
            "Server=localhost;Database=minimal_api_tests;Uid=test;Pwd=test;");
        builder.UseSetting(
            "Jwt:Key",
            "ChaveSomenteParaTestesAutomatizadosComTamanhoSuficiente123456789");
        builder.UseSetting("Jwt:Issuer", "MinimalApiTests");
        builder.UseSetting("Jwt:Audience", "MinimalApiTests");
    }
}

internal sealed record TokenResponse(string Token, int ExpiresInMinutes);
