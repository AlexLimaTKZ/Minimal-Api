using System.Data.Common;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Entidades.DTOs;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.DTO;

namespace minimal_api.Dominio.Servicos;

public class AdministradorServico : IAdministradorServico
{
    private readonly DbContexto _contexto;
    public AdministradorServico(DbContexto db)
    {
        _contexto = Context;
    }
    public List<Administrador> Login(LoginDTO loginDTO)
    (
        if(_contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha== loginDTO.Senha))
    )
}