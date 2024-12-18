using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutodorInfoApi.Data;
using AutodorInfoApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace AutodorInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly AutodorContext _context;

        public ProjectsController(AutodorContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects([FromQuery] int? idProjecter, [FromQuery] bool? isCompleted)
        {
            var query = _context.Projects.Include(p => p.ProjectersIdUsers).Include(p => p.Tasks).AsQueryable();
            if (idProjecter != null)
                query = query.Where(p => p.ProjectersIdUsers.Any(pid => pid.IdUser == idProjecter));
            if (isCompleted != null)
                query = query.Where(p => p.IsCompleted);
            return await query.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectersIdUsers)
                .Include(p => p.Tasks)
                .ThenInclude(p => p.MaterialsHasTasks)
                .ThenInclude(p => p.IdMaterialNavigation)
                .Include(p => p.Tasks)
                .ThenInclude(p => p.EquipmentHasTasks)
                .ThenInclude(p => p.IdEquipmentNavigation)
                .Include(p => p.Tasks)
                .ThenInclude(p => p.WorkersHasTasks)
                .ThenInclude(p => p.IdWorkerNavigation)
                .FirstOrDefaultAsync(p =>p.IdProject == id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [Authorize]
        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.IdProject)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        [Authorize]
        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project, [FromQuery] int? idProjecter)
        {
            try
            {
                project.IdProject = 0;
                if(idProjecter != null)
                {
                    project.ProjectersIdUsers.Add(await _context.Projecters.FindAsync(idProjecter));
                }
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetProject", new { id = project.IdProject }, project);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                // Например, используйте ILogger для записи ошибки
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.IdProject == id);
        }
    }
}
