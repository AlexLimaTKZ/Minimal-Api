using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalApi.Infrastructure.DB;

namespace MinimalApi.Tests.Api;

internal static class TestWebHost
{
    internal static void Configure(IWebHostBuilder builder, string databaseName)
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

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContexto>();
            services.RemoveAll<DbContextOptions<DbContexto>>();
            services.RemoveAll<IDbContextOptionsConfiguration<DbContexto>>();

            services.AddDbContext<DbContexto>(options =>
                options.UseInMemoryDatabase(databaseName));
        });
    }
}

internal sealed record TokenResponse(string Token, int ExpiresInMinutes);
