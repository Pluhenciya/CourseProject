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
    public class ProjectersController : ControllerBase
    {
        private readonly AutodorContext _context;

        public ProjectersController(AutodorContext context)
        {
            _context = context;
        }

        // GET: api/Projecters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Projecter>>> GetProjecters()
        {
            return await _context.Projecters.ToListAsync();
        }

        // GET: api/Projecters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Projecter>> GetProjecter(int id)
        {
            var projecter = await _context.Projecters.FindAsync(id);

            if (projecter == null)
            {
                return NotFound();
            }

            return projecter;
        }

        // PUT: api/Projecters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjecter(int id, Projecter projecter)
        {
            if (id != projecter.IdUser)
            {
                return BadRequest();
            }

            _context.Entry(projecter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjecterExists(id))
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

        // POST: api/Projecters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Projecter>> PostProjecter(Projecter projecter)
        {
            _context.Projecters.Add(projecter);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProjecterExists(projecter.IdUser))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProjecter", new { id = projecter.IdUser }, projecter);
        }

        // DELETE: api/Projecters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjecter(int id)
        {
            var projecter = await _context.Projecters.FindAsync(id);
            if (projecter == null)
            {
                return NotFound();
            }

            _context.Projecters.Remove(projecter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjecterExists(int id)
        {
            return _context.Projecters.Any(e => e.IdUser == id);
        }
    }
}
