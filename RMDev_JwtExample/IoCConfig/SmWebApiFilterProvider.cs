using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using StructureMap;

namespace RMDev_JwtExample.IoCConfig
{
    public class SmWebApiFilterProvider : ActionDescriptorFilterProvider, IFilterProvider
    {
        private readonly IContainer _container;

        public SmWebApiFilterProvider(IContainer container)
        {
            _container = container;
        }

        public new IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(configuration, actionDescriptor);

            foreach (var filter in filters)
            {
                _container.BuildUp(filter.Instance);
                yield return filter;
            }
        }
    }
}