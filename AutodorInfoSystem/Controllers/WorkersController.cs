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
                    _context.Add(new WorkersHasTask
                    {
                        IdTask = idTask,
                        IdWorker = findWorker.IdWorker,
                        Quantity = worker.Quantity ?? 0,
                    });
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
                ModelState.AddModelError("Salary", "Введенное не является зарплптой");
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
                        _context.Add(new WorkersHasTask
                        {
                            IdTask = idTask,
                            IdWorker = findWorker.IdWorker,
                            Quantity = worker.Quantity ?? 0,
                        });
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
                ModelState.AddModelError("Salary", "Введенное не является зарплптой");
            }
            ViewBag.IdTask = idTask;
            return View(worker);
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
