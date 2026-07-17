using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Dominio.Enuns;

namespace Test.Dominio.Enuns
{
    [TestClass]
    public class PerfilTest
    {
        [TestMethod]
        public void TestGetValuesRetornaApenasDoisValores()
        {
            // ARRANGE & ACT
            var valores = Enum.GetValues<Perfil>();

            // ASSERT
            Assert.AreEqual(2, valores.Length);
            CollectionAssert.AreEqual(new[] { Perfil.Admin, Perfil.Editor }, valores);
        }

        [TestMethod]
        public void TestParseStringValidaAdmin()
        {
            // ARRANGE
            var texto = "Admin";

            // ACT
            var perfil = Enum.Parse<Perfil>(texto);

            // ASSERT
            Assert.AreEqual(Perfil.Admin, perfil);
        }

        [TestMethod]
        public void TestParseStringValidaEditor()
        {
            // ARRANGE
            var texto = "Editor";

            // ACT
            var perfil = Enum.Parse<Perfil>(texto);

            // ASSERT
            Assert.AreEqual(Perfil.Editor, perfil);
        }

        [TestMethod]
        public void TestParseStringInvalidaLancaExcecao()
        {
            // ARRANGE
            var texto = "Invalido";

            // ACT & ASSERT
            Assert.ThrowsExactly<ArgumentException>(() => Enum.Parse<Perfil>(texto));
        }
    }
}
