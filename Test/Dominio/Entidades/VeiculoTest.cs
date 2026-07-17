using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Dominio.Entidades;

namespace Test.Dominio.Entidades
{
    [TestClass]
    public class VeiculoTest
    {
        [TestMethod]
        public void TestGetSetPropriedadesVeiculo()
        {
            // ARRANGE
            var veiculo = new Veiculo();

            // ACT
            veiculo.Id = 1;
            veiculo.Nome = "teste@teste.com";
            veiculo.Marca = "teste";
            veiculo.Ano = 1;

            //ASSERT
            Assert.AreEqual(1, veiculo.Id);
            Assert.AreEqual("teste@teste.com", veiculo.Nome);
            Assert.AreEqual("teste", veiculo.Marca);
            Assert.AreEqual(1, veiculo.Ano);
        }
    }
}
