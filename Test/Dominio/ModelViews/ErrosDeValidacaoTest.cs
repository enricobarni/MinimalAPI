using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Dominio.ModelViews;

namespace Test.Dominio.ModelViews
{
    [TestClass]
    public class ErrosDeValidacaoTest
    {
        [TestMethod]
        public void TestGetSetMensagensListaVazia()
        {
            // ARRANGE
            var erros = new ErrosDeValidacao();

            // ACT
            erros.Mensagens = new List<string>();

            // ASSERT
            Assert.AreEqual(0, erros.Mensagens.Count);
        }

        [TestMethod]
        public void TestGetSetMensagensComUmItem()
        {
            // ARRANGE
            var erros = new ErrosDeValidacao();

            // ACT
            erros.Mensagens = new List<string> { "Email inválido" };

            // ASSERT
            Assert.AreEqual(1, erros.Mensagens.Count);
            Assert.AreEqual("Email inválido", erros.Mensagens[0]);
        }

        [TestMethod]
        public void TestGetSetMensagensComMultiplosItens()
        {
            // ARRANGE
            var erros = new ErrosDeValidacao();

            // ACT
            erros.Mensagens = new List<string> { "Email inválido", "Senha vazia", "Ano inválido" };

            // ASSERT
            Assert.AreEqual(3, erros.Mensagens.Count);
            CollectionAssert.AreEqual(
                new List<string> { "Email inválido", "Senha vazia", "Ano inválido" },
                erros.Mensagens);
        }

        [TestMethod]
        public void TestMensagensNulaNaoLancaExcecaoAoAtribuirELer()
        {
            // ARRANGE
            var erros = new ErrosDeValidacao();

            // ACT
            erros.Mensagens = null!;

            // ASSERT
            // A struct não faz nenhuma validação: atribuir e ler null é permitido
            // e não lança exceção, apenas retorna null.
            Assert.IsNull(erros.Mensagens);
        }
    }
}
