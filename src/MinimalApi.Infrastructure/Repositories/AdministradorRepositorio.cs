using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entidades;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Infrastructure.DB;

namespace MinimalApi.Infrastructure.Repositories;

public class AdministradorRepositorio : IAdministradorRepositorio
{
    private readonly DbContexto _dbContexto;

    public AdministradorRepositorio(DbContexto dbContexto)
    {
        _dbContexto = dbContexto;
    }

    public Task<Administrador?> BuscarPorEmailAsync(string email)
    {
        return _dbContexto.Administradores
            .FirstOrDefaultAsync(administrador => administrador.Email == email);
    }
}
