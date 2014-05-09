using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SisVest.DomainModel.Entities;

namespace SisVest.DomainModel.Abstract
{
    public interface ICandidatoRepository
    {
        IQueryable<Candidato> Candidatos { get; }
        void RealizarInscricao(Candidato candidato);
        void AtualizarCadastro(Candidato candidato);
        void Excluir(int idCandidato);
        void Aprovar(int idCandidato);
        Candidato Retornar(int id);
        IList<Candidato> RetornarTodos();
        IList<Candidato> RetornarPorVestibularPorCurso(int iVestibularID, int iCursoID);

    }
}
