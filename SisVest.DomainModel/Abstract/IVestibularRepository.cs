using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SisVest.DomainModel.Entities;

namespace SisVest.DomainModel.Abstract
{
    public interface IVestibularRepository
    {
        IQueryable<Vestibular> Vestibulares { get; }
        void Inserir(Vestibular vestibular);
        void Alterar(Vestibular vestibular);
        void Excluir(int iVestibular);
        IList<Candidato> RetornarCandidatosPorVestibular(int iVestibular);
    }
}
