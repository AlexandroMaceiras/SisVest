using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SisVest.DomainModel.Abstract;

namespace SisVest.WebUI.Models
{
    public class CursoModel
    {
        private ICursoRepository cursoRepository;

        public CursoModel(ICursoRepository repository)
        {
            cursoRepository = repository;
        }

        public int iCursoID { get; set; }

        public string sDescricao { get; set; }

        public int iVagas { get; set; }

        public int iTotalCandidatos { get; set; }

        public int iTotalCandidatosAprovados { get; set; }

        public IList<CursoModel> RetornarTodos()
        {
            //Todos os cursos
            var result = cursoRepository.Cursos.ToList();
            //Novo modelo para apresentar na tela uma lista de cursos com seus candidatos aprovados ou não.
            IList<CursoModel> cursoModelList = new List<CursoModel>();
            //Para cada curso
            foreach (var curso in result)
            {//Insere curso no Novo modelo de curso com os candidatos totais e candidatos aprovados já somados!
                cursoModelList.Add(new CursoModel(cursoRepository)
                {
                    iCursoID = curso.iCursoID,
                    iVagas = curso.iVagas,
                    sDescricao = curso.sDescricao,
                    iTotalCandidatos = curso.CandidatosList.Count,
                    iTotalCandidatosAprovados = cursoRepository.CandidatosAprovados(curso.iCursoID).Count()
                });
            }
            return cursoModelList;
        }

    }
}