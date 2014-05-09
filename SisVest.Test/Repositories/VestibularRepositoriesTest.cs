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
    public class VestibularRepositoriesTest
    {
        private IVestibularRepository vestibularRepository;
        private VestContext vestContext = new VestContext();
        private Vestibular vestibularInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
            vestibularRepository = new EFVestibularRepository(vestContext);
            vestibularInserir = (new Vestibular()
            {
                dtInicioInscricao = new DateTime(2012,3,25),
                dtFimInscricao = new DateTime(2012,3,25).AddDays(5),
                dtProva = new DateTime(2012,3,25).AddDays(7),
                sDescricao = "Vestibular 20121"
            });
        }

        [TestMethod]
        public void Pode_Consultar_LINQ_Usando_RepositorioTest()
        {
            //Ambiente
            vestContext.Vestibulares.Add(vestibularInserir);
            vestContext.SaveChanges();


            //Ação
            var vestibulares = vestibularRepository.Vestibulares;

            var retorno = (from v in vestibulares
                           where v.iVestibularId.Equals(vestibularInserir.iVestibularId)
                           select v).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(vestibulares, typeof(IQueryable<Vestibular>));
            Assert.AreEqual(retorno, vestibularInserir);
        }

        [TestMethod]
        public void Pode_Inserir_test()
        {
            //Ambiente


            //Ação
            vestibularRepository.Inserir(vestibularInserir);

            var retorno = (from v in vestibularRepository.Vestibulares
                           where v.iVestibularId.Equals(vestibularInserir.iVestibularId)
                           select v).FirstOrDefault();

            //Assertivas
            Assert.AreEqual(retorno, vestibularInserir);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Inserir_Vestibular_Com_Mesma_Descricao_test()
        {
            //Ambiente
            var vestibularInserir2 = new Vestibular()
            {
                dtInicioInscricao = new DateTime(2012, 3, 25),
                dtFimInscricao = new DateTime(2012, 3, 25).AddDays(5),
                dtProva = new DateTime(2012, 3, 25).AddDays(7),
                sDescricao = vestibularInserir.sDescricao
            };

            vestibularRepository.Inserir(vestibularInserir);

            //Ação
            vestibularRepository.Inserir(vestibularInserir2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Inserir_Vestibular_Sem_Informar_As_Datas_test()
        {
            //Ambiente
            var vestibularInserir2 = new Vestibular()
            {
                sDescricao = "Vestibular 20122"
            };
            
            //Ação
            vestibularRepository.Inserir(vestibularInserir2);
        }

        [TestMethod]
        public void Pode_Alterar_test()
        {
            //Ambiente
            var descricaoEsperada = vestibularInserir.sDescricao;

            vestibularRepository.Inserir(vestibularInserir);

            var vestibularAlterar = (from v in vestibularRepository.Vestibulares
                                where v.iVestibularId == vestibularInserir.iVestibularId
                                select v).FirstOrDefault();

            vestibularAlterar.sDescricao = "Vestibular 20122";

            //Ação
            vestibularRepository.Alterar(vestibularAlterar);

            var retorno = (from v in vestibularRepository.Vestibulares
                           where v.iVestibularId.Equals(vestibularInserir.iVestibularId)
                           select v).FirstOrDefault();

            //Assertivas
            Assert.AreEqual(vestibularInserir.iVestibularId, retorno.iVestibularId);
            Assert.AreNotEqual(descricaoEsperada, retorno.sDescricao);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Alterar_Vestibular_Com_Mesmo_Descricao_Jah_Persistido_test()
        {
            //Ambiente
            vestibularRepository.Inserir(vestibularInserir);

            //Criando curso que o cursoAlterar irá conflitar
            var vestibularInserir2 = (new Vestibular()
            {
                sDescricao = "Vestibular 20122",
                dtInicioInscricao = new DateTime(2012, 9, 25),
                dtFimInscricao = new DateTime(2012, 8, 25).AddDays(5),
                dtProva = new DateTime(2012, 9, 25).AddDays(7)
            });

            vestibularRepository.Inserir(vestibularInserir2);

            var vestibularAlterar = (from v in vestibularRepository.Vestibulares
                                where v.iVestibularId == vestibularInserir.iVestibularId
                                select v).FirstOrDefault();


            vestibularAlterar.sDescricao = vestibularInserir2.sDescricao;

            //Ação
            vestibularRepository.Alterar(vestibularAlterar);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Pode_Excluir_Test()
        {
            //Ambiente
            vestibularRepository.Inserir(vestibularInserir);

            //Ação
            vestibularRepository.Excluir(vestibularInserir.iVestibularId);

            //Assertivas
            var result = (from v in vestContext.Vestibulares
                          where v.iVestibularId.Equals(vestibularInserir.iVestibularId)
                          select v);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Caso_Id_Nao_Exista_Test()
        {
            //Ambiente

            //Ação
            vestibularRepository.Excluir(15);
        }

        [TestMethod]
        public void Pode_Retornar_Candidatos_Por_Vestibular_test()
        {
            //Ambiente

            //Criando vestibular
            vestContext.Vestibulares.Add(vestibularInserir);
            //Criando Curso
            var cursoInserir = new Curso()
            {
                sDescricao = "Computação"
            };
            vestContext.Cursos.Add(cursoInserir);
            vestContext.SaveChanges();

            //Criando Candidatos
            var candidatoInserir = new Candidato()
            {
                Curso = cursoInserir,
                dtNascimento = new DateTime(1994,5,5),
                sCpf = "12345678921",
                sEmail = "candidato@devmedia.com.br",
                sSenha = "123456",
                sSexo = "M",
                sTelefone = "8533221155",
                sNome = "Joao",
                Vestibular = vestibularInserir
            };
            vestContext.Candidatos.Add(candidatoInserir);
            vestContext.SaveChanges();

            var candidatoInserir2 = new Candidato()
            {
                Curso = cursoInserir,
                dtNascimento = new DateTime(1994, 5, 5),
                sCpf = "12345678921",
                sEmail = "candidato@devmedia.com.br",
                sSenha = "123456",
                sSexo = "M",
                sTelefone = "8533221155",
                sNome = "Pedro",
                Vestibular = vestibularInserir
            };
            vestContext.Candidatos.Add(candidatoInserir2);
            vestContext.SaveChanges();

            //Ação
            var candidatos = vestibularRepository.RetornarCandidatosPorVestibular(vestibularInserir.iVestibularId);

            //Assertivas
            Assert.AreEqual(2, candidatos.Count());
            Assert.IsTrue(candidatos.Contains(candidatoInserir));
            Assert.IsTrue(candidatos.Contains(candidatoInserir2));

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Caso_Exista_Algum_Candidato_Inscrito_test()
        {
            //Ambiente

            //Criando vestibular
            vestContext.Vestibulares.Add(vestibularInserir);
            //Criando Curso
            var cursoInserir = new Curso()
            {
                sDescricao = "Computação"
            };
            vestContext.Cursos.Add(cursoInserir);
            vestContext.SaveChanges();

            //Criando Candidatos
            var candidatoInserir = new Candidato()
            {
                Curso = cursoInserir,
                dtNascimento = new DateTime(1994, 5, 5),
                sCpf = "12345678921",
                sEmail = "candidato@devmedia.com.br",
                sSenha = "123456",
                sSexo = "M",
                sTelefone = "8533221155",
                sNome = "Joao",
                Vestibular = vestibularInserir
            };
            vestContext.Candidatos.Add(candidatoInserir);
            vestContext.SaveChanges();

            //Ação
            vestibularRepository.Excluir(vestibularInserir.iVestibularId);
        }

        [TestCleanup]
        public void LimparCenario()
        {
            var candidatosParaRemover = (from cand in vestContext.Candidatos
                                           select cand);

            vestContext.Candidatos.RemoveRange(candidatosParaRemover);
            vestContext.SaveChanges();

            var cursossParaRemover = (from c in vestContext.Cursos
                                           select c);

            vestContext.Cursos.RemoveRange(cursossParaRemover);
            vestContext.SaveChanges();

            var vestibularesParaRemover = (from v in vestContext.Vestibulares
                                           select v);

            vestContext.Vestibulares.RemoveRange(vestibularesParaRemover);
            vestContext.SaveChanges();

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
        }
    }
}
