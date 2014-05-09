using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SisVest.WebUI.Models
{
    public class AutenticacaoModel
    {
        [Required(ErrorMessage = "Login é obrigatório.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Grupo { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string NomeTratamento { get; set; }
    }
}