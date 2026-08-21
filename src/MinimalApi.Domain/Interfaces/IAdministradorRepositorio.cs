using MinimalApi.Domain.Entidades;

namespace MinimalApi.Domain.Interfaces;

public interface IAdministradorRepositorio
{
    Task<Administrador?> BuscarPorEmailAsync(string email);
}
