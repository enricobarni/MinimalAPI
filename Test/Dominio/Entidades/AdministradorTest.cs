using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Dominio.Entidades;

namespace Test.Dominio.Entidades
{
    [TestClass]
    public class AdministradorTest
    {
        [TestMethod]
        public void TestGetSetPropriedadesAdmin()
        {
            // ARRANGE
            var admin = new Administrador();

            // ACT
            admin.Id = 1;
            admin.Email = "teste@teste.com";
            admin.Password = "teste";
            admin.Perfil = "Admin";

            //ASSERT
            Assert.AreEqual(1, admin.Id);
            Assert.AreEqual("teste@teste.com", admin.Email);
            Assert.AreEqual("teste", admin.Password);
            Assert.AreEqual("Admin", admin.Perfil);
        }
    }
}
