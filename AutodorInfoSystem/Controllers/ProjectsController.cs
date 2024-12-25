using AutodorInfoSystem.Data;
using AutodorInfoSystem.Models;
using AutodorInfoSystem.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
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
        private readonly HttpClientService _httpClientService;

        public ProjectsController(AutodorContext context, ExcelService excelService, HttpClientService httpClientService)
        {
            _context = context;
            _excelService = excelService;
            _httpClientService = httpClientService;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            // Проверяем, аутентифицирован ли пользователь
            if (User.Identity.IsAuthenticated)
            {
                // Если пользователь имеет роль "Admin"
                if (User.IsInRole("Admin"))
                    // Получаем список всех проектов и возвращаем представление с этими данными
                    return View(await _httpClientService.GetHttpClient().GetFromJsonAsync<List<Project>>("Projects"));

                // Если пользователь не администратор, получаем проекты, связанные с конкретным пользователем
                return View(await _httpClientService.GetHttpClient().GetFromJsonAsync<List<Project>>($"Projects?idProjecter={Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)}"));
            }
            else
            {
                // Если пользователь не аутентифицирован, получаем только завершенные проекты
                return View(await _httpClientService.GetHttpClient().GetFromJsonAsync<List<Project>>("Projects?isCompleted=true"));
            }
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClientService.GetHttpClient().GetAsync($"Projects/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var project = await response.Content.ReadFromJsonAsync<Project>();
            return View(project);
        }

        // GET: Projects/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("Admin"))
            {
                var projecters = await _httpClientService.GetHttpClient().GetFromJsonAsync<List<Projecter>>("Projecters");
                ViewBag.Projecters = new SelectList(projecters, "IdUser", "LongName");
            }
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProject,Name,Description,IsCompleted")] Project project, int? idProjecter)
        {
            ModelState.Remove("IdProject");

            if (ModelState.IsValid)
            {
                Projecter projecter;
                HttpResponseMessage response;
                if (User.IsInRole("Admin"))
                {

                    response = await _httpClientService.GetHttpClient().PostAsJsonAsync($"Projects?idProjecter={idProjecter}", project);
                }
                else
                {
                    var projecterId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    response = await _httpClientService.GetHttpClient().PostAsJsonAsync($"Projects?idProjecter={projecterId}", project);
                }

                if (response.IsSuccessStatusCode)
                {
                    var createdProject = await response.Content.ReadFromJsonAsync<Project>();
                    return RedirectToAction("Details", new { id = createdProject.IdProject });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Не удалось создать проект. Пожалуйста, попробуйте еще раз.");
                }
            }

            if (User.IsInRole("Admin"))
            {
                var projecters = await _httpClientService.GetHttpClient().GetFromJsonAsync<List<Projecter>>("Projecters");
                ViewBag.Projecters = new SelectList(projecters, "IdUser", "LongName");
            }
            return View(project);
        }



        // GET: Projects/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _httpClientService.GetHttpClient().GetFromJsonAsync<Project>($"Projects/{id}");
            if (project == null)
            {
                return NotFound();
            }
            if (User.IsInRole("Admin"))
            {
                var projecters = await _httpClientService.GetHttpClient().GetFromJsonAsync<List<Projecter>>("Projecters");
                ViewBag.Projecters = new SelectList(projecters, "IdUser", "LongName");
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
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
                    var existingProject = await _httpClientService.GetHttpClient().GetFromJsonAsync<Project>($"Projects/{id}");

                    if (existingProject == null)
                    {
                        return NotFound();
                    }

                    // Если пользователь - администратор, добавляем нового проектировщика
                    if (User.IsInRole("Admin") && idProjecter.HasValue)
                    {
                        // Удаляем старую связь с проектировщиком
                        existingProject.ProjectersIdUsers.Clear();
                        var projecter = await _httpClientService.GetHttpClient().GetFromJsonAsync<Projecter>($"Projecters/{idProjecter}");
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
                    await _httpClientService.GetHttpClient().PutAsJsonAsync($"Projects/{id}", project);
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
                var projecters = await _httpClientService.GetHttpClient().GetFromJsonAsync<List<Projecter>>("Projecters");
                ViewBag.Projecters = new SelectList(projecters, "IdUser", "LongName");
            }
            return View(project);
        }


        // GET: Projects/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            await _httpClientService.GetHttpClient().DeleteAsync($"Projects/{id}");
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DownloadTable(int idProject)
        {
            if (idProject == null)
            {
                return NotFound();
            }

            var project = await _httpClientService.GetHttpClient().GetFromJsonAsync<Project>($"Projects/{idProject}");

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
