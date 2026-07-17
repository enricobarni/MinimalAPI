using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Dominio.ModelViews;

namespace Test.Dominio.ModelViews
{
    [TestClass]
    public class AdministradorModelViewTest
    {
        [TestMethod]
        public void TestGetSetPropriedadesAdministradorModelView()
        {
            // ARRANGE
            var admModelView = new AdministradorModelView();

            // ACT
            admModelView.Id = 1;
            admModelView.Email = "teste@teste.com";
            admModelView.Perfil = "Admin";

            // ASSERT
            Assert.AreEqual(1, admModelView.Id);
            Assert.AreEqual("teste@teste.com", admModelView.Email);
            Assert.AreEqual("Admin", admModelView.Perfil);
        }

        [TestMethod]
        public void TestAdministradorModelViewAceitaEmailEPerfilVazios()
        {
            // ARRANGE
            var admModelView = new AdministradorModelView();

            // ACT
            admModelView.Email = string.Empty;
            admModelView.Perfil = string.Empty;

            // ASSERT
            Assert.AreEqual(string.Empty, admModelView.Email);
            Assert.AreEqual(string.Empty, admModelView.Perfil);
        }

        [TestMethod]
        public void TestAdministradorModelViewAceitaEmailEPerfilNulos()
        {
            // ARRANGE
            var admModelView = new AdministradorModelView();

            // ACT
            admModelView.Email = null!;
            admModelView.Perfil = null!;

            // ASSERT
            Assert.IsNull(admModelView.Email);
            Assert.IsNull(admModelView.Perfil);
        }

        [TestMethod]
        public void TestAdministradorModelViewIgualdadeEstruturalComMesmosValores()
        {
            // ARRANGE
            var admModelView1 = new AdministradorModelView { Id = 1, Email = "teste@teste.com", Perfil = "Admin" };
            var admModelView2 = new AdministradorModelView { Id = 1, Email = "teste@teste.com", Perfil = "Admin" };

            // ACT & ASSERT
            Assert.AreEqual(admModelView1, admModelView2);
            Assert.IsTrue(admModelView1 == admModelView2);
        }

        [TestMethod]
        public void TestAdministradorModelViewDesigualdadeEstruturalComValoresDiferentes()
        {
            // ARRANGE
            var admModelView1 = new AdministradorModelView { Id = 1, Email = "teste@teste.com", Perfil = "Admin" };
            var admModelView2 = new AdministradorModelView { Id = 2, Email = "teste@teste.com", Perfil = "Editor" };

            // ACT & ASSERT
            Assert.AreNotEqual(admModelView1, admModelView2);
            Assert.IsTrue(admModelView1 != admModelView2);
        }
    }
}
