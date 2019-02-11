using System;
using TuttofilaSPA.Core.Models;

namespace TuttofilaSPA.Core.BusinessObjects
{
	public class ChiamataSportello
	{
		public ChiamataSportello()
		{
		}

		public ChiamataSportello(Servizio servizio)
		{
			SalaId = servizio.SalaId;
			NomeServizio = servizio.Nome;
		}

		public Guid SalaId { get; set; }
		public string NomeServizio { get; set; }
		public DateTime Data => DateTime.Now;
	}
}
