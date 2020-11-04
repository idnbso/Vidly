using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Vidly.Controllers.Api;
using Vidly.Infrastructure;

namespace Vidly
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var serializerSettings = config.Formatters.JsonFormatter.SerializerSettings;
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            serializerSettings.Formatting = Formatting.Indented;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ConfigureDependencyInjection(config);
        }

        public static void ConfigureDependencyInjection(HttpConfiguration config)
        {

            var builder = new ContainerBuilder();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule<AutoFacModule>();

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
