using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Dominio.ModelViews;

namespace Test.Dominio.ModelViews
{
    [TestClass]
    public class HomeTest
    {
        [TestMethod]
        public void TestMensagemRetornaTextoFixo()
        {
            // ARRANGE
            var home = new Home();

            // ACT
            var mensagem = home.Mensagem;

            // ASSERT
            Assert.AreEqual(
                "Bem Vindo à API de Veículos 'MinimalAPI'! Para acessar a documentação da API, acesse o seguinte link: ",
                mensagem);
        }

        [TestMethod]
        public void TestDocRetornaTextoFixo()
        {
            // ARRANGE
            var home = new Home();

            // ACT
            var doc = home.doc;

            // ASSERT
            Assert.AreEqual("/swagger", doc);
        }
    }
}
