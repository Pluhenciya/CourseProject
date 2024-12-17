using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutodorInfoSystem.Data;
using AutodorInfoSystem.Models;
using Microsoft.Build.Framework;

namespace AutodorInfoSystem.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly AutodorContext _context;

        public MaterialsController(AutodorContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetSimilarNames(string name)
        {
            var similarItems = _context.Materials
                .Where(m => m.Name.Contains(name) && !m.IsDeleted)
                .Select(m => new
                {
                    m.Name,
                    m.Price // Возвращаем также цену
                })
                .ToList();

            return Json(similarItems);
        }

        // GET: Materials
        public async Task<IActionResult> Index()
        {
            return View(await _context.Materials.Where(m => !m.IsDeleted).ToListAsync());
        }

        public IActionResult SimpleCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleCreate(Material material)
        {
            ModelState.Remove("Quantity");
            if (ModelState.IsValid)
            {
                _context.Materials.Add(material);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        public async Task<IActionResult> SimpleEdit(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SimpleEdit(int id, Material material)
        {
            if (id != material.IdMaterial)
            {
                return NotFound();
            }
            ModelState.Remove("Quantity");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.IdMaterial))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        public async Task<IActionResult> SimpleDelete(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material != null)
            {
                material.IsDeleted = true;
                _context.Materials.Update(material);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.IdMaterial == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // GET: Materials/Create
        public IActionResult Create(int idTask)
        {
            ViewBag.IdTask = idTask;
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material, int idTask)
        {
            if (ModelState.IsValid)
            {
                var findMaterial = await _context.Materials.FirstOrDefaultAsync(e => e.Name == material.Name);
                if (findMaterial != null)
                {
                    // Проверяем, существует ли уже связь между материалом и задачей
                    var existingRelation = await _context.MaterialsHasTasks
                        .FirstOrDefaultAsync(mht => mht.IdTask == idTask && mht.IdMaterial == findMaterial.IdMaterial);

                    if (existingRelation != null)
                    {
                        // Перенаправляем на страницу подтверждения
                        ViewBag.ExistingRelation = existingRelation;
                        ViewBag.NewQuantity = material.Quantity ?? 0;
                        ViewBag.IdTask = idTask;
                        return View("ConfirmQuantity", material); // Создайте представление ConfirmQuantity
                    }
                    else
                    {
                        _context.Add(new MaterialsHasTask
                        {
                            IdTask = idTask,
                            IdMaterial = findMaterial.IdMaterial,
                            Quantity = material.Quantity ?? 0,
                        });
                    }
                }
                else
                {
                    _context.Materials.Add(material);
                    await _context.SaveChangesAsync();
                    var idMaterial = _context.Materials.FirstOrDefault(m => m.Name == material.Name).IdMaterial;
                    _context.MaterialsHasTasks.Add(new MaterialsHasTask
                    {
                        IdTask = idTask,
                        IdMaterial = idMaterial,
                        Quantity = material.Quantity ?? 0
                    });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tasks", new { id = idTask });
            }
            if (material.Price == null)
            {
                ModelState["Price"].Errors.Clear();
                ModelState.AddModelError("Price", "Введенное не является ценой");
            }
            return View(material);
        }


        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id, int idTask)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            ViewBag.IdTask = idTask;
            material.Quantity = _context.MaterialsHasTasks.FirstOrDefault(mht => mht.IdTask == idTask && mht.IdMaterial == material.IdMaterial).Quantity;
            return View(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Material material, int idTask)
        {
            if (id != material.IdMaterial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var findMaterial = await _context.Materials.FirstOrDefaultAsync(e => e.Name == material.Name);
                    if (findMaterial != null)
                    {
                        // Проверяем, существует ли уже связь между материалом и задачей
                        var existingRelation = await _context.MaterialsHasTasks
                            .FirstOrDefaultAsync(mht => mht.IdTask == idTask && mht.IdMaterial == findMaterial.IdMaterial);

                        if (existingRelation != null)
                        {
                            // Перенаправляем на страницу подтверждения
                            ViewBag.ExistingRelation = existingRelation;
                            ViewBag.NewQuantity = material.Quantity ?? 0;
                            ViewBag.IdTask = idTask;
                            return View("ConfirmQuantity", material); // Создайте представление ConfirmQuantity
                        }
                        else
                        {
                            _context.Update(new MaterialsHasTask
                            {
                                IdTask = idTask,
                                IdMaterial = findMaterial.IdMaterial,
                                Quantity = material.Quantity ?? 0,
                            });
                        }
                    }
                    else
                    {
                        _context.Materials.Update(material);
                        await _context.SaveChangesAsync();
                        var idMaterial = _context.Materials.FirstOrDefault(m => m.Name == material.Name).IdMaterial;
                        _context.MaterialsHasTasks.Update(new MaterialsHasTask
                        {
                            IdTask = idTask,
                            IdMaterial = idMaterial,
                            Quantity = material.Quantity ?? 0
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.IdMaterial))
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
            if (material.Price == null)
            {
                ModelState["Price"].Errors.Clear();
                ModelState.AddModelError("Price", "Введенное не является ценой");
            }
            ViewBag.IdTask = idTask;
            return View(material);
        }


        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id, int idTask)
        {
            var materialsHasTask = await _context.MaterialsHasTasks.FirstOrDefaultAsync(mht => mht.IdTask == idTask && mht.IdMaterial == id);
            if (materialsHasTask != null)
            {
                _context.MaterialsHasTasks.Remove(materialsHasTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int idTask, int idMaterial, int newQuantity, string action)
        {
            var existingRelation = await _context.MaterialsHasTasks
                .FirstOrDefaultAsync(mht => mht.IdTask == idTask && mht.IdMaterial == idMaterial);

            if (action == "add")
            {
                existingRelation.Quantity += newQuantity;
            }
            else if (action == "replace")
            {
                existingRelation.Quantity = newQuantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.IdMaterial == id);
        }
    }
}
