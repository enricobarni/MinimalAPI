using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Dominio.Entidades;
using MinimalAPI.Dominio.Interfaces;
using MinimalAPI.Infraestrutura.Db;

namespace MinimalAPI.Dominio.Servicos
{
    public class VeiculoServico : IVeiculoServico
    {
        private readonly DbContexto _context;

        public VeiculoServico(DbContexto context)
        {
            _context = context;
        }

        public List<Veiculo> Todos(int pagina = 1, string? nome = null, string? marca = null)
        {
            var query = _context.Veiculos.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(v =>
                    EF.Functions.Like(v.Nome.ToLower(), $"%{nome.ToLower()}%")
                );
            }

            int itensPerPage = 10;

            query = query.Skip((pagina - 1) * itensPerPage).Take(itensPerPage);

            return query.ToList();
        }

        public Veiculo? BuscarPorId(int id)
        {
            return _context.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Adicionar(Veiculo veiculo)
        {
            _context.Veiculos.Add(veiculo);
            _context.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            _context.Veiculos.Update(veiculo);
            _context.SaveChanges();
        }

        public void Deletar(Veiculo veiculo)
        {
            _context.Veiculos.Remove(veiculo);
            _context.SaveChanges();
        }
    }
}
