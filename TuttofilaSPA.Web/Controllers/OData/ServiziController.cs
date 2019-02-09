using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using TuttofilaSPA.Core;
using TuttofilaSPA.Core.Models;

namespace TuttofilaSPA.Web.Controllers.OData
{
	public class ServiziController : ODataController
	{
		private readonly TuttofilaContext _db;

		public ServiziController(ContextFactory contextFactory)
		{
			_db = contextFactory.GetContext<TuttofilaContext>();
		}

		// GET: odata/Servizi
		[EnableQuery]
		public IQueryable<Servizio> Get()
		{
			return _db.Servizi;
		}

		// GET: odata/Servizi(5)
		[EnableQuery]
		public SingleResult<Servizio> Get(Guid key)
		{
			return SingleResult.Create(_db.Servizi.Where(b => b.Id == key));
		}

		// PATCH: odata/Servizi(5)
		[AcceptVerbs("PATCH", "MERGE")]
		public async Task<IHttpActionResult> Patch(Guid key, Delta<Servizio> patch)
		{
			Validate(patch.GetInstance());

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var servizio = await _db.Servizi.FindAsync(key);
			if (servizio == null)
			{
				return NotFound();
			}

			patch.Patch(servizio);
			_db.Entry(servizio).State = EntityState.Modified;

			await _db.SaveChangesAsync();

			return Updated(servizio);
		}

		// POST: odata/Servizi
		public async Task<IHttpActionResult> Post(Servizio servizio)
		{
			servizio.Id = Guid.NewGuid();

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_db.Servizi.Add(servizio);
			await _db.SaveChangesAsync();

			return Created(servizio);
		}

		// DELETE: odata/Servizi(5)
		public async Task<IHttpActionResult> Delete(Guid key)
		{
			var entity = await _db.Servizi.FindAsync(key);
			if (entity == null)
			{
				return NotFound();
			}

			_db.Servizi.Remove(entity);
			await _db.SaveChangesAsync();

			return StatusCode(HttpStatusCode.OK);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}