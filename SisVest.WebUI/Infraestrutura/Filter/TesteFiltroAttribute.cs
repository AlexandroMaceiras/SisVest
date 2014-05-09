using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SisVest.WebUI.Infraestrutura.Filter
{
    public class TesteFiltroAttribute : FilterAttribute, IActionFilter
    {
        //Executado depois que a action é finalizada
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Write("Executando filtro apos encerrar a action<br>");
        }
        //Antes que  depois que a action é inicializada
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Write("Executando filtro antes de iniciar a action<br>");
        }
    }
}