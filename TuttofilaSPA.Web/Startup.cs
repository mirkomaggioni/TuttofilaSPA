using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using TuttofilaSPA.Core;
using TuttofilaSPA.Core.Models;

[assembly: OwinStartup(typeof(TuttofilaSPA.Web.Startup))]

namespace TuttofilaSPA.Web
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterModule(new ModuloCore());
			containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
			containerBuilder.RegisterHubs(Assembly.GetExecutingAssembly());

			var container = containerBuilder.Build();

			var config = new HttpConfiguration
			{
				DependencyResolver = new AutofacWebApiDependencyResolver(container)
			};

			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			var builder = new ODataConventionModelBuilder();
			builder.EntitySet<Sala>("Sale");
			builder.EntitySet<Servizio>("Servizi");
			config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
			config.Filter().Expand().Select().OrderBy().MaxTop(null).Count();

			app.UseWebApi(config);

			var hubConfig = new HubConfiguration {
				EnableDetailedErrors = true
			};
			hubConfig.Resolver = new AutofacDependencyResolver(container);
			app.MapSignalR(hubConfig);
		}
	}
}
