using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutodorInfoApi.Data;
using AutodorInfoApi.Models;

namespace AutodorInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly AutodorContext _context;

        public WorkersController(AutodorContext context)
        {
            _context = context;
        }

        // GET: api/Workers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Worker>>> GetWorkers([FromQuery] string? name, [FromQuery] bool? isDeleted)
        {
            var query = _context.Workers.AsQueryable();

            // Фильтрация по имени, если оно указано
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(w => w.Name == name);
            }

            // Фильтрация по isDeleted, если оно указано
            if (isDeleted.HasValue)
            {
                query = query.Where(w => w.IsDeleted == isDeleted.Value);
            }

            // Получение списка работников
            return await query.ToListAsync();
        }

        // GET: api/Workers/5
        [HttpGet("one")]
        public async Task<ActionResult<Worker>> GetWorker([FromQuery] int? id, [FromQuery] string? name)
        {
            var query = _context.Workers.OrderBy(e => e.CreatedDate).AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(w => w.IdWorker == id);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(w => w.Name.Contains(name));
            }

            var worker = await query.FirstOrDefaultAsync();

            if (worker == null)
            {
                return NotFound();
            }

            return worker;
        }

        // PUT: api/Workers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorker(int id, Worker worker)
        {
            if (id != worker.IdWorker)
            {
                return BadRequest();
            }

            _context.Entry(worker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Workers
        [HttpPost]
        public async Task<ActionResult<Worker>> PostWorker(Worker worker)
        {
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorker", new { id = worker.IdWorker }, worker);
        }

        // DELETE: api/Workers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(int id)
        {
            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }

            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkerExists(int id)
        {
            return _context.Workers.Any(e => e.IdWorker == id);
        }
    }
}
