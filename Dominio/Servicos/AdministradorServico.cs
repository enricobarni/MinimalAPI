using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Dominio.DTOs;
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

        public Administrador Adicionar(Administrador administrador)
        {
            _context.Administradores.Add(administrador);
            _context.SaveChanges();

            return administrador;
        }

        public Administrador? BuscarPorId(int id)
        {
            return _context.Administradores.Where(v => v.Id == id).FirstOrDefault();
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

        public List<Administrador> Todos(int? pagina)
        {
            var query = _context.Administradores.AsQueryable();

            int itensPerPage = 10;

            if (pagina != null)
            {
                query = query.Skip(((int)pagina - 1) * itensPerPage).Take(itensPerPage);
            }

            return query.ToList();
        }
    }
}
