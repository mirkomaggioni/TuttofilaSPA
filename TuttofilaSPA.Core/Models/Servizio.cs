using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuttofilaSPA.Core.Models
{
	[Table("Servizi")]
	public class Servizio
	{
		[Key]
		public Guid Id { get; set; }
		public string Nome { get; set; }

		public Guid SalaId { get; set; }
		[ForeignKey("SalaId")]
		public virtual Sala Sala { get; set; }
	}
}
