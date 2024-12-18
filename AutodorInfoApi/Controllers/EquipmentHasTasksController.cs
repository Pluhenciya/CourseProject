using AutodorInfoApi.Data;
using AutodorInfoApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EquipmentHasTasksController : ControllerBase
{
    private readonly AutodorContext _context;

    public EquipmentHasTasksController(AutodorContext context)
    {
        _context = context;
    }

    // GET: api/EquipmentHasTasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipmentHasTask>>> GetEquipmentHasTasks()
    {
        return await _context.EquipmentHasTasks.ToListAsync();
    }

    // GET: api/EquipmentHasTasks/5
    [HttpGet("{idTask}/{idEquipment}")]
    public async Task<ActionResult<EquipmentHasTask>> GetEquipmentHasTask(int idTask, int idEquipment)
    {
        var equipmentHasTask = await _context.EquipmentHasTasks
                    .FirstOrDefaultAsync(eht => eht.IdTask == idTask && eht.IdEquipment == idEquipment);

        if (equipmentHasTask == null)
        {
            return NotFound();
        }

        return equipmentHasTask;
    }

    // PUT: api/EquipmentHasTasks
    [HttpPut]
    public async Task<IActionResult> PutEquipmentHasTask(EquipmentHasTask equipmentHasTask)
    {
        if (!EquipmentHasTaskExists(equipmentHasTask.IdTask, equipmentHasTask.IdEquipment))
        {
            return NotFound();
        }

        _context.Entry(equipmentHasTask).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/EquipmentHasTasks
    [HttpPost]
    public async Task<ActionResult<EquipmentHasTask>> PostEquipmentHasTask(EquipmentHasTask equipmentHasTask)
    {
        if (EquipmentHasTaskExists(equipmentHasTask.IdTask, equipmentHasTask.IdEquipment))
        {
            return Conflict();
        }

        _context.EquipmentHasTasks.Add(equipmentHasTask);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEquipmentHasTask", new { idTask = equipmentHasTask.IdTask, idEquipment = equipmentHasTask.IdEquipment }, equipmentHasTask);
    }

    // DELETE: api/EquipmentHasTasks/5
    [HttpDelete("{idTask}/{idEquipment}")]
    public async Task<IActionResult> DeleteEquipmentHasTask(int idTask, int idEquipment)
    {
        var equipmentHasTask = await _context.EquipmentHasTasks
                    .FirstOrDefaultAsync(eht => eht.IdTask == idTask && eht.IdEquipment == idEquipment);
        if (equipmentHasTask == null)
        {
            return NotFound();
        }

        _context.EquipmentHasTasks.Remove(equipmentHasTask);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EquipmentHasTaskExists(int idTask, int idEquipment)
    {
        return _context.EquipmentHasTasks.Any(eht => eht.IdTask == idTask && eht.IdEquipment == idEquipment);
    }
}

