using AutodorInfoSystem.Data;
using AutodorInfoSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutodorInfoSystem.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly AutodorContext _context;

        public EquipmentsController(AutodorContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetSimilarNames(string name)
        {
            var similarItems = _context.Equipment
                .Where(e => e.Name.Contains(name))
                .Select(e => new
                {
                    e.Name,
                    e.Price // Возвращаем также цену
                })
                .ToList();

            return Json(similarItems);
        }

        // GET: Equipments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Equipment.ToListAsync());
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(m => m.IdEquipment == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipments/Create
        public IActionResult Create(int idTask)
        {
            ViewBag.IdTask = idTask;
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Equipment equipment, int idTask)
        {
            if (ModelState.IsValid)
            {
                var findEquipment = await _context.Equipment.FirstOrDefaultAsync(e => e.Name == equipment.Name);
                if (findEquipment != null)
                {
                    var existingRelation = await _context.EquipmentHasTasks
                        .FirstOrDefaultAsync(eht => eht.IdTask == idTask && eht.IdEquipment == findEquipment.IdEquipment);

                    if (existingRelation != null)
                    {
                        // Здесь вы можете добавить логику для запроса у пользователя
                        // Например, перенаправить на страницу с подтверждением
                        ViewBag.ExistingRelation = existingRelation;
                        ViewBag.NewQuantity = equipment.Quantity ?? 0;
                        ViewBag.IdTask = idTask;
                        return View("ConfirmQuantity", equipment); // Создайте представление ConfirmQuantity
                    }
                    else
                    {
                        _context.Add(new EquipmentHasTask
                        {
                            IdTask = idTask,
                            IdEquipment = findEquipment.IdEquipment,
                            Quantity = equipment.Quantity ?? 0,
                        });
                    }
                }
                else
                {
                    _context.Equipment.Add(equipment);
                    await _context.SaveChangesAsync();
                    var idEquipment = _context.Equipment.FirstOrDefault(e => e.Name == equipment.Name).IdEquipment;
                    _context.EquipmentHasTasks.Add(new EquipmentHasTask
                    {
                        IdTask = idTask,
                        IdEquipment = idEquipment,
                        Quantity = equipment.Quantity ?? 0
                    });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tasks", new { id = idTask });
            }
            if (equipment.Price == null)
            {
                ModelState["Price"].Errors.Clear();
                ModelState.AddModelError("Price", "Введенное не является ценой");
            }
            ViewBag.IdTask = idTask;
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id, int idTask)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            ViewBag.IdTask = idTask;
            equipment.Quantity = _context.EquipmentHasTasks.FirstOrDefault(wht => wht.IdTask == idTask && wht.IdEquipment == equipment.IdEquipment).Quantity;
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Equipment equipment, int idTask)
        {
            if (id != equipment.IdEquipment)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var findEquipment = await _context.Equipment.FirstOrDefaultAsync(e => e.Name == equipment.Name);
                    if (findEquipment != null)
                    {
                        var existingRelation = await _context.EquipmentHasTasks
                        .FirstOrDefaultAsync(eht => eht.IdTask == idTask && eht.IdEquipment == findEquipment.IdEquipment);

                        if (existingRelation != null)
                        {
                            // Здесь вы можете добавить логику для запроса у пользователя
                            // Например, перенаправить на страницу с подтверждением
                            ViewBag.ExistingRelation = existingRelation;
                            ViewBag.NewQuantity = equipment.Quantity ?? 0;
                            ViewBag.IdTask = idTask;
                            return View("ConfirmQuantity", equipment); // Создайте представление ConfirmQuantity
                        }
                        else
                        {
                            _context.Update(new EquipmentHasTask
                            {
                                IdTask = idTask,
                                IdEquipment = findEquipment.IdEquipment,
                                Quantity = equipment.Quantity ?? 0
                            });
                        }
                    }
                    else
                    {
                        _context.Equipment.Update(equipment);
                        await _context.SaveChangesAsync();
                        var idEquipment = _context.Equipment.FirstOrDefault(e => e.Name == equipment.Name).IdEquipment;
                        _context.EquipmentHasTasks.Update(new EquipmentHasTask
                        {
                            IdTask = idTask,
                            IdEquipment = idEquipment,
                            Quantity = equipment.Quantity ?? 0
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.IdEquipment))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Tasks", new { id = idTask });
            }
            if (equipment.Price == null)
            {
                ModelState["Price"].Errors.Clear();
                ModelState.AddModelError("Price", "Введенное не является ценой");
            }
            ViewBag.IdTask = idTask;
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id, int idTask)
        {
            var equipmentHasTask = await _context.EquipmentHasTasks.FirstOrDefaultAsync(mht => mht.IdTask == idTask && mht.IdEquipment == id);
            if (equipmentHasTask != null)
            {
                _context.EquipmentHasTasks.Remove(equipmentHasTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int idTask, int idEquipment, int newQuantity, string action)
        {
            var existingRelation = await _context.EquipmentHasTasks
                .FirstOrDefaultAsync(eht => eht.IdTask == idTask && eht.IdEquipment == idEquipment);

            if (action == "add")
            {
                existingRelation.Quantity += newQuantity;
            }
            else if (action == "replace")
            {
                existingRelation.Quantity = newQuantity;
            }
            _context.EquipmentHasTasks.Update(existingRelation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }


        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.IdEquipment == id);
        }
    }
}
