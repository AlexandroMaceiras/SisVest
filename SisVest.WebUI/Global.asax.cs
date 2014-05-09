using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SisVest.WebUI.Infraestrutura;
using SisVest.WebUI.Infraestrutura.FilterProvider;

namespace SisVest.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Curso", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            DependencyResolver.SetResolver(new NinjectDependecyResolver());

            FilterProviders.Providers.Clear();
            FilterProviders.Providers.Add(new FilterProviderCustom());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //Novo controller de Ninject da infraestrutura sendo instanciado na inicialização. 
            //O ControllerBuilder do MVC desta maneira irá usar o Ninject ao invés de usar o controler default dele.
            //ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
        }
    }
}