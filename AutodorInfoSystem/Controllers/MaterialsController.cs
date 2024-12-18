using AutodorInfoSystem.Models;
using AutodorInfoSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutodorInfoSystem.Controllers
{
    [Authorize]
    public class MaterialsController : Controller
    {
        private readonly HttpClientService _httpClientService;

        public MaterialsController(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        [HttpGet]
        public async Task<JsonResult> GetSimilarNames(string name)
        {
            var similarItems = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<List<Material>>($"Materials?name={name}&isDeleted=false");
            return Json(similarItems);
        }

        // GET: Materials
        public async Task<IActionResult> Index()
        {
            var materials = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<List<Material>>("Materials?isDeleted=false");
            return View(materials);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SimpleCreate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SimpleCreate(Material material)
        {
            ModelState.Remove("Quantity");
            if (ModelState.IsValid)
            {
                await _httpClientService.GetHttpClient().PostAsJsonAsync("Materials", material);
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SimpleEdit(int id)
        {
            var material = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<Material>($"Materials/one?id={id}");
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        [Authorize(Roles = "Admin")]
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
                await _httpClientService.GetHttpClient().PutAsJsonAsync($"Materials/{id}", material);
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SimpleDelete(int id)
        {
            var material = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<Material>($"Materials/one?id={id}");
            if (material != null)
            {
                material.IsDeleted = true;
                await _httpClientService.GetHttpClient().PutAsJsonAsync($"Materials/{id}", material);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Materials/Create
        public IActionResult Create(int idTask)
        {
            ViewBag.IdTask = idTask;
            return View();
        }

        // POST: Materials/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material, int idTask)
        {
            if (ModelState.IsValid)
            {
                var findMaterial = await _httpClientService.GetHttpClient()
                    .GetFromJsonAsync<Material>($"Materials/one?name={material.Name}");
                if (findMaterial != null)
                {
                    // Проверяем, существует ли уже связь между материалом и задачей
                    var existingRelation = await _httpClientService.GetHttpClient()
                        .GetFromJsonAsync<MaterialsHasTask>($"MaterialsHasTasks/{idTask}/{findMaterial.IdMaterial}");

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
                        await _httpClientService.GetHttpClient().PostAsJsonAsync("MaterialsHasTasks", new MaterialsHasTask
                        {
                            IdTask = idTask,
                            IdMaterial = findMaterial.IdMaterial,
                            Quantity = material.Quantity ?? 0,
                        });
                    }
                }
                else
                {
                    await _httpClientService.GetHttpClient().PostAsJsonAsync("Materials", material);
                    var idMaterial = (await _httpClientService.GetHttpClient()
                        .GetFromJsonAsync<Material>($"Materials/one?name={material.Name}")).IdMaterial;

                    await _httpClientService.GetHttpClient().PostAsJsonAsync("MaterialsHasTasks", new MaterialsHasTask
                    {
                        IdTask = idTask,
                        IdMaterial = idMaterial,
                        Quantity = material.Quantity ?? 0
                    });
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

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id, int idTask)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<Material>($"Materials/one?id={id}");
            if (material == null)
            {
                return NotFound();
            }
            ViewBag.IdTask = idTask;
            var existingRelation = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<MaterialsHasTask>($"MaterialsHasTasks/{idTask}/{material.IdMaterial}");
            material.Quantity = existingRelation?.Quantity ?? 0;
            return View(material);
        }

        // POST: Materials/Edit/5
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
                    var findMaterial = await _httpClientService.GetHttpClient()
                        .GetFromJsonAsync<Material>($"Materials/one?name={material.Name}");
                    if (findMaterial != null)
                    {
                        await _httpClientService.GetHttpClient().PutAsJsonAsync("MaterialsHasTasks", new MaterialsHasTask
                        {
                            IdTask = idTask,
                            IdMaterial = findMaterial.IdMaterial,
                            Quantity = material.Quantity ?? 0,
                        });
                    }
                    else
                    {
                        await _httpClientService.GetHttpClient().PutAsJsonAsync($"Materials/{id}", material);
                        var idMaterial = (await _httpClientService.GetHttpClient()
                            .GetFromJsonAsync<Material>($"Materials/one?name={material.Name}")).IdMaterial;

                        await _httpClientService.GetHttpClient().PutAsJsonAsync("MaterialsHasTasks", new MaterialsHasTask
                        {
                            IdTask = idTask,
                            IdMaterial = idMaterial,
                            Quantity = material.Quantity ?? 0
                        });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Обработка исключения, если материал не существует
                    if (!await MaterialExists(material.IdMaterial))
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
            if (id == null)
            {
                return NotFound();
            }

            var materialsHasTask = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<MaterialsHasTask>($"MaterialsHasTasks/{idTask}/{id}");
            if (materialsHasTask != null)
            {
                await _httpClientService.GetHttpClient().DeleteAsync($"MaterialsHasTasks/{idTask}/{id}");
            }

            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int idTask, int idMaterial, int newQuantity, string action)
        {
            var existingRelation = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<MaterialsHasTask>($"MaterialsHasTasks/{idTask}/{idMaterial}");

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

                await _httpClientService.GetHttpClient().PutAsJsonAsync("MaterialsHasTasks", existingRelation);
            }

            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }

        private async Task<bool> MaterialExists(int id)
        {
            var material = await _httpClientService.GetHttpClient()
                .GetFromJsonAsync<Material>($"Materials/one?id={id}");
            return material != null;
        }
    }
}