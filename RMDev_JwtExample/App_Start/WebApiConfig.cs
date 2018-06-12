using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RMDev_JwtExample.IoCConfig;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;

namespace RMDev_JwtExample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = SmObjectFactory.Container;
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator), new SmWebApiControllerActivator(container));

            config.Services.Replace(typeof(IFilterProvider), new SmWebApiFilterProvider(container));

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            setSerializerSettings();
        }

        private static void setSerializerSettings()
        {
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}