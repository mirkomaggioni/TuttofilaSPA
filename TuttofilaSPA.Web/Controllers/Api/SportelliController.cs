using System;
using System.Web.Http;
using TuttofilaSPA.Core;
using TuttofilaSPA.Core.Models;
using TuttofilaSPA.Core.Services;

namespace TuttofilaSPA.Web.Controllers.Api
{
	public class SportelliController : ApiController
	{
		private readonly TuttofilaContext _db;
		private readonly SportelloService _sportelloService;

		public SportelliController(ContextFactory contextFactory, SportelloService sportelloService)
		{
			_db = contextFactory.GetContext<TuttofilaContext>();
			_sportelloService = sportelloService;
		}

		[HttpPost]
		public IHttpActionResult ChiamaServizio(Servizio servizio)
		{
			_sportelloService.Pubblica(servizio);
			return Ok();
		}
	}
}