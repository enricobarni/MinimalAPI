using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.Dominio.DTOs;
using MinimalAPI.Dominio.Enuns;

namespace Test.Dominio.DTOs
{
    [TestClass]
    public class AdministradorDTOTest
    {
        [TestMethod]
        public void TestGetSetPropriedadesAdministradorDTO()
        {
            // ARRANGE
            var admDto = new AdministradorDTO();

            // ACT
            admDto.Email = "teste@teste.com";
            admDto.Password = "senha123";
            admDto.Perfil = Perfil.Admin;

            // ASSERT
            Assert.AreEqual("teste@teste.com", admDto.Email);
            Assert.AreEqual("senha123", admDto.Password);
            Assert.AreEqual(Perfil.Admin, admDto.Perfil);
        }

        [TestMethod]
        public void TestAdministradorDTOAceitaEmailEPasswordVazios()
        {
            // ARRANGE
            var admDto = new AdministradorDTO();

            // ACT
            admDto.Email = string.Empty;
            admDto.Password = string.Empty;

            // ASSERT
            Assert.AreEqual(string.Empty, admDto.Email);
            Assert.AreEqual(string.Empty, admDto.Password);
        }

        [TestMethod]
        public void TestAdministradorDTOAceitaEmailEPasswordNulos()
        {
            // ARRANGE
            var admDto = new AdministradorDTO();

            // ACT
            admDto.Email = null!;
            admDto.Password = null!;

            // ASSERT
            Assert.IsNull(admDto.Email);
            Assert.IsNull(admDto.Password);
        }

        [TestMethod]
        public void TestAdministradorDTOPerfilNulo()
        {
            // ARRANGE
            var admDto = new AdministradorDTO();

            // ACT
            admDto.Perfil = null;

            // ASSERT
            Assert.IsNull(admDto.Perfil);
        }

        [TestMethod]
        public void TestAdministradorDTOPerfilAdmin()
        {
            // ARRANGE
            var admDto = new AdministradorDTO();

            // ACT
            admDto.Perfil = Perfil.Admin;

            // ASSERT
            Assert.AreEqual(Perfil.Admin, admDto.Perfil);
        }

        [TestMethod]
        public void TestAdministradorDTOPerfilEditor()
        {
            // ARRANGE
            var admDto = new AdministradorDTO();

            // ACT
            admDto.Perfil = Perfil.Editor;

            // ASSERT
            Assert.AreEqual(Perfil.Editor, admDto.Perfil);
        }
    }
}
