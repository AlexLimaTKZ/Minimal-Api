using MinimalApi.Domain.Entidades;
using MinimalApi.Domain.Interfaces;

namespace MinimalApi.Application.Services;

public class AdministradorServico : IAdministradorServico
{
    private readonly IAdministradorRepositorio _repositorio;

    public AdministradorServico(IAdministradorRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<Administrador?> GetAdministradorByEmailAndSenha(string email, string senha)
    {
        var administrador = await _repositorio.BuscarPorEmailAsync(email);

        // Simplificação didática do projeto original.
        // Em produção, a senha deve ser validada por hash seguro, nunca por texto puro.
        if (administrador is null || administrador.Senha != senha)
            return null;

        return administrador;
    }
}
