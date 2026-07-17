using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Dominio.Entidades;
using MinimalAPI.Dominio.Servicos;
using MinimalAPI.Infraestrutura.Db;
using ProjetoAPI.Dominio.DTOs;

namespace Test.Dominio.Servicos
{
    [TestClass]
    public class AdministradorServicoTest
    {
        private DbContexto _context = default!;
        private AdministradorServico _administradorServico = default!;

        [TestInitialize]
        public void Setup()
        {
            // ARRANGE - banco InMemory isolado por teste
            var options = new DbContextOptionsBuilder<DbContexto>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DbContexto(options);
            _context.Database.EnsureCreated();
            _administradorServico = new AdministradorServico(_context);
        }

        [TestMethod]
        public void TestAdicionar_DeveInserirERetornarAdministradorComIdGerado()
        {
            // ARRANGE
            var administrador = new Administrador
            {
                Email = "novo@teste.com",
                Password = "senha123",
                Perfil = "Editor",
            };

            // ACT
            var resultado = _administradorServico.Adicionar(administrador);

            // ASSERT
            Assert.IsTrue(resultado.Id > 0);
            Assert.AreEqual("novo@teste.com", resultado.Email);
            Assert.AreEqual("senha123", resultado.Password);
            Assert.AreEqual("Editor", resultado.Perfil);
        }

        [TestMethod]
        public void TestBuscarPorId_IdExistente_DeveRetornarAdministradorCorreto()
        {
            // ARRANGE
            var administrador = _administradorServico.Adicionar(
                new Administrador
                {
                    Email = "buscar@teste.com",
                    Password = "senha123",
                    Perfil = "Editor",
                }
            );

            // ACT
            var resultado = _administradorServico.BuscarPorId(administrador.Id);

            // ASSERT
            Assert.IsNotNull(resultado);
            Assert.AreEqual(administrador.Id, resultado!.Id);
            Assert.AreEqual("buscar@teste.com", resultado.Email);
        }

        [TestMethod]
        public void TestBuscarPorId_IdInexistente_DeveRetornarNull()
        {
            // ARRANGE
            int idInexistente = 999;

            // ACT
            var resultado = _administradorServico.BuscarPorId(idInexistente);

            // ASSERT
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void TestLogin_CredenciaisCorretasDoSeed_DeveRetornarAdministrador()
        {
            // ARRANGE - reaproveita o Administrador do seed (Id=1, admin@teste.com/123456/Admin)
            var loginDTO = new LoginDTO { Email = "admin@teste.com", Password = "123456" };

            // ACT
            var resultado = _administradorServico.Login(loginDTO);

            // ASSERT
            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado!.Id);
            Assert.AreEqual("admin@teste.com", resultado.Email);
            Assert.AreEqual("Admin", resultado.Perfil);
        }

        [TestMethod]
        public void TestLogin_EmailIncorreto_DeveRetornarNull()
        {
            // ARRANGE
            var loginDTO = new LoginDTO { Email = "naoexiste@teste.com", Password = "123456" };

            // ACT
            var resultado = _administradorServico.Login(loginDTO);

            // ASSERT
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void TestLogin_SenhaIncorreta_DeveRetornarNull()
        {
            // ARRANGE
            var loginDTO = new LoginDTO { Email = "admin@teste.com", Password = "senhaerrada" };

            // ACT
            var resultado = _administradorServico.Login(loginDTO);

            // ASSERT
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void TestLogin_EmailVazio_DeveRetornarNull()
        {
            // ARRANGE
            var loginDTO = new LoginDTO { Email = "", Password = "123456" };

            // ACT
            var resultado = _administradorServico.Login(loginDTO);

            // ASSERT
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void TestLogin_SenhaVazia_DeveRetornarNull()
        {
            // ARRANGE
            var loginDTO = new LoginDTO { Email = "admin@teste.com", Password = "" };

            // ACT
            var resultado = _administradorServico.Login(loginDTO);

            // ASSERT
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void TestLogin_EmailESenhaNulos_DeveRetornarNull()
        {
            // ARRANGE
            var loginDTO = new LoginDTO { Email = null!, Password = null! };

            // ACT
            var resultado = _administradorServico.Login(loginDTO);

            // ASSERT
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void TestTodos_PaginaNula_DeveRetornarTodosOsRegistros()
        {
            // ARRANGE - o seed já insere 1 Administrador (Id=1); adicionamos mais 2
            _administradorServico.Adicionar(
                new Administrador
                {
                    Email = "a1@teste.com",
                    Password = "123456",
                    Perfil = "Editor",
                }
            );
            _administradorServico.Adicionar(
                new Administrador
                {
                    Email = "a2@teste.com",
                    Password = "123456",
                    Perfil = "Editor",
                }
            );

            // ACT
            var resultado = _administradorServico.Todos(null);

            // ASSERT - 2 adicionados + 1 do seed = 3
            Assert.AreEqual(3, resultado.Count);
        }

        [TestMethod]
        public void TestTodos_Pagina1_DeveRetornarAteDezItens()
        {
            // ARRANGE - insere 15 administradores além do seed (totalizando 16 registros)
            for (int i = 0; i < 15; i++)
            {
                _administradorServico.Adicionar(
                    new Administrador
                    {
                        Email = $"admin{i}@teste.com",
                        Password = "123456",
                        Perfil = "Editor",
                    }
                );
            }

            // ACT
            var resultado = _administradorServico.Todos(1);

            // ASSERT
            Assert.AreEqual(10, resultado.Count);
        }

        [TestMethod]
        public void TestTodos_PaginaForaDoRange_DeveRetornarListaVazia()
        {
            // ARRANGE
            _administradorServico.Adicionar(
                new Administrador
                {
                    Email = "unico@teste.com",
                    Password = "123456",
                    Perfil = "Editor",
                }
            );

            // ACT
            var resultado = _administradorServico.Todos(999);

            // ASSERT
            Assert.AreEqual(0, resultado.Count);
        }
    }
}
