using System;
using System.Configuration;
using Autofac;
using RabbitMQ.Client;
using TuttofilaSPA.Core.Services;

namespace TuttofilaSPA.Core
{
	public class ModuloCore : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(typeof(SportelloService).Assembly)
				.Where(t => t.Namespace == typeof(SportelloService).Namespace)
				.SingleInstance();

			builder.RegisterType<ContextFactory>().SingleInstance();

			var connectionFactory = new ConnectionFactory
			{
				UserName = ConfigurationManager.AppSettings["RabbitMQUsername"],
				Password = ConfigurationManager.AppSettings["RabbitMQPassword"]
			};

			builder.Register(c => connectionFactory).SingleInstance();
		}
	}
}
