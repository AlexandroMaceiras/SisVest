using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Entities;
using SisVest.WebUI.Models;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;
using SisVest.WebUI.Infraestrutura.Filter;

namespace SisVest.WebUI.Controllers
{
    //[Authorize]
    //[TesteFiltro] Usa o este filtro Attribute em todas as Actions
    public class CursoController : Controller
    {
        private ICursoRepository repository;
        private CursoModel cursoModel;
        private IAutenticacaoProvider autenticacaoProvider;

        /// <summary>
        /// O construtor instancia e assim faz a inversão de controle para ser usado em todas as ações.
        /// </summary>
        /// <param name="cursoRepository"></param>
        public CursoController(ICursoRepository cursoRepository, CursoModel cursoModelparam, IAutenticacaoProvider autenticacaoProviderParam)
        {
            repository = cursoRepository;
            cursoModel = cursoModelparam;
            autenticacaoProvider = autenticacaoProviderParam;
        }

        //[Authorize] Pode ser colocado aqui barrando só esta action 
        //ou direto lá em cima barrando todo o controller
        //[TesteFiltro] Usa o este filtro Attribute só nesta Action
        //[CustomAutenticacao] Nega o acesso á esta Action sem precisar testar neste local
        //mas sim onde ele é declarado
        [CustomAutenticacao("administrador")]
        public ActionResult Index()
        {
            //HttpContext.Response.Write("Action iniciou<br>");
            //if (!(autenticacaoProvider.Autenticado && autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
            //    HttpContext.Response.StatusCode = 401;
            return View(cursoModel.RetornarTodos());
        }

        public ActionResult Alterar(int id)
        {
            if (!(autenticacaoProvider.Autenticado && autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
                HttpContext.Response.StatusCode = 401;
            return View(repository.RetornarPorId(id));
        }

        [HttpPost]
        public ActionResult Alterar(Curso curso)
        {
            if (ModelState.IsValid)
            {
                repository.Alterar(curso);
                TempData["Mensagem"] = "Curso alterado com sucesso.";
                //return View("Index", cursoModel.RetornarTodos());
                return RedirectToAction("Index");
            }
            return View(curso);
        }

        //[Authorize]
        public ActionResult Excluir(int id)
        {
            if (!(autenticacaoProvider.Autenticado && autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
                HttpContext.Response.StatusCode = 401;
            return View(repository.RetornarPorId(id));
        }

        [HttpPost]
        public ActionResult Excluir(Curso curso)
        {
            try
            {
                repository.Excluir(curso.iCursoID);
                TempData["Mensagem"] = "Curso excluído com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = ex.Message;
            }
            //return View("Index", cursoModel.RetornarTodos());
            return RedirectToAction("Index");
        }

        //[Authorize]
        //[CustomAutenticacao("candidato")] => colocando isto o inserir só vai funcionar pra quem
        //é do grupo candidato.
        [CustomAutenticacao("candidato")]
        public ActionResult Inserir()
        {
            if (!(autenticacaoProvider.Autenticado && autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
                HttpContext.Response.StatusCode = 401;
            // Para validação Assincrona! Passa-se um iCursoID = 0 para o JS não achar que o 
            //campo iCursoID esteja vazio já que o iCursoID irá ser criado pelo BD!
            var curso = new Curso { iCursoID = 0 };
            return View(curso);

            // Para validação Sincrona!
            //return View(); 
        }

        [HttpPost]
        public ActionResult Inserir(Curso curso)
        {
            //Isto serve para tirar o iCursoID da validação pois ele é inserido automáticamente pelo banco. (Validação Sincrona!)
            //ModelState["iCursoID"].Errors.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    repository.Inserir(curso);
                    TempData["Mensagem"] = "Curso inserido com sucesso.";
                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = ex.Message;
                }

                //return View("Index", cursoModel.RetornarTodos());
                return RedirectToAction("Index");
            }
            return View(curso);
        }
    }
}
