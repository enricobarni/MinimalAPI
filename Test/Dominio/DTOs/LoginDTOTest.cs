using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetoAPI.Dominio.DTOs;

namespace Test.Dominio.DTOs
{
    [TestClass]
    public class LoginDTOTest
    {
        [TestMethod]
        public void TestGetSetPropriedadesLoginDTO()
        {
            // ARRANGE
            var login = new LoginDTO();

            // ACT
            login.Email = "teste@teste.com";
            login.Password = "senha123";

            // ASSERT
            Assert.AreEqual("teste@teste.com", login.Email);
            Assert.AreEqual("senha123", login.Password);
        }

        [TestMethod]
        public void TestLoginDTOAceitaEmailEPasswordVazios()
        {
            // ARRANGE
            var login = new LoginDTO();

            // ACT
            login.Email = string.Empty;
            login.Password = string.Empty;

            // ASSERT
            Assert.AreEqual(string.Empty, login.Email);
            Assert.AreEqual(string.Empty, login.Password);
        }

        [TestMethod]
        public void TestLoginDTOAceitaEmailEPasswordNulos()
        {
            // ARRANGE
            var login = new LoginDTO();

            // ACT
            login.Email = null!;
            login.Password = null!;

            // ASSERT
            Assert.IsNull(login.Email);
            Assert.IsNull(login.Password);
        }
    }
}
