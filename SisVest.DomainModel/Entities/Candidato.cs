using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SisVest.DomainModel.Entities
{
    [Table("tbCandidato")]
    public class Candidato
    {
        [Key]
        public int iCandidatoId { get; set; }

        public string sNome { get; set; }
        public string sTelefone { get; set; }
        
        [Required(ErrorMessage = "Informe o email do candidato")]
        public string sEmail { get; set; }
        public DateTime dtNascimento { get; set; }
        public string sSenha { get; set; }
        public string sSexo { get; set; }
        public string sCpf { get; set; }

        [Required(ErrorMessage = "Informe o vestibular do candidato")]
        public virtual Vestibular Vestibular { get; set; }

        [Required(ErrorMessage = "Informa o curaso do candidato")]
        public virtual Curso Curso { get; set; }
        public bool bAprovado { get; set; }

        public override bool Equals(object obj)
        {
            var candidatoParam = (Candidato) obj;
            if (this.iCandidatoId == candidatoParam.iCandidatoId || this.sCpf == candidatoParam.sCpf || this.sEmail == candidatoParam.sEmail)
                return true;
            return false;
        }
    }
}
