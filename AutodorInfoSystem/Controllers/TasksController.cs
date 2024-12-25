using AutodorInfoSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutodorInfoSystem.Controllers
{
    public class TasksController : Controller
    {
        private readonly HttpClientService _httpClientService;

        public TasksController(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _httpClientService.GetHttpClient().GetFromJsonAsync<Models.Task>($"Tasks/{id}");
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        [Authorize]
        public IActionResult Create(int idProject)
        {
            ViewBag.IdProject = idProject;
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTask,Name,Description,IdProject")] Models.Task task)
        {
            ModelState.Remove("IdProjectNavigation");

            if (ModelState.IsValid)
            {
                var response = await _httpClientService.GetHttpClient().PostAsJsonAsync("Tasks", task);
                if (response.IsSuccessStatusCode)
                {
                    task = await response.Content.ReadFromJsonAsync<Models.Task>();
                }
                return RedirectToAction("Details", "Tasks", new { id = task.IdTask }); ;
            }
            ViewBag.IdProject = task.IdProject;
            return View();
        }

        // GET: Tasks/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _httpClientService.GetHttpClient().GetFromJsonAsync<Models.Task>($"Tasks/{id}");
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTask,Name,Description,IdProject,Cost")] Models.Task task)
        {
            if (id != task.IdTask)
            {
                return NotFound();
            }

            ModelState.Remove("IdProjectNavigation");

            if (ModelState.IsValid)
            {
                    await _httpClientService.GetHttpClient().PutAsJsonAsync($"Tasks/{id}", task);
                return RedirectToAction("Details", "Projects", new { id = task.IdProject });
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var task = await _httpClientService.GetHttpClient().GetFromJsonAsync<Models.Task>($"Tasks/{id}");
            await _httpClientService.GetHttpClient().DeleteAsync($"Tasks/{id}");
			return RedirectToAction("Details", "Projects", new { id = task.IdProject });
		}
    }
}
