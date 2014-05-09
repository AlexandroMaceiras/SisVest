using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SisVest.DomainModel.Abstract;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace SisVest.DomainModel.Concrete
{
    public class EFVestibularRepository : IVestibularRepository
    {
        private VestContext vestContext;

        public EFVestibularRepository(VestContext context)
        {
            vestContext = context;
        }

        public IQueryable<Entities.Vestibular> Vestibulares
        {
            get { return vestContext.Vestibulares.AsQueryable(); }
        }

        public void Inserir(Entities.Vestibular vestibular)
        {
            var result = from v in Vestibulares
                         where v.sDescricao.ToUpper().Equals(vestibular.sDescricao)
                         select v;

            if (result.Count() > 0)
                throw new InvalidOperationException("Já existe um Vestibular cadastrado com essa descrição!");

            try
            {
                vestContext.Vestibulares.Add(vestibular);
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
                vestContext.Entry(vestibular).State = EntityState.Detached;
                throw new InvalidOperationException(msgErro);
            }
        }

        public void Alterar(Entities.Vestibular vestibular)
        {
            var result = from v in Vestibulares
                         where (v.sDescricao.ToUpper().Equals(vestibular.sDescricao)
                         && !v.iVestibularId.Equals(vestibular.iVestibularId))
                         select v;

            if (result.Count() > 0)
                throw new InvalidOperationException("Já existe um Vestibular cadastrado com essa descrição!");

            // Isto é para quando alterar o valor de algum campo e a entidade chegar aqui populada ele 
            //poder saber o que fazer com ela, ou seja modificar pra depois poder salvar.
            vestContext.Entry(vestibular).State = EntityState.Modified;
            vestContext.SaveChanges();
        }

        public void Excluir(int iVestibularID)
        {
            var result = from v in Vestibulares
                         where v.iVestibularId.Equals(iVestibularID)
                         select v;

            if (result.Count() == 0)
                throw new InvalidOperationException("Vestibular não localizado no repositório!");

            var result2 = from v in Vestibulares
                          from c in v.CandidatosList
                         where v.iVestibularId.Equals(iVestibularID)
                         select c;

            if (result2.Count() > 0)
                throw new InvalidOperationException("Há candidatos inscritos nesse vestibular!");

            vestContext.Vestibulares.Remove(result.FirstOrDefault());
            vestContext.SaveChanges();
        }

        public IList<Entities.Candidato> RetornarCandidatosPorVestibular(int iVestibularID)
        {
            var result  = from v in Vestibulares
                          from c in v.CandidatosList
                          where v.iVestibularId.Equals(iVestibularID)
                          select c;

            if (result.Count() > 0)
                return result.ToList();
            return null;
        }
    }
}
