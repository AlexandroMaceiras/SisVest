using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SisVest.DomainModel.Entities;

namespace SisVest.DomainModel.Abstract
{
    public interface ICursoRepository
    {
        IQueryable<Curso> Cursos { get; }
        void Inserir(Curso curso);
        void Alterar(Curso curso);
        void Excluir(int iCursoID);
        Curso RetornarPorId(int iCursoID);

        IQueryable<Candidato> CandidatosAprovados(int iCursoID);
    }
}
