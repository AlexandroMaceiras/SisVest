using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;
using Ninject;

namespace SisVest.WebUI.Infraestrutura.Filter
{
    public class CustomAutenticacaoAttribute : AuthorizeAttribute
    {
        [Inject]
        public IAutenticacaoProvider autenticacaoProvider { get; set; }

        private string grupoEscolhido;

        public CustomAutenticacaoAttribute(string grupo)
        {
            grupoEscolhido = grupo;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!(autenticacaoProvider.Autenticado && autenticacaoProvider.UsuarioAutenticado.Grupo == grupoEscolhido))
                return false;
            return true;
        }
    }
}