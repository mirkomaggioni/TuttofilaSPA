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
	public class SaleController : ODataController
	{
		private readonly TuttofilaContext _db;

		public SaleController(ContextFactory contextFactory)
		{
			_db = contextFactory.GetContext<TuttofilaContext>();
		}

		// GET: odata/Sale
		[EnableQuery]
		public IQueryable<Sala> Get()
		{
			return _db.Sale;
		}

		// GET: odata/Sale(5)
		[EnableQuery]
		public SingleResult<Sala> Get(Guid key)
		{
			return SingleResult.Create(_db.Sale.Where(b => b.Id == key));
		}

		// PATCH: odata/Sale(5)
		[AcceptVerbs("PATCH", "MERGE")]
		public async Task<IHttpActionResult> Patch(Guid key, Delta<Sala> patch)
		{
			Validate(patch.GetInstance());

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var sala = await _db.Sale.FindAsync(key);
			if (sala == null)
			{
				return NotFound();
			}

			patch.Patch(sala);
			_db.Entry(sala).State = EntityState.Modified;

			await _db.SaveChangesAsync();

			return Updated(sala);
		}

		// POST: odata/Sale
		public async Task<IHttpActionResult> Post(Sala sala)
		{
			sala.Id = Guid.NewGuid();

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_db.Sale.Add(sala);
			await _db.SaveChangesAsync();

			return Created(sala);
		}

		// DELETE: odata/Sale(5)
		public async Task<IHttpActionResult> Delete(Guid key)
		{
			var entity = await _db.Sale.FindAsync(key);
			if (entity == null)
			{
				return NotFound();
			}

			_db.Sale.Remove(entity);
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