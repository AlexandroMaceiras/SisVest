using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisVest.WebUI.Models;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;
using Ninject;

namespace SisVest.WebUI.Controllers
{
    public class AutenticacaoController : Controller
    {
        [Inject]
        public IAutenticacaoProvider autenticacaoProvider { get; set; }

        //private IAutenticacaoProvider autenticacaoProvider;

        //public AutenticacaoController(IAutenticacaoProvider autenticacaoProviderParam)
        //{
        //    autenticacaoProvider = autenticacaoProviderParam;
        //}

        public ActionResult Entrar()
        {
            ViewBag.Autenticado = autenticacaoProvider.Autenticado;

            return View(autenticacaoProvider.UsuarioAutenticado);
        }

        public ActionResult Sair()
        {
            autenticacaoProvider.Desautenticar();

            return RedirectToAction("Entrar");
        }

        [HttpPost]
        public ActionResult Entrar(AutenticacaoModel autenticacaoModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                //autenticacaoProvider.Desautenticar();

                string msgErro;
                if (autenticacaoProvider.Autenticar(autenticacaoModel, out msgErro, "administrador"))
                {
                    return Redirect(returnUrl ?? Url.Action("Entrar", "Autenticacao"));
                }
                ModelState.AddModelError("", msgErro);
                ViewBag.Autenticado = false;
            }
            return View();
        }

    }
}
