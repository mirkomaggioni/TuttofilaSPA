using Microsoft.AspNet.SignalR;
using TuttofilaSPA.Core;
using TuttofilaSPA.Core.Models;
using TuttofilaSPA.Core.Services;

namespace TuttofilaSPA.Web.Hubs
{
	public class SportelliHub : Hub
	{
		private readonly TuttofilaContext _db;
		private readonly SportelloService _sportelloService;

		public SportelliHub(ContextFactory contextFactory, SportelloService sportelloService)
		{
			_db = contextFactory.GetContext<TuttofilaContext>();
			_sportelloService = sportelloService;
			_sportelloService.MessaggioRicevuto += MessaggioRicevuto;
		}

		private void MessaggioRicevuto(object sender, MessaggioRicevutoEventArgs e)
		{
			Clients.All.aggiornaServizi(_sportelloService.RestituisciServiziChiamati(e.SalaId));
		}
	}
}