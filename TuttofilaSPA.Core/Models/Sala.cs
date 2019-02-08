using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuttofilaSPA.Core.Models
{
	[Table("Sale")]
	public class Sala
	{
		[Key]
		public Guid Id { get; set; }
		public string Nome { get; set; }
		public virtual List<Servizio> Servizi { get; set; }
	}
}
