using AutodorInfoSystem.Models;
using AutodorInfoSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutodorInfoSystem.Controllers
{
    [Authorize]
    public class WorkersController : Controller
    {
        private readonly HttpClientService _httpClientService;

        public WorkersController(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        [HttpGet]
        public async Task<JsonResult> GetSimilarNames(string name)
        {
            var similarItems = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<List<Worker>>($"Workers?name={name}&isDeleted=false");
            return Json(similarItems);
        }

        // GET: Workers
        public async Task<IActionResult> Index()
        {
            var workers = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<List<Worker>>("Workers?isDeleted=false");
            return View(workers);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SimpleCreate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SimpleCreate(Worker worker)
        {
            ModelState.Remove("Quantity");
            if (ModelState.IsValid)
            {
                await _httpClientService.GetHttpClient().PostAsJsonAsync("Workers", worker);
                return RedirectToAction(nameof(Index));
            }
            return View(worker);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SimpleEdit(int id)
        {
            var worker = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<Worker>($"Workers/one?id={id}");
            if (worker == null)
            {
                return NotFound();
            }
            return View(worker);
        }

        [Authorize(Roles = "Admin")]
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
                    await _httpClientService.GetHttpClient().PutAsJsonAsync($"Workers/{id}", worker);
                }
                catch (HttpRequestException)
                {
                    if (!await WorkerExists(worker.IdWorker))
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SimpleDelete(int id)
        {
            var worker = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<Worker>($"Workers/one?id={id}");
            
            if (worker != null)
            {
                worker.IsDeleted = true;
                await _httpClientService.GetHttpClient().PutAsJsonAsync($"Workers/{id}", worker);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Workers/Create
        public IActionResult Create(int idTask)
        {
            ViewBag.IdTask = idTask;
            return View();
        }

        [HttpPost]
        // POST: Workers/Create
        public async Task<IActionResult> Create(Worker worker, int idTask)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClientService.GetHttpClient().GetAsync($"Workers/one?name={worker.Name}");
                if (response.IsSuccessStatusCode)
                {
                    var findWorker = await response.Content.ReadFromJsonAsync<Worker>();
                    response = await _httpClientService.GetHttpClient().GetAsync($"WorkersHasTasks/{idTask}/{findWorker.IdWorker}");

                    if (response.IsSuccessStatusCode)
                    {
                        var existingRelation = await response.Content.ReadFromJsonAsync<WorkersHasTask>();
                        ViewBag.ExistingRelation = existingRelation;
                        ViewBag.NewQuantity = worker.Quantity ?? 0;
                        ViewBag.IdTask = idTask;
                        return View("ConfirmQuantity", worker); // Создайте представление ConfirmQuantity
                    }
                    else
                    {
                        await _httpClientService.GetHttpClient().PostAsJsonAsync("WorkersHasTask", new WorkersHasTask
                        {
                            IdTask = idTask,
                            IdWorker = findWorker.IdWorker,
                            Quantity = worker.Quantity ?? 0,
                        });
                    }
                }
                else
                {
                    await _httpClientService.GetHttpClient().PostAsJsonAsync("Workers", worker);
                    var idWorker = (await _httpClientService.GetHttpClient()
                        .GetFromJsonAsync<Worker>($"Workers/one?name={worker.Name}")).IdWorker;

                    await _httpClientService.GetHttpClient().PostAsJsonAsync("WorkersHasTasks", new WorkersHasTask
                    {
                        IdTask = idTask,
                        IdWorker = idWorker,
                        Quantity = worker.Quantity ?? 0
                    });
                }
                return RedirectToAction("Details", "Tasks", new { id = idTask });
            }
            if (worker.Salary == null)
            {
                ModelState["Salary"].Errors.Clear();
                ModelState.AddModelError("Salary", "Введенное не является ценой");
            }
            ViewBag.IdTask = idTask;
            return View(worker);
        }

        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(int? id, int idTask)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<Worker>($"Workers/one?id={id}");
            if (worker == null)
            {
                return NotFound();
            }
            ViewBag.IdTask = idTask;
            var existingRelation = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<WorkersHasTask>($"WorkersHasTasks/{idTask}/{worker.IdWorker}");
            worker.Quantity = existingRelation?.Quantity ?? 0;
            return View(worker);
        }

        // POST: Workers/Edit/5
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
                    var findWorker = await _httpClientService.GetHttpClient()
                        .GetFromJsonAsync<Worker>($"Workers/one?name={worker.Name}");
                    if (findWorker != null)
                    {
                        // Проверяем, существует ли уже связь между работником и задачей
                        var existingRelation = await _httpClientService.GetHttpClient()
                            .GetFromJsonAsync<WorkersHasTask>($"WorkersHasTasks/{idTask}/{findWorker.IdWorker}");

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
                            await _httpClientService.GetHttpClient().PutAsJsonAsync("WorkersHasTasks", new WorkersHasTask
                            {
                                IdTask = idTask,
                                IdWorker = findWorker.IdWorker,
                                Quantity = worker.Quantity ?? 0,
                            });
                        }
                    }
                    else
                    {
                        await _httpClientService.GetHttpClient().PutAsJsonAsync($"Workers/{id}", worker);
                        var idWorker = (await _httpClientService.GetHttpClient()
                            .GetFromJsonAsync<Worker>($"Workers/one?name={worker.Name}")).IdWorker;

                        await _httpClientService.GetHttpClient().PutAsJsonAsync("WorkersHasTasks", new WorkersHasTask
                        {
                            IdTask = idTask,
                            IdWorker = idWorker,
                            Quantity = worker.Quantity ?? 0
                        });
                    }
                }
                catch (HttpRequestException)
                {
                    if (!await WorkerExists(worker.IdWorker))
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

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id, int idTask)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workersHasTask = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<WorkersHasTask>($"WorkersHasTasks/{idTask}/{id}");
            if (workersHasTask != null)
            {
                await _httpClientService.GetHttpClient().DeleteAsync($"WorkersHasTasks/{idTask}/{id}");
            }

            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int idTask, int idWorker, int newQuantity, string action)
        {
            var existingRelation = await _httpClientService.GetHttpClient()
                                .GetFromJsonAsync<WorkersHasTask>($"WorkersHasTasks/{idTask}/{idWorker}");

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

                await _httpClientService.GetHttpClient().PutAsJsonAsync("WorkersHasTasks", existingRelation);
            }

            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }

        private async Task<bool> WorkerExists(int id)
        {
            var worker = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<Worker>($"Workers/one?id={id}");
            return worker != null;
        }
    }
}

