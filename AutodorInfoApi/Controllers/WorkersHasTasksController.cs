using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutodorInfoApi.Data;
using AutodorInfoApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace AutodorInfoApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersHasTasksController : ControllerBase
    {
        private readonly AutodorContext _context;

        public WorkersHasTasksController(AutodorContext context)
        {
            _context = context;
        }

        // GET: api/WorkersHasTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkersHasTask>>> GetWorkersHasTasks()
        {
            return await _context.WorkersHasTasks.ToListAsync();
        }

        // GET: api/WorkersHasTasks/{idWorker}/{idTask}
        [HttpGet("{idWorker}/{idTask}")]
        public async Task<ActionResult<WorkersHasTask>> GetWorkersHasTask(int idWorker, int idTask)
        {
            var workersHasTask = await _context.WorkersHasTasks
                .FirstOrDefaultAsync(wht => wht.IdWorker == idWorker && wht.IdTask == idTask);

            if (workersHasTask == null)
            {
                return NotFound();
            }

            return workersHasTask;
        }

        // PUT: api/WorkersHasTasks
        [HttpPut]
        public async Task<IActionResult> PutWorkersHasTask(WorkersHasTask workersHasTask)
        {
            if (!WorkersHasTaskExists(workersHasTask.IdWorker, workersHasTask.IdTask))
            {
                return NotFound();
            }

            _context.Entry(workersHasTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkersHasTaskExists(workersHasTask.IdWorker, workersHasTask.IdTask))
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

        // POST: api/WorkersHasTasks
        [HttpPost]
        public async Task<ActionResult<WorkersHasTask>> PostWorkersHasTask(WorkersHasTask workersHasTask)
        {
            if (WorkersHasTaskExists(workersHasTask.IdWorker, workersHasTask.IdTask))
            {
                return Conflict();
            }

            _context.WorkersHasTasks.Add(workersHasTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkersHasTask", new { idWorker = workersHasTask.IdWorker, idTask = workersHasTask.IdTask }, workersHasTask);
        }

        // DELETE: api/WorkersHasTasks/{idWorker}/{idTask}
        [HttpDelete("{idWorker}/{idTask}")]
        public async Task<IActionResult> DeleteWorkersHasTask(int idWorker, int idTask)
        {
            var workersHasTask = await _context.WorkersHasTasks
                .FirstOrDefaultAsync(wht => wht.IdWorker == idWorker && wht.IdTask == idTask);
            if (workersHasTask == null)
            {
                return NotFound();
            }

            _context.WorkersHasTasks.Remove(workersHasTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkersHasTaskExists(int idWorker, int idTask)
        {
            return _context.WorkersHasTasks.Any(wht => wht.IdWorker == idWorker && wht.IdTask == idTask);
        }
    }
}
