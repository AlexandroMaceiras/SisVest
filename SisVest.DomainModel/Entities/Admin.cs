using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisVest.DomainModel.Entities
{
    [Table("tbAdmin")]
    public class Admin
    {
        [Key]
        public int iAdminID { get; set; }

        [Required]
        public string sLogin { get; set; }

        [Required]
        public string sSenha { get; set; }

        [Required]
        public string sNomeTratamento { get; set; }

        [Required]
        public string sEmail { get; set; }

        public override bool Equals(object obj)
        {
            var adminParam = (Admin) obj;
            if (this.iAdminID == adminParam.iAdminID || this.sLogin == adminParam.sLogin || this.sEmail == adminParam.sEmail)
                return true;
            return false;
        }
    }
}
