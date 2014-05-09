using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Ninject.Syntax;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;
using SisVest.WebUI.Infraestrutura.Provider.Concrete;

namespace SisVest.WebUI.Infraestrutura
{
    public class NinjectDependecyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependecyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        public IBindingToSyntax<T> Bind<T>()
        {
            return kernel.Bind<T>();
        }

        public IKernel Kernel
        {
            get { return kernel; }
        }

        private void AddBindings()
        {
            Bind<ICursoRepository>().To<EFCursoRepository>();
            Bind<IAdminRepository>().To<EFAdminRepository>();
            Bind<IVestibularRepository>().To<EFVestibularRepository>();
            Bind<ICandidatoRepository>().To<EFCandidatoRepository>();
            //Não necessário
            //ninjectKernel.Bind<VestContext>().ToSelf();
            //ninjectKernel.Bind<CursoModel>().ToSelf();
            Bind<IAutenticacaoProvider>().To<CustomAutenticacaoProvider>();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}