using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Dominio.Entidades;
using MinimalAPI.Dominio.Servicos;
using MinimalAPI.Infraestrutura.Db;

namespace Test.Dominio.Servicos
{
    [TestClass]
    public class VeiculoServicoTest
    {
        private DbContexto _context = default!;
        private VeiculoServico _veiculoServico = default!;

        [TestInitialize]
        public void Setup()
        {
            // ARRANGE - banco InMemory isolado por teste
            var options = new DbContextOptionsBuilder<DbContexto>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DbContexto(options);
            _context.Database.EnsureCreated();
            _veiculoServico = new VeiculoServico(_context);
        }

        [TestMethod]
        public void TestAdicionar_DeveInserirVeiculo()
        {
            // ARRANGE
            var veiculo = new Veiculo
            {
                Nome = "Gol",
                Marca = "Volkswagen",
                Ano = 2020,
            };

            // ACT
            _veiculoServico.Adicionar(veiculo);

            // ASSERT
            Assert.IsTrue(veiculo.Id > 0);
            var resultado = _veiculoServico.BuscarPorId(veiculo.Id);
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Gol", resultado!.Nome);
        }

        [TestMethod]
        public void TestBuscarPorId_IdExistente_DeveRetornarVeiculoCorreto()
        {
            // ARRANGE
            var veiculo = new Veiculo
            {
                Nome = "Civic",
                Marca = "Honda",
                Ano = 2021,
            };
            _veiculoServico.Adicionar(veiculo);

            // ACT
            var resultado = _veiculoServico.BuscarPorId(veiculo.Id);

            // ASSERT
            Assert.IsNotNull(resultado);
            Assert.AreEqual(veiculo.Id, resultado!.Id);
            Assert.AreEqual("Civic", resultado.Nome);
            Assert.AreEqual("Honda", resultado.Marca);
            Assert.AreEqual(2021, resultado.Ano);
        }

