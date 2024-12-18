using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutodorInfoApi.Data;
using AutodorInfoApi.Models;

namespace AutodorInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AutodorContext _context;

        public TasksController(AutodorContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTask(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.MaterialsHasTasks)
                    .ThenInclude(m => m.IdMaterialNavigation)
                .Include(t => t.EquipmentHasTasks)
                    .ThenInclude(e => e.IdEquipmentNavigation)
                .Include(t => t.WorkersHasTasks)
                    .ThenInclude(w => w.IdWorkerNavigation)
                .FirstOrDefaultAsync(m => m.IdTask == id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Models.Task task)
        {
            if (id != task.IdTask)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Task>> PostTask(Models.Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.IdTask }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.IdTask == id);
        }
    }
}
