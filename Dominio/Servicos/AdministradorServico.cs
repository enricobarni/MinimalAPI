using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Dominio.Entidades;
using MinimalAPI.Infraestrutura.Db;
using MinimalAPI.Infraestrutura.Interfaces;
using ProjetoAPI.Dominio.DTOs;

namespace MinimalAPI.Dominio.Servicos
{
    public class AdministradorServico : IAdministradorServico
    {
        private readonly DbContexto _context;

        public AdministradorServico(DbContexto context)
        {
            _context = context;
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
            var adm = _context
                .Administradores.Where(a =>
                    a.Email == loginDTO.Email && a.Password == loginDTO.Password
                )
                .FirstOrDefault();

            return adm;
        }
    }
}
