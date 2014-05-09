using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SisVest.WebUI.Infraestrutura.FilterProvider
{
    public class FilterProviderCustom : FilterAttributeFilterProvider
    {
        public override IEnumerable<System.Web.Mvc.Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);

            var dependencyResolver = (NinjectDependecyResolver)DependencyResolver.Current;

            if (dependencyResolver != null)
            {
                foreach (var filter in filters)
                {
                    dependencyResolver.Kernel.Inject(filter.Instance);
                }
            }

            return filters;
        }
    }
}