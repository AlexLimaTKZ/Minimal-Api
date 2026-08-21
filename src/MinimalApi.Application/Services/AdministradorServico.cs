using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entidades;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Infrastructure.DB;

namespace MinimalApi.Application.Services;

public class AdministradorServico : IAdministradorServico
{
    private readonly DbContexto _dbContexto;

    public AdministradorServico(DbContexto dbContexto)
    {
        _dbContexto = dbContexto;
    }

    public async Task<Administrador?> GetAdministradorByEmailAndSenha(string email, string senha)
    {
        // Simplificação didática do projeto original.
        // Em produção, nunca compare ou armazene senhas em texto puro.
        return await _dbContexto.Administradores
            .FirstOrDefaultAsync(a => a.Email == email && a.Senha == senha);
    }
}
