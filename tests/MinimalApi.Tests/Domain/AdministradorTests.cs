using MinimalApi.Domain.Entidades;
using Xunit;

namespace MinimalApi.Tests.Domain
{
    public class AdministradorTests
    {
        [Fact]
        public void Administrador_CanBeCreatedAndPropertiesSet()
        {
            // Arrange
            var admin = new Administrador
            {
                Id = 1,
                Email = "test@example.com",
                Senha = "password123",
                Perfil = "Admin"
            };

            // Assert
            Assert.Equal(1, admin.Id);
            Assert.Equal("test@example.com", admin.Email);
            Assert.Equal("password123", admin.Senha);
            Assert.Equal("Admin", admin.Perfil);
        }
    }
}