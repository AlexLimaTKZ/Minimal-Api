using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entidades;
using MinimalApi.Infrastructure.DB;
using Xunit;

namespace MinimalApi.Tests.Infrastructure
{
    public class DbContextoTests
    {
        private DbContexto GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DbContexto>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new DbContexto(options);
        }

        [Fact]
        public async Task AddAdministrador_ShouldAddSuccessfully()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var admin = new Administrador { Email = "test@example.com", Senha = "password", Perfil = "Admin" };

            // Act
            context.Administradores.Add(admin);
            await context.SaveChangesAsync();

            // Assert
            Assert.Equal(1, await context.Administradores.CountAsync());
            Assert.NotNull(await context.Administradores.FirstOrDefaultAsync(a => a.Email == "test@example.com"));
        }

        [Fact]
        public async Task AddVeiculo_ShouldAddSuccessfully()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var veiculo = new Veiculo { Marca = "Ford", Modelo = "Fiesta", Ano = 2020, Cor = "Azul" };

            // Act
            context.Veiculos.Add(veiculo);
            await context.SaveChangesAsync();

            // Assert
            Assert.Equal(1, await context.Veiculos.CountAsync());
            Assert.NotNull(await context.Veiculos.FirstOrDefaultAsync(v => v.Modelo == "Fiesta"));
        }

        [Fact]
        public async Task GetVeiculo_ShouldReturnCorrectVeiculo()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var veiculo = new Veiculo { Marca = "VW", Modelo = "Gol", Ano = 2022, Cor = "Preto" };
            context.Veiculos.Add(veiculo);
            await context.SaveChangesAsync();

            // Act
            var retrievedVeiculo = await context.Veiculos.FindAsync(veiculo.Id);

            // Assert
            Assert.NotNull(retrievedVeiculo);
            Assert.Equal("Gol", retrievedVeiculo.Modelo);
        }

        [Fact]
        public async Task UpdateVeiculo_ShouldUpdateSuccessfully()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var veiculo = new Veiculo { Marca = "Honda", Modelo = "Civic", Ano = 2021, Cor = "Prata" };
            context.Veiculos.Add(veiculo);
            await context.SaveChangesAsync();

            // Act
            veiculo.Cor = "Vermelho";
            context.Veiculos.Update(veiculo);
            await context.SaveChangesAsync();

            // Assert
            var updatedVeiculo = await context.Veiculos.FindAsync(veiculo.Id);
            Assert.Equal("Vermelho", updatedVeiculo.Cor);
        }

        [Fact]
        public async Task DeleteVeiculo_ShouldDeleteSuccessfully()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var veiculo = new Veiculo { Marca = "Toyota", Modelo = "Corolla", Ano = 2023, Cor = "Branco" };
            context.Veiculos.Add(veiculo);
            await context.SaveChangesAsync();

            // Act
            context.Veiculos.Remove(veiculo);
            await context.SaveChangesAsync();

            // Assert
            Assert.Equal(0, await context.Veiculos.CountAsync());
        }
    }
}