using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Entidades.DTOs;
using MinimalApi.DTOs;

namespace MinimalApi.Dominio.Interfaces;

public interface IAdministradorServico
{
    List<Administrador> Login(LoginDTO loginDTO);
}