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
    public class CandidatoRepositoriesTest
    {
        private ICandidatoRepository candidatoRepository;
        private VestContext vestContext = new VestContext();
        private Candidato candidatoInserir;
        private Vestibular vestibularInserir;
        private Curso cursoInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
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

            //Criando Curso
            cursoInserir = (new Curso()
            {
                sDescricao = "Computação",
                iVagas = 2
            });

            vestContext.Cursos.Add(cursoInserir);
            vestContext.SaveChanges();

            candidatoRepository = new EFCandidatoRepository(vestContext);
            candidatoInserir = new Candidato()
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
        }

        [TestMethod]
        public void Pode_Consultar_LINQ_Usando_RepositorioTest()
        {
            //Ambiente
            vestContext.Candidatos.Add(candidatoInserir);
            vestContext.SaveChanges();


            //Ação
            var candidatos = candidatoRepository.Candidatos;

            var retorno = (from c in candidatos
                           where c.iCandidatoId.Equals(candidatoInserir.iCandidatoId)
                           select c).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(candidatos, typeof(IQueryable<Candidato>));
            Assert.AreEqual(retorno, candidatoInserir);
        }

        [TestMethod]
        public void Pode_Realizar_Inscricao_Candidato_Test()
        {
            //Ambiente

            //Ação
            candidatoRepository.RealizarInscricao(candidatoInserir);

            //Assertivas
            var retorno = (from c in candidatoRepository.Candidatos
                           where c.iCandidatoId.Equals(candidatoInserir.iCandidatoId)
                           select c).FirstOrDefault();

            Assert.AreEqual(retorno, candidatoInserir);
            Assert.AreEqual(vestibularInserir, retorno.Vestibular);
            Assert.AreEqual(cursoInserir, retorno.Curso);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Sem_Email_test()
        {
            //Ambiente
            candidatoInserir.sEmail = null;
            //Ação
            candidatoRepository.RealizarInscricao(candidatoInserir);
            //Assertivas
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Com_Cpf_Jah_Existente_test()
        {
            //Ambiente
            candidatoRepository.RealizarInscricao(candidatoInserir);

            //Inserindo Segundo candidato
            var candidatoInserir2 = new Candidato()
            {
                Curso = cursoInserir,
                dtNascimento = new DateTime(1994, 5, 5),
                sCpf = candidatoInserir.sCpf,
                sEmail = "candidato@devmedia.com.br",
                sSenha = "123456",
                sSexo = "M",
                sTelefone = "8533221155",
                sNome = "Marcelo",
                Vestibular = vestibularInserir
            };

            //Ação
            candidatoRepository.RealizarInscricao(candidatoInserir2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Com_Email_Jah_Existente_test()
        {
            //Ambiente
            candidatoRepository.RealizarInscricao(candidatoInserir);

            //Inserindo Segundo candidato
            var candidatoInserir2 = new Candidato()
            {
                Curso = cursoInserir,
                dtNascimento = new DateTime(1994, 5, 5),
                sCpf = candidatoInserir.sCpf,
                sEmail = candidatoInserir.sEmail,
                sSenha = "123456",
                sSexo = "M",
                sTelefone = "8533221155",
                sNome = "Marcelo",
                Vestibular = vestibularInserir
            };

            //Ação
            candidatoRepository.RealizarInscricao(candidatoInserir2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Sem_Curso_test()
        {
            //Ambiente
            candidatoInserir.Curso = null;
            //Ação
            candidatoRepository.RealizarInscricao(candidatoInserir);
            //Assertivas
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Sem_Vestibular_test()
        {
            //Ambiente
            candidatoInserir.Vestibular = null;
            //Ação
            candidatoRepository.RealizarInscricao(candidatoInserir);
            //Assertivas
        }

        [TestMethod]
        public void Pode_Atualizar_Cadastro_Candidato_Test()
        {
            //Ambiente
            var emailEsperado = candidatoInserir.sEmail;

            candidatoRepository.RealizarInscricao(candidatoInserir);

            var candidatoAtualizarCadastro = (from c in candidatoRepository.Candidatos
                                              where c.iCandidatoId == candidatoInserir.iCandidatoId
                                              select c).FirstOrDefault();
            
            candidatoAtualizarCadastro.sEmail = "candidatoatualizado@devmedia.com.br";

            //Ação
            candidatoRepository.AtualizarCadastro(candidatoAtualizarCadastro);

            var retorno =  (from c in candidatoRepository.Candidatos
                            where c.iCandidatoId.Equals(candidatoInserir.iCandidatoId)
                            select c).FirstOrDefault();
            
            //Assertivas
            Assert.AreEqual(candidatoInserir.iCandidatoId, retorno.iCandidatoId);
            Assert.AreNotEqual(emailEsperado, retorno.sEmail);
        }

        [TestMethod]
        public void Pode_Excluir_Candidato_Test()
        {
            //Ambiente
            candidatoRepository.RealizarInscricao(candidatoInserir);

            //Ação
            candidatoRepository.Excluir(candidatoInserir.iCandidatoId);

            //Assertivas
            var retorno = from c in vestContext.Candidatos
                          where c.iCandidatoId.Equals(candidatoInserir.iCandidatoId)
                          select c;

            Assert.AreEqual(0, retorno.Count());

        }

        [TestMethod]
        public void Pode_Aprovar_Candidato_Test()
        {
            //Ambiente

            //Modificando curso para que ele tenha 3 vagas
            cursoInserir.iVagas = 3;
            vestContext.SaveChanges();

            //Inserindo 2º candidato
            var candidatoInserir2 = new Candidato()
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

            //Inserindo 3º candidato
            var candidatoInserir3 = new Candidato()
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
            candidatoRepository.RealizarInscricao(candidatoInserir3);

            //Ação
            candidatoRepository.Aprovar(candidatoInserir.iCandidatoId);
            candidatoRepository.Aprovar(candidatoInserir2.iCandidatoId);
            candidatoRepository.Aprovar(candidatoInserir3.iCandidatoId);

            //Assertivas
            var retorno = (from cur in vestContext.Cursos
                           from cand in cur.CandidatosList
                           where cur.iCursoID.Equals(cursoInserir.iCursoID)
                           && cand.bAprovado
                           select cand);

            Assert.AreEqual(3, retorno.Count());
            Assert.IsTrue(retorno.ToList().Contains(candidatoInserir));
            Assert.IsTrue(retorno.ToList().Contains(candidatoInserir2));
            Assert.IsTrue(retorno.ToList().Contains(candidatoInserir3));

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Aprovar_Candidato_Em_Curso_Sem_Vaga_Test()
        {
            //Ambiente

            //Modificando curso para que ele tenha 2 vagas
            cursoInserir.iVagas = 2;

            vestContext.SaveChanges();

            //Inserindo 2º candidato
            var candidatoInserir2 = new Candidato()
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

            //Inserindo 3º candidato
            var candidatoInserir3 = new Candidato()
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
            candidatoRepository.RealizarInscricao(candidatoInserir3);

            candidatoRepository.Aprovar(candidatoInserir.iCandidatoId);
            candidatoRepository.Aprovar(candidatoInserir2.iCandidatoId);

            //Ação
            //Ao realizar a aprovação do 3º candidato deve levantar exceção por não haver vaga.
            candidatoRepository.Aprovar(candidatoInserir3.iCandidatoId);

        }

        [TestMethod]
        public void Pode_Retornar_Candidato_Por_Id_Test()
        {
            //Ambiente
            candidatoRepository.RealizarInscricao(candidatoInserir);

            //Ação
            var retorno = candidatoRepository.Retornar(candidatoInserir.iCandidatoId);

            //Assertivas
            Assert.IsNotNull(retorno);
            Assert.IsInstanceOfType(retorno, typeof(Candidato));
            Assert.AreEqual(candidatoInserir, retorno);
        }

        [TestMethod]
        public void Pode_Retornar_Todos_Os_Candidatos_Test()
        {
            //Ambiente

            //Inserindo 2º candidato
            var candidatoInserir2 = new Candidato()
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

            //Inserindo 3º candidato
            var candidatoInserir3 = new Candidato()
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
            candidatoRepository.RealizarInscricao(candidatoInserir3);

            //Ação
            var retorno = candidatoRepository.RetornarTodos();

            Assert.AreEqual(3, retorno.Count());
            Assert.IsTrue(retorno.ToList().Contains(candidatoInserir));
            Assert.IsTrue(retorno.ToList().Contains(candidatoInserir2));
            Assert.IsTrue(retorno.ToList().Contains(candidatoInserir3));
        }

        [TestMethod]
        public void Pode_Retornar_Candidatos_Por_Vestibular_Por_Curso_Test()
        {
            //Ambiente

            //Inserindo 2º candidato
            var candidatoInserir2 = new Candidato()
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

            //Inserindo 3º candidato
            var candidatoInserir3 = new Candidato()
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
            candidatoRepository.RealizarInscricao(candidatoInserir3);

            //Ação
            var retorno = candidatoRepository.RetornarPorVestibularPorCurso(vestibularInserir.iVestibularId, cursoInserir.iCursoID);

            Assert.AreEqual(3, retorno.Count());
            Assert.IsTrue(retorno.ToList().Contains(candidatoInserir));
            Assert.IsTrue(retorno.ToList().Contains(candidatoInserir2));
            Assert.IsTrue(retorno.ToList().Contains(candidatoInserir3));
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