        [TestMethod]
        public void TestBuscarPorId_IdInexistente_DeveRetornarNull()
        {
            // ARRANGE
            int idInexistente = 999;

            // ACT
            var resultado = _veiculoServico.BuscarPorId(idInexistente);

            // ASSERT
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void TestAtualizar_DeveAlterarNomeMarcaAnoEPersistir()
        {
            // ARRANGE
            var veiculo = new Veiculo
            {
                Nome = "Gol",
                Marca = "Volkswagen",
                Ano = 2020,
            };
            _veiculoServico.Adicionar(veiculo);

            // ACT
            veiculo.Nome = "Gol GTI";
            veiculo.Marca = "VW";
            veiculo.Ano = 2023;
            _veiculoServico.Atualizar(veiculo);

            // ASSERT
            var resultado = _veiculoServico.BuscarPorId(veiculo.Id);
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Gol GTI", resultado!.Nome);
            Assert.AreEqual("VW", resultado.Marca);
            Assert.AreEqual(2023, resultado.Ano);
        }

        [TestMethod]
        public void TestDeletar_DeveRemoverVeiculoENaoAparecerMaisEmBuscarPorId()
        {
            // ARRANGE
            var veiculo = new Veiculo
            {
                Nome = "Palio",
                Marca = "Fiat",
                Ano = 2015,
            };
            _veiculoServico.Adicionar(veiculo);
            int id = veiculo.Id;

            // ACT
            _veiculoServico.Deletar(veiculo);

            // ASSERT
            var resultado = _veiculoServico.BuscarPorId(id);
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void TestTodos_SemFiltros_DeveRetornarTodosInseridos()
        {
            // ARRANGE
            _veiculoServico.Adicionar(new Veiculo { Nome = "Gol", Marca = "VW", Ano = 2020 });
            _veiculoServico.Adicionar(new Veiculo { Nome = "Civic", Marca = "Honda", Ano = 2021 });
            _veiculoServico.Adicionar(new Veiculo { Nome = "Palio", Marca = "Fiat", Ano = 2015 });

            // ACT
            var resultado = _veiculoServico.Todos(null);

            // ASSERT
            Assert.AreEqual(3, resultado.Count);
        }

        [TestMethod]
        public void TestTodos_FiltroNomeCaseInsensitive_DeveEncontrarGol()
        {
            // ARRANGE
            _veiculoServico.Adicionar(new Veiculo { Nome = "Gol", Marca = "VW", Ano = 2020 });
            _veiculoServico.Adicionar(new Veiculo { Nome = "Civic", Marca = "Honda", Ano = 2021 });

            // ACT
            var resultado = _veiculoServico.Todos(null, "gol");

            // ASSERT
            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("Gol", resultado[0].Nome);
        }

        [TestMethod]
        public void TestTodos_FiltroNomePorSubstringParcial_DeveEncontrarCorrespondencias()
        {
            // ARRANGE
            _veiculoServico.Adicionar(new Veiculo { Nome = "Gol", Marca = "VW", Ano = 2020 });
            _veiculoServico.Adicionar(new Veiculo { Nome = "Golf", Marca = "VW", Ano = 2022 });
            _veiculoServico.Adicionar(new Veiculo { Nome = "Civic", Marca = "Honda", Ano = 2021 });

            // ACT
            var resultado = _veiculoServico.Todos(null, "gol");

            // ASSERT
            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public void TestTodos_FiltroNomeSemMatch_DeveRetornarListaVazia()
        {
            // ARRANGE
            _veiculoServico.Adicionar(new Veiculo { Nome = "Gol", Marca = "VW", Ano = 2020 });

            // ACT
            var resultado = _veiculoServico.Todos(null, "inexistente");

            // ASSERT
            Assert.AreEqual(0, resultado.Count);
        }

        [TestMethod]
        public void TestTodos_ParametroMarca_NaoDeveAfetarResultado()
        {
            // ARRANGE - o filtro implementado em VeiculoServico.Todos usa EF.Functions.Like
            // somente sobre o campo Nome; o parametro "marca" existe na assinatura mas
            // nao e utilizado em nenhum filtro da query. Este teste documenta esse
            // comportamento real (nao assume que "marca" filtra os resultados).
            _veiculoServico.Adicionar(new Veiculo { Nome = "Gol", Marca = "VW", Ano = 2020 });
            _veiculoServico.Adicionar(new Veiculo { Nome = "Civic", Marca = "Honda", Ano = 2021 });

            // ACT - informar uma marca que nao corresponde a nenhum veiculo
            var resultado = _veiculoServico.Todos(null, null, "MarcaQueNaoExiste");

            // ASSERT - todos os veiculos sao retornados, pois "marca" e ignorado
            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public void TestTodos_PaginaNula_DeveRetornarTodosOsRegistros()
        {
            // ARRANGE
            for (int i = 0; i < 12; i++)
            {
                _veiculoServico.Adicionar(
                    new Veiculo { Nome = $"Veiculo{i}", Marca = "Marca", Ano = 2020 }
                );
            }

            // ACT
            var resultado = _veiculoServico.Todos(null);

            // ASSERT
            Assert.AreEqual(12, resultado.Count);
        }

        [TestMethod]
        public void TestTodos_Pagina1_DeveRetornarAteDezItens()
        {
            // ARRANGE
            for (int i = 0; i < 15; i++)
            {
                _veiculoServico.Adicionar(
                    new Veiculo { Nome = $"Veiculo{i}", Marca = "Marca", Ano = 2020 }
                );
            }

            // ACT
            var resultado = _veiculoServico.Todos(1);

            // ASSERT
            Assert.AreEqual(10, resultado.Count);
        }

        [TestMethod]
        public void TestTodos_PaginaForaDoRange_DeveRetornarListaVazia()
        {
            // ARRANGE
            _veiculoServico.Adicionar(new Veiculo { Nome = "Gol", Marca = "VW", Ano = 2020 });

            // ACT
            var resultado = _veiculoServico.Todos(999);

            // ASSERT
            Assert.AreEqual(0, resultado.Count);
        }
    }
}
