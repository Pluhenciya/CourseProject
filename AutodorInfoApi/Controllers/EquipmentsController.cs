using AutodorInfoApi.Data;
using AutodorInfoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutodorInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly AutodorContext _context;

        public EquipmentsController(AutodorContext context)
        {
            _context = context;
        }

        // GET: api/Equipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipment([FromQuery] string? name, [FromQuery] bool? isDeleted)
        {
            var query = _context.Equipment.AsQueryable();

            // Фильтрация по имени, если оно указано
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name == name);
            }

            // Фильтрация по isDeleted, если оно указано
            if (isDeleted.HasValue)
            {
                query = query.Where(e => e.IsDeleted == isDeleted.Value);
            }

            // Получение списка оборудования
            return await query.ToListAsync();
        }

        [HttpGet("one")]
        // GET: api/Equipments?id=5
        public async Task<ActionResult<Equipment>> GetEquipment([FromQuery] int? id, [FromQuery] string? name)
        {
            var query = _context.Equipment.OrderBy(e => e.CreatedDate).AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(e => e.IdEquipment == id);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }

            var equipment = await query.FirstOrDefaultAsync();

            if (equipment == null)
            {
                return NotFound();
            }

            return equipment;
        }

        // PUT: api/Equipments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment(int id, Equipment equipment)
        {
            if (id != equipment.IdEquipment)
            {
                return BadRequest();
            }

            _context.Entry(equipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentExists(id))
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

        // POST: api/Equipments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(Equipment equipment)
        {
            _context.Equipment.Add(equipment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquipment", new { id = equipment.IdEquipment }, equipment);
        }

        // DELETE: api/Equipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.IdEquipment == id);
        }
    }
}
