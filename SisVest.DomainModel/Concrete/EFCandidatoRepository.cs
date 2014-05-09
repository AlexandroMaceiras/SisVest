using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SisVest.DomainModel.Abstract;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace SisVest.DomainModel.Concrete
{
    public class EFCandidatoRepository : ICandidatoRepository
    {
        private VestContext vestContext;

        public EFCandidatoRepository(VestContext context)
        {
            vestContext = context;
        }

        public IQueryable<Entities.Candidato> Candidatos
        {
            get { return vestContext.Candidatos.AsQueryable(); }
        }

        public void RealizarInscricao(Entities.Candidato candidato)
        {
            var result = vestContext.Candidatos.Where(c => c.sCpf.Equals(candidato.sCpf));

            if (result.Count() > 0)
                throw new InvalidOperationException("Já há um candidato usando este CPF!");

            var result2 = vestContext.Candidatos.Where(c => c.sEmail.Equals(candidato.sEmail));

            if (result2.Count() > 0)
                throw new InvalidOperationException("Já há um candidato usando este E-mail!");

            try
            {
                vestContext.Candidatos.Add(candidato);
                vestContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string msgErro = string.Empty;
                var erros = vestContext.GetValidationErrors();
                foreach (var erro in erros)
                {
                    foreach (var detalheErro in erro.ValidationErrors)
                    {
                        msgErro += detalheErro.ErrorMessage + "\n";
                    }
                }
                //Desatachar o candidato do contexto pra não dar erro na proxima vêz
                //Obs: Eu acho que istoi não é necessário!
                vestContext.Entry(candidato).State = EntityState.Detached;
                throw new InvalidOperationException(msgErro);

            }

        }

        public void AtualizarCadastro(Entities.Candidato candidato)
        {

            // Isto é para quando alterar o valor de algum campo e a entidade chegar aqui populada ele 
            //poder saber o que fazer com ela, ou seja modificar pra depois poder salvar.
            vestContext.Entry(candidato).State = EntityState.Modified;
            vestContext.SaveChanges();
        }

        public void Excluir(int idCandidato)
        {
            var candidato = vestContext.Candidatos.Where(c => c.iCandidatoId == idCandidato).FirstOrDefault();
            vestContext.Candidatos.Remove(candidato);
            vestContext.SaveChanges();
        }

        public void Aprovar(int idCandidato)
        {
            var candidato = vestContext.Candidatos.Where(c => c.iCandidatoId == idCandidato).FirstOrDefault();
            var totalVagasCurso = vestContext.Cursos.Where(c => c.iCursoID == candidato.Curso.iCursoID).Select(c => new { c.iVagas }).FirstOrDefault();

            var teste =
                vestContext.Cursos.Where(c => c.iCursoID == candidato.Curso.iCursoID).Select(c => c.iVagas).FirstOrDefault();



            var result = (from cur in vestContext.Cursos
                          from cand in cur.CandidatosList
                          where cur.iCursoID == candidato.Curso.iCursoID && cand.bAprovado
                          select cand).Count();

            if (result == totalVagasCurso.iVagas)
                throw new InvalidOperationException("O curso já está lotado e não pode receber aprovação");

            if (result == teste)
                throw new InvalidOperationException("O TESTE QUE EU FIZ BARROU! FERIFIQUE A DIFERENÇA DO TESTE ORIGINAL!");

            candidato.bAprovado = true;
            vestContext.SaveChanges();
        }

        public Entities.Candidato Retornar(int id)
        {
            return vestContext.Candidatos.Where(c => c.iCandidatoId == id).FirstOrDefault();
        }

        public IList<Entities.Candidato> RetornarTodos()
        {
            return vestContext.Candidatos.ToList();
        }

        public IList<Entities.Candidato> RetornarPorVestibularPorCurso(int iVestibularID, int iCursoID)
        {
            return
                vestContext.Candidatos.Where(
                c => c.Curso.iCursoID.Equals(iCursoID) && c.Vestibular.iVestibularId.Equals(iVestibularID)
                    ).ToList();
        }
    }
}
