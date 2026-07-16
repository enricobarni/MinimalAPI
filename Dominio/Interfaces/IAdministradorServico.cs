using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalAPI.Dominio.Entidades;
using ProjetoAPI.Dominio.DTOs;

namespace MinimalAPI.Infraestrutura.Interfaces
{
    public interface IAdministradorServico
    {
        Administrador? Login(LoginDTO loginDTO);
    }
}
