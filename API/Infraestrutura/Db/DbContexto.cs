using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Dominio.Entidades;

[assembly: InternalsVisibleTo("Test")]

namespace MinimalAPI.Infraestrutura.Db
{
    public class DbContexto : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbContexto(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal DbContexto(DbContextOptions<DbContexto> options)
            : base(options) { }

        public DbSet<Administrador> Administradores { get; set; } = default!;
        public DbSet<Veiculo> Veiculos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Administrador>()
                .HasData(
                    new Administrador
                    {
                        Id = 1,
                        Email = "admin@teste.com",
                        Password = "123456",
                        Perfil = "Admin",
                    }
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("SqlServer")?.ToString();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseSqlServer(connectionString);
                }
            }
        }
    }
}
