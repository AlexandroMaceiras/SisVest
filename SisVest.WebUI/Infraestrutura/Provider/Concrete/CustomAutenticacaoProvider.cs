using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;
using SisVest.DomainModel.Abstract;
using SisVest.WebUI.Models;

namespace SisVest.WebUI.Infraestrutura.Provider.Concrete
{
    public class CustomAutenticacaoProvider : IAutenticacaoProvider
    {
        private IAdminRepository adminRepository;

        public CustomAutenticacaoProvider(IAdminRepository repository)
        {
            adminRepository = repository;
        }

        public bool Autenticar(AutenticacaoModel autenticacaoModel, out string msgErro, string grupo = "administrador")
        {
            msgErro = string.Empty;

            var usuario = adminRepository.Admins.Where(a => a.sLogin == autenticacaoModel.Login).FirstOrDefault();

            if (usuario == null)
            {
                msgErro = "Login não pertence a nenhum usuário.";
                return false;
            }

            if (usuario.sSenha != autenticacaoModel.Senha)
            {
                msgErro = "Senha incorreta.";
                return false;
            }
            HttpContext.Current.Session["autenticacao"] = new AutenticacaoModel
            {
                Grupo = grupo,
                Login = autenticacaoModel.Login,
                Senha = autenticacaoModel.Senha,
                NomeTratamento = usuario.sNomeTratamento

            };
            return true;
        }

        public void Desautenticar()
        {
            HttpContext.Current.Session.Remove("autenticacao");
        }

        public bool Autenticado
        {
            get
            {
                //bool nulo = HttpContext.Current.Session["autenticacao"] != null;
                //bool tipo = HttpContext.Current.Session["autenticacao"].GetType() == typeof(AutenticacaoModel);
                //return nulo && tipo;

                return HttpContext.Current.Session["autenticacao"] != null && HttpContext.Current.Session["autenticacao"].GetType() == typeof(AutenticacaoModel);
            }
        }

        public AutenticacaoModel UsuarioAutenticado
        {
            get
            {
                if (Autenticado)
                    return (AutenticacaoModel)HttpContext.Current.Session["autenticacao"];
                return null;
            }
        }
    }
}