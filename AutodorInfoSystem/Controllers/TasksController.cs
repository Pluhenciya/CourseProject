using AutodorInfoSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutodorInfoSystem.Controllers
{
    public class TasksController : Controller
    {
        private readonly AutodorContext _context;

        public TasksController(AutodorContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var autodorContext = _context.Tasks.Include(t => t.IdProjectNavigation);
            return View(await autodorContext.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.IdProjectNavigation)
                .Include(t => t.MaterialsHasTasks)
                    .ThenInclude(m => m.IdMaterialNavigation)
                .Include(t => t.EquipmentHasTasks)
                    .ThenInclude(e => e.IdEquipmentNavigation)
                .Include(t => t.WorkersHasTasks)
                    .ThenInclude(w => w.IdWorkerNavigation)
                .FirstOrDefaultAsync(m => m.IdTask == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create(int idProject)
        {
            ViewBag.IdProject = idProject;
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTask,Name,Description,IdProject")] Models.Task task)
        {
            ModelState.Remove("IdProjectNavigation");

            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Projects", task.IdProject); ;
            }
            return View();
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTask,Name,Description,ProjectsIdProject")] Models.Task task)
        {
            if (id != task.IdTask)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.IdTask))
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
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
			var task = await _context.Tasks.FindAsync(id);
			if (task != null)
			{
				_context.Tasks.Remove(task);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction("Details", "Projects", new { id = task.IdProject });
		}

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.IdTask == id);
        }
    }
}
