using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SisVest.DomainModel.Entities
{
    [Table("tbVestibular")]
    public class Vestibular
    {
        [Key]
        [HiddenInput(DisplayValue=false)]
        public int iVestibularId { get; set; }

        [Required(ErrorMessage = "A Descrição é obrigatória.")]
        [Display(Name = "Descrição")]
        public string sDescricao { get; set; }

        [Required(ErrorMessage = "Data de Início da Inscrição é obrigatório.")]
        [Display(Name = "Início da Inscrição")]
        public DateTime? dtInicioInscricao { get; set; }

        [Required(ErrorMessage = "Data de Fim da Inscrição é obrigatório.")]
        [Display(Name = "Fim da Inscrição")]
        public DateTime? dtFimInscricao { get; set; }

        [Required(ErrorMessage = "Data da Prova é obrigatório.")]
        [Display(Name = "Data da Prova")]
        public DateTime? dtProva { get; set; }

        public virtual ICollection<Candidato> CandidatosList { get; set; }

        public override bool Equals(object obj)
        {
            var vestParam = (Vestibular)obj;
            if (this.iVestibularId == vestParam.iVestibularId || this.sDescricao == vestParam.sDescricao)
                return true;
            return false;
        }
    }
}
