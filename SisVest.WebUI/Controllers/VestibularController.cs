using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Entities;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;

namespace SisVest.WebUI.Controllers
{
    //[Authorize]
    public class VestibularController : Controller
    {
        private IVestibularRepository repository;
        private IAutenticacaoProvider autenticacaoProvider;

        /// <summary>
        /// O construtor instancia e assim faz a inversão de controle para ser usado em todas as ações.
        /// </summary>
        /// <param name="cursoRepository"></param>
        public VestibularController(IVestibularRepository vestibularRepository, IAutenticacaoProvider autenticacaoProviderParam)
        {
            repository = vestibularRepository;
            autenticacaoProvider = autenticacaoProviderParam;
        }

        //[Authorize] Pode ser colocado aqui barrando só esta action 
        //ou direto lá em cima barrando todo o controller
        public ActionResult Index()
        {
            if (!(autenticacaoProvider.Autenticado && autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
                HttpContext.Response.StatusCode = 401;
            return View(repository.Vestibulares.ToList());
        }

        //[Authorize]
        public ActionResult Alterar(int id)
        {
            if (!(autenticacaoProvider.Autenticado && autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
                HttpContext.Response.StatusCode = 401;
            return View(repository.Vestibulares.Where(v => v.iVestibularId == id).FirstOrDefault());
        }
        
        [HttpPost]
        public ActionResult Alterar(Vestibular vestibular)
        {
            if (ModelState.IsValid)
            {
                repository.Alterar(vestibular);
                TempData["Mensagem"] = "Vestibular alterado com sucesso.";
                //return View("Index", cursoModel.RetornarTodos());
                return RedirectToAction("Index");
            }
            return View(vestibular);
        }

        //[Authorize]
        public ActionResult Excluir(int id)
        {
            if (!(autenticacaoProvider.Autenticado && autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
                HttpContext.Response.StatusCode = 401;
            return View(repository.Vestibulares.Where(v => v.iVestibularId == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Excluir(Vestibular vestibular)
        {
            try
            {
                repository.Excluir(vestibular.iVestibularId);
                TempData["Mensagem"] = "Vestibular excluído com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = ex.Message;
            }
            //return View("Index", cursoModel.RetornarTodos());
            return RedirectToAction("Index");
        }

        //[Authorize]
        public ActionResult Inserir()
        {
            if (!(autenticacaoProvider.Autenticado && autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
                HttpContext.Response.StatusCode = 401;
            // Para validação Assincrona! Passa-se um iVestibularId = 0 para o JS não achar que o 
            //campo iVestibularId esteja vazio já que o iVestibularId irá ser criado pelo BD!
            var vestibular = new Vestibular { iVestibularId = 0 };
            return View(vestibular);

            // Para validação Sincrona!
            //return View(); 
        }

        [HttpPost]
        public ActionResult Inserir(Vestibular vestibular)
        {
            //Isto serve para tirar o iVestibularId da validação pois ele é inserido automáticamente pelo banco. (Validação Sincrona!)
            ModelState["iVestibularId"].Errors.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    repository.Inserir(vestibular);
                    TempData["Mensagem"] = "Vestibular inserido com sucesso.";
                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = ex.Message;
                }

                //return View("Index", cursoModel.RetornarTodos());
                return RedirectToAction("Index");
            }
            return View(vestibular);
        }
        
    }
}
