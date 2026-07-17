using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Dominio.ModelViews;

namespace Test.Dominio.ModelViews
{
    [TestClass]
    public class AdminLogadoTest
    {
        [TestMethod]
        public void TestGetSetPropriedadesAdminLogado()
        {
            // ARRANGE
            var adminLogado = new AdminLogado();

            // ACT
            adminLogado.Email = "teste@teste.com";
            adminLogado.Perfil = "Admin";
            adminLogado.Token = "token-jwt-abc123";

            // ASSERT
            Assert.AreEqual("teste@teste.com", adminLogado.Email);
            Assert.AreEqual("Admin", adminLogado.Perfil);
            Assert.AreEqual("token-jwt-abc123", adminLogado.Token);
        }

        [TestMethod]
        public void TestAdminLogadoAceitaPropriedadesVazias()
        {
            // ARRANGE
            var adminLogado = new AdminLogado();

            // ACT
            adminLogado.Email = string.Empty;
            adminLogado.Perfil = string.Empty;
            adminLogado.Token = string.Empty;

            // ASSERT
            Assert.AreEqual(string.Empty, adminLogado.Email);
            Assert.AreEqual(string.Empty, adminLogado.Perfil);
            Assert.AreEqual(string.Empty, adminLogado.Token);
        }

        [TestMethod]
        public void TestAdminLogadoAceitaPropriedadesNulas()
        {
            // ARRANGE
            var adminLogado = new AdminLogado();

            // ACT
            adminLogado.Email = null!;
            adminLogado.Perfil = null!;
            adminLogado.Token = null!;

            // ASSERT
            Assert.IsNull(adminLogado.Email);
            Assert.IsNull(adminLogado.Perfil);
            Assert.IsNull(adminLogado.Token);
        }

        [TestMethod]
        public void TestAdminLogadoIgualdadeEstruturalComMesmosValores()
        {
            // ARRANGE
            var adminLogado1 = new AdminLogado { Email = "teste@teste.com", Perfil = "Admin", Token = "abc" };
            var adminLogado2 = new AdminLogado { Email = "teste@teste.com", Perfil = "Admin", Token = "abc" };

            // ACT & ASSERT
            Assert.AreEqual(adminLogado1, adminLogado2);
            Assert.IsTrue(adminLogado1 == adminLogado2);
        }

        [TestMethod]
        public void TestAdminLogadoDesigualdadeEstruturalComValoresDiferentes()
        {
            // ARRANGE
            var adminLogado1 = new AdminLogado { Email = "teste@teste.com", Perfil = "Admin", Token = "abc" };
            var adminLogado2 = new AdminLogado { Email = "outro@teste.com", Perfil = "Editor", Token = "xyz" };

            // ACT & ASSERT
            Assert.AreNotEqual(adminLogado1, adminLogado2);
            Assert.IsTrue(adminLogado1 != adminLogado2);
        }
    }
}
