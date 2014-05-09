using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SisVest.DomainModel.Abstract;
using System.Data.Entity;

namespace SisVest.DomainModel.Concrete
{
    public class EFCursoRepository : ICursoRepository
    {
        private VestContext vestContext;

        public EFCursoRepository(VestContext context)
        {
            vestContext = context;
        }

        public IQueryable<Entities.Curso> Cursos
        {
            get { return vestContext.Cursos.AsQueryable(); }
        }

        public void Inserir(Entities.Curso curso)
        {
            var result = from c in Cursos
                         where c.sDescricao.ToUpper().Equals(curso.sDescricao)
                         select c;

            if (result.Count() > 0)
                throw new InvalidOperationException("Já existe um Curso cadastrado com essa descrição!");

            vestContext.Cursos.Add(curso);
            vestContext.SaveChanges();
        }

        public void Alterar(Entities.Curso curso)
        {
            var result = from c in Cursos
                         where (c.sDescricao.ToUpper().Equals(curso.sDescricao)
                         && !c.iCursoID.Equals(curso.iCursoID))
                         select c;

            if (result.Count() > 0)
                throw new InvalidOperationException("Já existe um Curso cadastrado com essa descrição!");

            // Isto é para quando alterar o valor de algum campo e a entidade chegar aqui populada ele 
            //poder saber o que fazer com ela, ou seja modificar pra depois poder salvar.
            vestContext.Entry(curso).State = EntityState.Modified;
            vestContext.SaveChanges();
        }

        public void Excluir(int iCursoID)
        {
            var result = from c in Cursos
                         where c.iCursoID.Equals(iCursoID)
                         select c;

            if (result.Count() == 0)
                throw new InvalidOperationException("Curso não localizado no repositório!");

            vestContext.Cursos.Remove(result.FirstOrDefault());
            vestContext.SaveChanges();
        }

        public Entities.Curso RetornarPorId(int iCursoID)
        {
            return Cursos.Where(c => c.iCursoID.Equals(iCursoID)).FirstOrDefault();
        }


        public IQueryable<Entities.Candidato> CandidatosAprovados(int iCursoID)
        {
            var result = from cur in vestContext.Cursos
                         from cand in cur.CandidatosList
                         where cur.iCursoID == iCursoID && cand.bAprovado
                         select cand;
            return result;
        }
    }
}
