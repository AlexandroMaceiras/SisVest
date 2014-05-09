using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Repositories
{
    [TestClass]
    public class AdminRepositoriesTest
    {
        private IAdminRepository adminRepository;
        private VestContext vestContext = new VestContext();
        private Admin adminInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
            adminRepository = new EFAdminRepository(vestContext);
            adminInserir = (new Admin()
            {
                sEmail = "alexandro@maceiras.com.br",
                sLogin = "espanhol",
                sNomeTratamento = "Sr. Maceiras",
                sSenha = "123456"
            });
        }

        [TestMethod]
        public void Pode_Consultar_LINQ_Usando_RepositorioTest()
        {
            //Ambiente
            vestContext.Admins.Add(adminInserir);
            vestContext.SaveChanges();


            //Ação
            var admins = adminRepository.Admins;

            var retorno = (from a in admins
                           where a.sLogin.Equals(adminInserir.sLogin)
                           select a).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(admins, typeof(IQueryable<Admin>));
            Assert.AreEqual(retorno, adminInserir);
        }

        [TestMethod]
        public void Pode_Inserir_test()
        {
            //Ambiente


            //Ação
            adminRepository.Inserir(adminInserir);

            var retorno = (from a in adminRepository.Admins
                           where a.sLogin.Equals(adminInserir.sLogin)
                           select a).FirstOrDefault();

            //Assertivas
            Assert.AreEqual(retorno, adminInserir);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Inserir_Admin_Com_Mesmo_Email_test()
        {
            //Ambiente
            var adminInserir2 = new Admin()
            {
                sEmail = adminInserir.sEmail,
                sLogin = "espanhol",
                sNomeTratamento = "Sr. Maceiras",
                sSenha = "123456"
            };

            adminRepository.Inserir(adminInserir);

            //Ação
            adminRepository.Inserir(adminInserir2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Inserir_Admin_Com_Mesmo_Login_test()
        {
            //Ambiente
            var adminInserir2 = new Admin()
            {
                sEmail = "alexandro@maceiras.com.br",
                sLogin = adminInserir.sLogin,
                sNomeTratamento = "Sr. Maceiras",
                sSenha = "123456"
            };

            adminRepository.Inserir(adminInserir);

            //Ação
            adminRepository.Inserir(adminInserir2);
        }

        [TestMethod]
        public void Pode_Alterar_test()
        {
            //Ambiente
            var emailEsperado = adminInserir.sEmail;
            var loginEsperado = adminInserir.sLogin;
            var nomeTratamentoEsperado = adminInserir.sNomeTratamento;
            var senhaEsperada = adminInserir.sSenha;

            adminRepository.Inserir(adminInserir);

            var adminAlterar = (from a in adminRepository.Admins
                                where a.iAdminID == adminInserir.iAdminID
                                select a).FirstOrDefault();


            adminAlterar.sEmail = "ale@maceiras.com.br";
            adminAlterar.sLogin = "espanha";
            adminAlterar.sNomeTratamento = "Sr. Mac";
            adminAlterar.sSenha = "1234";

            //Ação
            adminRepository.Alterar(adminAlterar);

            var retorno = (from a in adminRepository.Admins
                           where a.iAdminID.Equals(adminInserir.iAdminID)
                           select a).FirstOrDefault();

            //Assertivas
            Assert.AreEqual(adminInserir.iAdminID, retorno.iAdminID);
            Assert.AreNotEqual(emailEsperado, retorno.sEmail);
            Assert.AreNotEqual(loginEsperado, retorno.sLogin);
            Assert.AreNotEqual(nomeTratamentoEsperado, retorno.sNomeTratamento);
            Assert.AreNotEqual(senhaEsperada, retorno.sSenha);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Alterar_Admin_Com_Mesmo_Email_Jah_Persistido_test()
        {
            //Ambiente
            adminRepository.Inserir(adminInserir);

            //Criando usuário que o adminAlterar irá conflitar
            var adminInserir2 = (new Admin()
            {
                sEmail = "ale@maceiras.com.br",
                sLogin = "espanha",
                sNomeTratamento = "Sr. Mac",
                sSenha = "1234"
            });
            
            adminRepository.Inserir(adminInserir2);

            var adminAlterar = (from a in adminRepository.Admins
                                where a.iAdminID == adminInserir.iAdminID
                                select a).FirstOrDefault();


            adminAlterar.sEmail = adminInserir2.sEmail;
            adminAlterar.sLogin = "espanhol";
            adminAlterar.sNomeTratamento = "Sr. Maceiras";
            adminAlterar.sSenha = "123456";

            //Ação
            adminRepository.Alterar(adminAlterar);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Alterar_Admin_Com_Mesmo_Login_Jah_Persistido_test()
        {
            //Ambiente
            adminRepository.Inserir(adminInserir);

            //Criando usuário que o adminAlterar irá conflitar
            var adminInserir2 = (new Admin()
            {
                sEmail = "ale@maceiras.com.br",
                sLogin = "espanha",
                sNomeTratamento = "Sr. Mac",
                sSenha = "1234"
            });

            adminRepository.Inserir(adminInserir2);

            var adminAlterar = (from a in adminRepository.Admins
                                where a.iAdminID == adminInserir.iAdminID
                                select a).FirstOrDefault();


            adminAlterar.sEmail = "ale@maceiras.com.br";
            adminAlterar.sLogin = adminInserir2.sLogin;
            adminAlterar.sNomeTratamento = "Sr. Mac";
            adminAlterar.sSenha = "1234";

            //Ação
            adminRepository.Alterar(adminAlterar);
        }

        [TestMethod]
        public void Pode_Excluir_Test()
        {
            //Ambiente
            adminRepository.Inserir(adminInserir);

            //Ação
            adminRepository.Excluir(adminInserir.iAdminID);

            //Assertivas
            var result = (from a in vestContext.Admins
                          where a.iAdminID.Equals(adminInserir.iAdminID)
                                     select a);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Caso_Id_Nao_Exista_Test()
        {
            //Ambiente

            //Ação
            adminRepository.Excluir(15);
        }

        [TestMethod]
        public void Pode_Recuperar_Por_Id_Test()
        {
            //Ambiente
            adminRepository.Inserir(adminInserir);

            //Ação
            var result = adminRepository.Retornar(adminInserir.iAdminID);

            //Assertivas
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Admin));
            Assert.AreEqual(adminInserir, result);
        }

        [TestCleanup]
        public void LimparCenario()
        {
            var adminsParaRemover = (from a in vestContext.Admins
                                     select a);


            /* -Na aula ele fez assim:
             * 
             * foreach (var admin in adminsParaRemover)
             * {
             *      vestContext.Admins.Remove(admin);
             * }
             * 
             * -Mas assim fica melhor:
             * 
             * vestContext.Admins.RemoveRange(adminsParaRemover); */

            vestContext.Admins.RemoveRange(adminsParaRemover);
            vestContext.SaveChanges();
        }
    }
}
