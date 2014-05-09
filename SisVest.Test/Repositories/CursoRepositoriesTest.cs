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
    public class CursoRepositoriesTest
    {
        private ICursoRepository cursoRepository;
        private ICandidatoRepository candidatoRepository;
        private VestContext vestContext = new VestContext();
        private Curso cursoInserir;
        private Vestibular vestibularInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
            cursoRepository = new EFCursoRepository(vestContext);
            cursoInserir = (new Curso()
            {
                sDescricao = "Computação",
                iVagas = 2
            });

            //Criando Vestibular
            vestibularInserir = (new Vestibular()
            {
                dtInicioInscricao = new DateTime(2012, 3, 25),
                dtFimInscricao = new DateTime(2012, 3, 25).AddDays(5),
                dtProva = new DateTime(2012, 3, 25).AddDays(7),
                sDescricao = "Vestibular 20121"
            });

            vestContext.Vestibulares.Add(vestibularInserir);
            vestContext.SaveChanges();

            candidatoRepository = new EFCandidatoRepository(vestContext);
        }

        [TestMethod]
        public void Pode_Consultar_LINQ_Usando_RepositorioTest()
        {
            //Ambiente
            vestContext.Cursos.Add(cursoInserir);
            vestContext.SaveChanges();


            //Ação
            var cursos = cursoRepository.Cursos;

            var retorno = (from c in cursos
                           where c.sDescricao.Equals(cursoInserir.sDescricao)
                           select c).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(cursos, typeof(IQueryable<Curso>));
            Assert.AreEqual(retorno, cursoInserir);
        }

        [TestMethod]
        public void Pode_Inserir_test()
        {
            //Ambiente


            //Ação
            cursoRepository.Inserir(cursoInserir);

            var retorno = (from c in cursoRepository.Cursos
                           where c.iCursoID.Equals(cursoInserir.iCursoID)
                           select c).FirstOrDefault();

            //Assertivas
            Assert.AreEqual(retorno, cursoInserir);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Inserir_Curso_Com_Mesma_Descricao_test()
        {
            //Ambiente
            var cursoInserir2 = new Curso()
            {
                sDescricao = "Computação"
            };

            cursoRepository.Inserir(cursoInserir);

            //Ação
            cursoRepository.Inserir(cursoInserir2);
        }

        [TestMethod]
        public void Pode_Alterar_test()
        {
            //Ambiente
            var descricaoEsperado = cursoInserir.sDescricao;

            cursoRepository.Inserir(cursoInserir);

            var cursoAlterar = (from c in cursoRepository.Cursos
                                where c.iCursoID == cursoInserir.iCursoID
                                select c).FirstOrDefault();

            cursoAlterar.sDescricao = "Matemática";

            //Ação
            cursoRepository.Alterar(cursoAlterar);

            var retorno = (from c in cursoRepository.Cursos
                           where c.iCursoID.Equals(cursoInserir.iCursoID)
                           select c).FirstOrDefault();

            //Assertivas
            Assert.AreEqual(cursoInserir.iCursoID, retorno.iCursoID);
            Assert.AreNotEqual(descricaoEsperado, retorno.sDescricao);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Alterar_Curso_Com_Mesmo_Descricao_Jah_Persistido_test()
        {
            //Ambiente
            cursoRepository.Inserir(cursoInserir);

            //Criando curso que o cursoAlterar irá conflitar
            var cursoInserir2 = (new Curso()
            {
                sDescricao = "Matemática"
            });

            cursoRepository.Inserir(cursoInserir2);

            var cursoAlterar = (from c in cursoRepository.Cursos
                                where c.iCursoID == cursoInserir.iCursoID
                                select c).FirstOrDefault();


            cursoAlterar.sDescricao = cursoInserir2.sDescricao;

            //Ação
            cursoRepository.Alterar(cursoAlterar);
        }

        [TestMethod]
        public void Pode_Excluir_Test()
        {
            //Ambiente
            cursoRepository.Inserir(cursoInserir);

            //Ação
            cursoRepository.Excluir(cursoInserir.iCursoID);

            //Assertivas
            var result = (from c in vestContext.Cursos
                          where c.iCursoID.Equals(cursoInserir.iCursoID)
                          select c);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Caso_Id_Nao_Exista_Test()
        {
            //Ambiente

            //Ação
            cursoRepository.Excluir(15);
        }

        [TestMethod]
        public void Pode_Recuperar_Por_Id_Test()
        {
            //Ambiente
            cursoRepository.Inserir(cursoInserir);

            //Ação
            var result = cursoRepository.RetornarPorId(cursoInserir.iCursoID);

            //Assertivas
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Curso));
            Assert.AreEqual(cursoInserir, result);
        }

        [TestMethod]
        public void Pode_Retornar_Candidatos_Aprovados_test()
        {
            //Ambiente
            cursoRepository.Inserir(cursoInserir);

            //Criando 1º candidato
            var candidatoInserir = new Candidato()
            {
                Curso = cursoInserir,
                dtNascimento = new DateTime(1994, 5, 5),
                sCpf = "12345678922",
                sEmail = "candidato2@devmedia.com.br",
                sSenha = "123456",
                sSexo = "M",
                sTelefone = "8533221155",
                sNome = "Marcelo",
                Vestibular = vestibularInserir
            };

            //Criando 2º candidato
            var candidatoInserir2 = new Candidato()
            {
                Curso = cursoInserir,
                dtNascimento = new DateTime(1994, 5, 5),
                sCpf = "12345678923",
                sEmail = "candidato3@devmedia.com.br",
                sSenha = "123456",
                sSexo = "M",
                sTelefone = "8533221155",
                sNome = "Joao",
                Vestibular = vestibularInserir
            };

            //Realizar inscrição dos Candidatos
            candidatoRepository.RealizarInscricao(candidatoInserir);
            candidatoRepository.RealizarInscricao(candidatoInserir2);          
            candidatoRepository.Aprovar(candidatoInserir.iCandidatoId);
            candidatoRepository.Aprovar(candidatoInserir2.iCandidatoId);
            
            //Ação
            
            //Recuperndo lista de candidatos aprovados
            var candidatosAprovados = cursoRepository.CandidatosAprovados(cursoInserir.iCursoID);

            //Assertivas
            Assert.AreEqual(2, candidatosAprovados.Count());
            Assert.IsTrue(candidatosAprovados.ToList().Contains(candidatoInserir));
            Assert.IsTrue(candidatosAprovados.ToList().Contains(candidatoInserir2));
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
