using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SisVest.DomainModel.Entities
{
    [Table("tbCurso")]
    public class Curso
    {
        [Key]
        [HiddenInput(DisplayValue=false)]
        public int iCursoID { get; set; }

        [Required(ErrorMessage="A descrição é obrigatória.")]
        [Display(Name = "Descrição")]
        public string sDescricao { get; set; }

        [Required(ErrorMessage="O número de vagas é obrigatório.")]
        [Display(Name = "Vagas")]
        [Range(1,500,ErrorMessage="Informe o número de vagas de 1 a 500.")]
        public int iVagas { get; set; }

        public virtual ICollection<Candidato> CandidatosList { get; set; }

        public override bool Equals(object obj)
        {
            var cursoParam = (Curso) obj;
            if (this.iCursoID == cursoParam.iCursoID || this.sDescricao == cursoParam.sDescricao)
                return true;
            return false;
        }
    }
}
