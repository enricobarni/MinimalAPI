using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Dominio.DTOs;

namespace Test.Dominio.DTOs
{
    [TestClass]
    public class VeiculoDTOTest
    {
        [TestMethod]
        public void TestGetSetPropriedadesVeiculoDTO()
        {
            // ARRANGE
            var veiculoDto = new VeiculoDTO();

            // ACT
            veiculoDto.Nome = "Fusca";
            veiculoDto.Marca = "Volkswagen";
            veiculoDto.Ano = 1990;

            // ASSERT
            Assert.AreEqual("Fusca", veiculoDto.Nome);
            Assert.AreEqual("Volkswagen", veiculoDto.Marca);
            Assert.AreEqual(1990, veiculoDto.Ano);
        }

        [TestMethod]
        public void TestVeiculoDTOAceitaNomeEMarcaVazios()
        {
            // ARRANGE
            var veiculoDto = new VeiculoDTO();

            // ACT
            veiculoDto.Nome = string.Empty;
            veiculoDto.Marca = string.Empty;

            // ASSERT
            Assert.AreEqual(string.Empty, veiculoDto.Nome);
            Assert.AreEqual(string.Empty, veiculoDto.Marca);
        }

        [TestMethod]
        public void TestVeiculoDTOAceitaNomeEMarcaNulos()
        {
            // ARRANGE
            var veiculoDto = new VeiculoDTO();

            // ACT
            veiculoDto.Nome = null!;
            veiculoDto.Marca = null!;

            // ASSERT
            Assert.IsNull(veiculoDto.Nome);
            Assert.IsNull(veiculoDto.Marca);
        }

        // As regras de negócio sobre o valor de "Ano" (ex: não aceitar ano < 1885)
        // são validadas em Program.cs, não no DTO. O DTO em si é apenas uma
        // estrutura de dados e deve aceitar qualquer valor de int, incluindo
        // valores de borda e inválidos do ponto de vista de negócio.
        [TestMethod]
        public void TestVeiculoDTOAceitaAnoLimiteInferiorValido()
        {
            // ARRANGE
            var veiculoDto = new VeiculoDTO();

            // ACT
            veiculoDto.Ano = 1885;

            // ASSERT
            Assert.AreEqual(1885, veiculoDto.Ano);
        }

        [TestMethod]
        public void TestVeiculoDTOAceitaAnoAbaixoDoLimiteDeNegocio()
        {
            // ARRANGE
            var veiculoDto = new VeiculoDTO();

            // ACT
            veiculoDto.Ano = 1884;

            // ASSERT
            // O DTO não valida essa regra, apenas armazena o valor.
            Assert.AreEqual(1884, veiculoDto.Ano);
        }

        [TestMethod]
        public void TestVeiculoDTOAceitaAnoZero()
        {
            // ARRANGE
            var veiculoDto = new VeiculoDTO();

            // ACT
            veiculoDto.Ano = 0;

            // ASSERT
            Assert.AreEqual(0, veiculoDto.Ano);
        }

        [TestMethod]
        public void TestVeiculoDTOAceitaAnoNegativo()
        {
            // ARRANGE
            var veiculoDto = new VeiculoDTO();

            // ACT
            veiculoDto.Ano = -1;

            // ASSERT
            Assert.AreEqual(-1, veiculoDto.Ano);
        }

        [TestMethod]
        public void TestVeiculoDTOAceitaAnoMaximoDeInt()
        {
            // ARRANGE
            var veiculoDto = new VeiculoDTO();

            // ACT
            veiculoDto.Ano = int.MaxValue;

            // ASSERT
            Assert.AreEqual(int.MaxValue, veiculoDto.Ano);
        }

        [TestMethod]
        public void TestVeiculoDTOIgualdadeEstruturalComMesmosValores()
        {
            // ARRANGE
            var veiculoDto1 = new VeiculoDTO { Nome = "Fusca", Marca = "Volkswagen", Ano = 1990 };
            var veiculoDto2 = new VeiculoDTO { Nome = "Fusca", Marca = "Volkswagen", Ano = 1990 };

            // ACT & ASSERT
            Assert.AreEqual(veiculoDto1, veiculoDto2);
            Assert.IsTrue(veiculoDto1 == veiculoDto2);
        }

        [TestMethod]
        public void TestVeiculoDTODesigualdadeEstruturalComValoresDiferentes()
        {
            // ARRANGE
            var veiculoDto1 = new VeiculoDTO { Nome = "Fusca", Marca = "Volkswagen", Ano = 1990 };
            var veiculoDto2 = new VeiculoDTO { Nome = "Gol", Marca = "Volkswagen", Ano = 1990 };

            // ACT & ASSERT
            Assert.AreNotEqual(veiculoDto1, veiculoDto2);
            Assert.IsTrue(veiculoDto1 != veiculoDto2);
        }
    }
}
