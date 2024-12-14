using AutodorInfoSystem.Data;
using AutodorInfoSystem.Models;
using AutodorInfoSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AutodorInfoSystem.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly AutodorContext _context;
        private readonly ExcelService _excelService;

        public ProjectsController(AutodorContext context, ExcelService excelService)
        {
            _context = context;
            _excelService = excelService;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return View(await _context.Projects.ToListAsync());

                return View(await _context.Projects
                    .Include(p => p.ProjectersIdUsers)
                    .Where(p => p.ProjectersIdUsers.Any(p => p.IdUser == Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)))
                    .ToListAsync());
            }
            else
                return View(await _context.Projects.Where(p => p.IsCompleted).ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.ProjectersIdUsers)
                .FirstOrDefaultAsync(m => m.IdProject == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("Admin"))
            {
                var projecters = await _context.Projecters.ToListAsync();
                ViewBag.Projecters = new SelectList(projecters, "IdUser", "LongName");
            }
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProject,Name,Description,IsCompleted")] Project project, int? idProjecter)
        {
            ModelState.Remove("IdProject");

            if (ModelState.IsValid)
            {
                if (User.IsInRole("Admin"))
                {
                    var projecter = await _context.Projecters.FindAsync(idProjecter.Value);
                    project.ProjectersIdUsers.Add(projecter);
                }
                else
                {
                    var projecterId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var projecter = await _context.Projecters.FindAsync(projecterId);
                    project.ProjectersIdUsers.Add(projecter);
                }
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Если модель не валидна, повторно получаем проектировщиков для передачи в представление
            if (User.IsInRole("Admin"))
            {
                var projecters = await _context.Projecters.ToListAsync();
                ViewBag.Projecters = new SelectList(projecters, "IdUser", "LongName");
            }
            return View(project);
        }


        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            if (User.IsInRole("Admin"))
            {
                var projecters = await _context.Projecters.ToListAsync();
                ViewBag.Projecters = new SelectList(projecters, "IdUser", "LongName");
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProject,Name,Description,IsCompleted,Cost")] Project project, int? idProjecter)
        {
            if (id != project.IdProject)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Получаем текущий проект из базы данных
                    var existingProject = await _context.Projects
                        .Include(p => p.ProjectersIdUsers) // Включаем проектировщиков
                        .FirstOrDefaultAsync(p => p.IdProject == id);

                    if (existingProject == null)
                    {
                        return NotFound();
                    }

                    // Удаляем старую связь с проектировщиком
                    existingProject.ProjectersIdUsers.Clear();

                    // Если пользователь - администратор, добавляем нового проектировщика
                    if (User.IsInRole("Admin") && idProjecter.HasValue)
                    {
                        var projecter = await _context.Projecters.FindAsync(idProjecter.Value);
                        if (projecter != null)
                        {
                            existingProject.ProjectersIdUsers.Add(projecter);
                        }
                    }

                    // Обновляем остальные свойства проекта
                    existingProject.Name = project.Name;
                    existingProject.Description = project.Description;
                    existingProject.IsCompleted = project.IsCompleted;
                    existingProject.Cost = project.Cost;

                    // Обновляем проект в контексте
                    _context.Update(existingProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.IdProject))
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

            // Если модель не валидна, повторно получаем проектировщиков для передачи в представление
            if (User.IsInRole("Admin"))
            {
                var projecters = await _context.Projecters.ToListAsync();
                ViewBag.Projecters = new SelectList(projecters, "IdUser", "LongName");
            }
            return View(project);
        }


        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DownloadTable(int idProject)
        {
            if (idProject == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Tasks)
                .ThenInclude(p => p.MaterialsHasTasks)
                .ThenInclude(p => p.IdMaterialNavigation)
                .Include(p => p.Tasks)
                .ThenInclude(p => p.EquipmentHasTasks)
                .ThenInclude(p => p.IdEquipmentNavigation)
                .Include(p => p.Tasks)
                .ThenInclude(p => p.WorkersHasTasks)
                .ThenInclude(p => p.IdWorkerNavigation)
                .FirstOrDefaultAsync(p => p.IdProject == idProject);

            if (project == null)
            {
                return NotFound();
            }

            return File(await _excelService.GenerateProjectReportAsync(project), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{project.Name}.xlsx");
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.IdProject == id);
        }
    }
}
