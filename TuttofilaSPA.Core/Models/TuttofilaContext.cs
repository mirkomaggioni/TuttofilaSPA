using System.Data.Entity;

namespace TuttofilaSPA.Core.Models
{
	public class TuttofilaContext : DbContext
	{
		public DbSet<Servizio> Servizi { get; set; }
		public DbSet<Sala> Sale { get; set; }
	}
}
