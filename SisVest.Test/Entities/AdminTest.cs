using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Entities
{
    [TestClass]
    public class AdminTest
    {
        public Admin admin1, admin2;

        [TestInitialize]
        public void InicializarTest()
        {
            admin1 = new Admin()
            {
                iAdminID = 1,
                sEmail = "João@devmedia.com.br",
                sLogin = "joao",
                sNomeTratamento = "Joaozinho",
                sSenha = "123456"
            };


        }

        [TestMethod]
        public void Garantir_Que_2_Admins_Sao_Iguais_Quando_Tem_Mesmo_Id()
        {
            admin2 = new Admin()
            {
                iAdminID = 1,
                sEmail = "marcelo@devmedia.com.br",
                sLogin = "marcelo",
                sNomeTratamento = "Marcelo",
                sSenha = "147852"
            };

            Assert.AreEqual(admin1.iAdminID, admin2.iAdminID);
            Assert.AreEqual(admin1, admin2);
        }

        [TestMethod]
        public void Garantir_Que_2_Admins_Sao_Iguais_Quando_Tem_Mesmo_Login()
        {
            admin2 = new Admin()
            {
                iAdminID = 2,
                sEmail = "joao@devmedia.com.br",
                sLogin = "joao",
                sNomeTratamento = "joao",
                sSenha = "147852"
            };

            Assert.AreEqual(admin2.sLogin, admin2.sLogin);
            Assert.AreEqual(admin1, admin2);
        }

        [TestMethod]
        public void Garantir_Que_2_Admins_Sao_Iguais_Quando_Tem_Mesmo_Email()
        {
            admin2 = new Admin()
            {
                iAdminID = 2,
                sEmail = "joao.oliveira@devmedia.com.br",
                sLogin = "joao",
                sNomeTratamento = "joao",
                sSenha = "147852"
            };

            Assert.AreEqual(admin2.sEmail, admin2.sEmail);
            Assert.AreEqual(admin1, admin2);
        }
    }
}
