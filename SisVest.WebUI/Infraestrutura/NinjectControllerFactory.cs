using System.Web.Mvc;
using Ninject;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using SisVest.WebUI.Models;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;
using SisVest.WebUI.Infraestrutura.Provider.Concrete;

namespace SisVest.WebUI.Infraestrutura
{ 
    /// <summary>
    /// Esta classe é de infraestrutura para o Global.asax.cs iniciar o ControllerBulder MVC com Ninject.
    /// </summary>
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, System.Type controllerType)
        {
            //return base.GetControllerInstance(requestContext, controllerType);

            //Assim as dependências do MVC controller serão injetadas pelo Ninject.
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<ICursoRepository>().To<EFCursoRepository>();
            ninjectKernel.Bind<IAdminRepository>().To<EFAdminRepository>();
            ninjectKernel.Bind<IVestibularRepository>().To<EFVestibularRepository>();
            ninjectKernel.Bind<ICandidatoRepository>().To<EFCandidatoRepository>();
            //Não necessário
            //ninjectKernel.Bind<VestContext>().ToSelf();
            //ninjectKernel.Bind<CursoModel>().ToSelf();
            ninjectKernel.Bind<IAutenticacaoProvider>().To<CustomAutenticacaoProvider>();
        }
    }
}