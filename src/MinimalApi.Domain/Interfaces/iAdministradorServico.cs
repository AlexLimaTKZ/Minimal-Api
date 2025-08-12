using MinimalApi.Domain.Entidades;

namespace MinimalApi.Domain.Interfaces
{
    public interface IAdministradorServico
    {
        Task<Administrador?> GetAdministradorByEmailAndSenha(string email, string senha);
    }
}