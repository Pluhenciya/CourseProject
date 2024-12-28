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
    public class MaterialsController : ControllerBase
    {
        private readonly AutodorContext _context;

        public MaterialsController(AutodorContext context)
        {
            _context = context;
        }

        // GET: api/Materials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetMaterials([FromQuery] string? name, [FromQuery] bool? isDeleted)
        {
            var query = _context.Materials.AsQueryable();

            // Фильтрация по имени, если оно указано
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name == name);
            }

            // Фильтрация по isDeleted, если оно указано
            if (isDeleted.HasValue)
            {
                query = query.Where(m => m.IsDeleted == isDeleted.Value);
            }

            // Получение списка материалов
            return await query.ToListAsync();
        }

        // GET: api/Materials/5
        [HttpGet("one")]
        public async Task<ActionResult<Material>> GetMaterial([FromQuery] int? id, [FromQuery] string? name)
        {
            var query = _context.Materials.OrderBy(e => e.CreatedDate).AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(m => m.IdMaterial == id);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }

            var material = await query.FirstOrDefaultAsync();

            if (material == null)
            {
                return NotFound();
            }

            return material;
        }

        // PUT: api/Materials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial(int id, Material material)
        {
            if (id != material.IdMaterial)
            {
                return BadRequest();
            }

            _context.Entry(material).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
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

        // POST: api/Materials
        [HttpPost]
        public async Task<ActionResult<Material>> PostMaterial(Material material)
        {
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaterial", new { id = material.IdMaterial }, material);
        }

        // DELETE: api/Materials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.IdMaterial == id);
        }
    }
}
