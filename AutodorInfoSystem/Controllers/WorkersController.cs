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
    public class WorkersController : Controller
    {
        private readonly AutodorContext _context;

        public WorkersController(AutodorContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetSimilarNames(string name)
        {
            var similarItems = _context.Workers
                .Where(e => e.Name.Contains(name))
                .Select(e => new
                {
                    e.Name,
                    e.Salary // Возвращаем также цену
                })
                .ToList();

            return Json(similarItems);
        }

        // GET: Workers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Workers.ToListAsync());
        }

        public IActionResult SimpleCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleCreate(Worker worker)
        {
            ModelState.Remove("Quantity");
            if (ModelState.IsValid)
            {
                _context.Workers.Add(worker);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(worker);
        }

        public async Task<IActionResult> SimpleEdit(int id)
        {
            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            return View(worker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SimpleEdit(int id, Worker worker)
        {
            if (id != worker.IdWorker)
            {
                return NotFound();
            }
            ModelState.Remove("Quantity");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.IdWorker))
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
            return View(worker);
        }

        public async Task<IActionResult> SimpleDelete(int id)
        {
            var worker = await _context.Workers.FindAsync(id);
            if (worker != null)
            {
                _context.Workers.Remove(worker);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .FirstOrDefaultAsync(m => m.IdWorker == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Create
        public IActionResult Create(int idTask)
        {
            ViewBag.IdTask = idTask;
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Worker worker, int idTask)
        {
            if (ModelState.IsValid)
            {
                var findWorker = await _context.Workers.FirstOrDefaultAsync(e => e.Name == worker.Name);
                if (findWorker != null)
                {
                    // Проверяем, существует ли уже связь между работником и задачей
                    var existingRelation = await _context.WorkersHasTasks
                        .FirstOrDefaultAsync(wht => wht.IdTask == idTask && wht.IdWorker == findWorker.IdWorker);

                    if (existingRelation != null)
                    {
                        // Перенаправляем на страницу подтверждения
                        ViewBag.ExistingRelation = existingRelation;
                        ViewBag.NewQuantity = worker.Quantity ?? 0;
                        ViewBag.IdTask = idTask;
                        return View("ConfirmQuantity", worker); // Создайте представление ConfirmQuantity
                    }
                    else
                    {
                        _context.Add(new WorkersHasTask
                        {
                            IdTask = idTask,
                            IdWorker = findWorker.IdWorker,
                            Quantity = worker.Quantity ?? 0,
                        });
                    }
                }
                else
                {
                    _context.Workers.Add(worker);
                    await _context.SaveChangesAsync();
                    var idWorker = _context.Workers.FirstOrDefault(e => e.Name == worker.Name).IdWorker;
                    _context.WorkersHasTasks.Add(new WorkersHasTask
                    {
                        IdTask = idTask,
                        IdWorker = idWorker,
                        Quantity = worker.Quantity ?? 0
                    });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tasks", new { id = idTask });
            }
            if (worker.Salary == null)
            {
                ModelState["Salary"].Errors.Clear();
                ModelState.AddModelError("Salary", "Введенное не является зарплатой");
            }
            ViewBag.IdTask = idTask;
            return View(worker);
        }


        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(int? id, int  idTask)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            ViewBag.IdTask = idTask;
            worker.Quantity = _context.WorkersHasTasks.FirstOrDefault(mht => mht.IdTask == idTask && mht.IdWorker == worker.IdWorker).Quantity;
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Worker worker, int idTask)
        {
            if (id != worker.IdWorker)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var findWorker = await _context.Workers.FirstOrDefaultAsync(e => e.Name == worker.Name);
                    if (findWorker != null)
                    {
                        // Проверяем, существует ли уже связь между работником и задачей
                        var existingRelation = await _context.WorkersHasTasks
                            .FirstOrDefaultAsync(wht => wht.IdTask == idTask && wht.IdWorker == findWorker.IdWorker);

                        if (existingRelation != null)
                        {
                            // Перенаправляем на страницу подтверждения
                            ViewBag.ExistingRelation = existingRelation;
                            ViewBag.NewQuantity = worker.Quantity ?? 0;
                            ViewBag.IdTask = idTask;
                            return View("ConfirmQuantity", worker); // Создайте представление ConfirmQuantity
                        }
                        else
                        {
                            _context.Update(new WorkersHasTask
                            {
                                IdTask = idTask,
                                IdWorker = findWorker.IdWorker,
                                Quantity = worker.Quantity ?? 0,
                            });
                        }
                    }
                    else
                    {
                        _context.Workers.Update(worker);
                        await _context.SaveChangesAsync();
                        var idWorker = _context.Workers.FirstOrDefault(e => e.Name == worker.Name).IdWorker;
                        _context.WorkersHasTasks.Update(new WorkersHasTask
                        {
                            IdTask = idTask,
                            IdWorker = idWorker,
                            Quantity = worker.Quantity ?? 0
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.IdWorker))
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
            if (worker.Salary == null)
            {
                ModelState["Salary"].Errors.Clear();
                ModelState.AddModelError("Salary", "Введенное не является зарплатой");
            }
            ViewBag.IdTask = idTask;
            return View(worker);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int idTask, int idWorker, int newQuantity, string action)
        {
            var existingRelation = await _context.WorkersHasTasks
                .FirstOrDefaultAsync(wht => wht.IdTask == idTask && wht.IdWorker == idWorker);

            if (existingRelation != null)
            {
                if (action == "add")
                {
                    existingRelation.Quantity += newQuantity;
                }
                else if (action == "replace")
                {
                    existingRelation.Quantity = newQuantity;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }


        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id, int idTask)
        {
            var workersHasTask = await _context.WorkersHasTasks.FirstOrDefaultAsync(mht => mht.IdTask == idTask && mht.IdWorker == id);
            if (workersHasTask != null)
            {
                _context.WorkersHasTasks.Remove(workersHasTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }

        private bool WorkerExists(int id)
        {
            return _context.Workers.Any(e => e.IdWorker == id);
        }
    }
}
