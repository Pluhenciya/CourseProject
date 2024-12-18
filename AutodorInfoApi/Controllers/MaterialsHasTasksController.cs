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
    public class MaterialsHasTasksController : ControllerBase
    {
        private readonly AutodorContext _context;

        public MaterialsHasTasksController(AutodorContext context)
        {
            _context = context;
        }

        // GET: api/MaterialsHasTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialsHasTask>>> GetMaterialsHasTasks()
        {
            return await _context.MaterialsHasTasks.ToListAsync();
        }

        // GET: api/MaterialsHasTasks/{idTask}/{idMaterial}
        [HttpGet("{idTask}/{idMaterial}")]
        public async Task<ActionResult<MaterialsHasTask>> GetMaterialsHasTask(int idTask, int idMaterial)
        {
            var materialsHasTask = await _context.MaterialsHasTasks
                .FirstOrDefaultAsync(mht => mht.IdTask == idTask && mht.IdMaterial == idMaterial);

            if (materialsHasTask == null)
            {
                return NotFound();
            }

            return materialsHasTask;
        }

        // PUT: api/MaterialsHasTasks
        [HttpPut]
        public async Task<IActionResult> PutMaterialsHasTask(MaterialsHasTask materialsHasTask)
        {
            if (!MaterialsHasTaskExists(materialsHasTask.IdTask, materialsHasTask.IdMaterial))
            {
                return NotFound();
            }

            _context.Entry(materialsHasTask).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/MaterialsHasTasks
        [HttpPost]
        public async Task<ActionResult<MaterialsHasTask>> PostMaterialsHasTask(MaterialsHasTask materialsHasTask)
        {
            if (MaterialsHasTaskExists(materialsHasTask.IdTask, materialsHasTask.IdMaterial))
            {
                return Conflict();
            }

            _context.MaterialsHasTasks.Add(materialsHasTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaterialsHasTask", new { idTask = materialsHasTask.IdTask, idMaterial = materialsHasTask.IdMaterial }, materialsHasTask);
        }

        // DELETE: api/MaterialsHasTasks/{idTask}/{idMaterial}
        [HttpDelete("{idTask}/{idMaterial}")]
        public async Task<IActionResult> DeleteMaterialsHasTask(int idTask, int idMaterial)
        {
            var materialsHasTask = await _context.MaterialsHasTasks
                .FirstOrDefaultAsync(mht => mht.IdTask == idTask && mht.IdMaterial == idMaterial);
            if (materialsHasTask == null)
            {
                return NotFound();
            }

            _context.MaterialsHasTasks.Remove(materialsHasTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaterialsHasTaskExists(int idTask, int idMaterial)
        {
            return _context.MaterialsHasTasks.Any(mht => mht.IdTask == idTask && mht.IdMaterial == idMaterial);
        }
    }
}
