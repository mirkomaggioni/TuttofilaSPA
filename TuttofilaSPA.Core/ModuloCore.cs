using Autofac;

namespace TuttofilaSPA.Core
{
	public class ModuloCore : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ContextFactory>().AsSelf().SingleInstance();
		}
	}
}
