using AutodorInfoSystem.Data;
using AutodorInfoSystem.Models;
using AutodorInfoSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutodorInfoSystem.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly AutodorContext _context;
        private readonly HttpClientService _httpClientService;

        public EquipmentsController(AutodorContext context, HttpClientService httpClientService)
        {
            _context = context;
            _httpClientService = httpClientService;
        }

        [HttpGet]
        public async Task<JsonResult> GetSimilarNames(string name)
        {
            var similarItems = await _httpClientService.GetHttpClient().GetFromJsonAsync<List<Equipment>>($"Equipments?name={name}&isDeleted=false");
            return Json(similarItems);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var equipments = await _httpClientService.GetHttpClient().GetFromJsonAsync<List<Equipment>>("Equipments?isDeleted=false");
            return View(equipments);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SimpleCreate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SimpleCreate(Equipment equipment)
        {
            ModelState.Remove("Quantity");
            if (ModelState.IsValid)
            {
                await _httpClientService.GetHttpClient().PostAsJsonAsync("Equipments", equipment);
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SimpleEdit(int id)
        {
            var equipment = await _httpClientService.GetHttpClient().GetFromJsonAsync<Equipment>($"Equipments/one?id={id}");
            if (equipment == null)
            {
                return NotFound();
            }
            return View(equipment);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SimpleEdit(int id, Equipment equipment)
        {
            if (id != equipment.IdEquipment)
            {
                return NotFound();
            }
            ModelState.Remove("Quantity");
            if (ModelState.IsValid)
            {
                await _httpClientService.GetHttpClient().PutAsJsonAsync($"Equipments/{id}", equipment);
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SimpleDelete(int id)
        {
            var equipment = await _httpClientService.GetHttpClient().GetFromJsonAsync<Equipment>($"Equipments/one?id={id}");
            if (equipment != null)
            {
                equipment.IsDeleted = true;
                await _httpClientService.GetHttpClient().PutAsJsonAsync($"Equipments/{id}", equipment);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Equipments/Create
        [Authorize]
        public IActionResult Create(int idTask)
        {
            ViewBag.IdTask = idTask;
            return View();
        }

        // POST: Equipments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Equipment equipment, int idTask)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClientService.GetHttpClient().GetAsync($"Equipments/one?name={equipment.Name}");
                if (response.IsSuccessStatusCode)
                {
                    var findEquipment = await response.Content.ReadFromJsonAsync<Equipment>();
                    response = await _httpClientService.GetHttpClient().GetAsync($"EquipmentHasTasks/{idTask}/{findEquipment.IdEquipment}");

                    if (response.IsSuccessStatusCode)
                    {
                        var existingRelation = await response.Content.ReadFromJsonAsync<EquipmentHasTask>();
                        ViewBag.ExistingRelation = existingRelation;
                        ViewBag.NewQuantity = equipment.Quantity ?? 0;
                        ViewBag.IdTask = idTask;
                        return View("ConfirmQuantity", equipment);
                    }
                    else
                    {
                        await _httpClientService.GetHttpClient().PostAsJsonAsync("EquipmentHasTasks", new EquipmentHasTask
                        {
                            IdTask = idTask,
                            IdEquipment = findEquipment.IdEquipment,
                            Quantity = equipment.Quantity ?? 0,
                        });
                    }
                }
                else
                {
                    await _httpClientService.GetHttpClient().PostAsJsonAsync("Equipments", equipment);
                    var idEquipment = (await _httpClientService.GetHttpClient().GetFromJsonAsync<Equipment>($"Equipments/one?name={equipment.Name}")).IdEquipment;
                    await _httpClientService.GetHttpClient().PostAsJsonAsync("EquipmentHasTasks", new EquipmentHasTask
                    {
                        IdTask = idTask,
                        IdEquipment = idEquipment,
                        Quantity = equipment.Quantity ?? 0
                    });
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

        // GET: Equipments/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id, int idTask)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _httpClientService.GetHttpClient().GetFromJsonAsync<Equipment>($"Equipments/one?id={id}");
            if (equipment == null)
            {
                return NotFound();
            }
            ViewBag.IdTask = idTask;
            equipment.Quantity = (await _httpClientService.GetHttpClient().GetFromJsonAsync<EquipmentHasTask>($"EquipmentHasTasks/{idTask}/{equipment.IdEquipment}")).Quantity;
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
                    var findEquipment = await _httpClientService.GetHttpClient().GetFromJsonAsync<Equipment>($"Equipments/one?name={equipment.Name}");
                    if (findEquipment != null)
                    {
                        await _httpClientService.GetHttpClient().PutAsJsonAsync($"EquipmentHasTasks", new EquipmentHasTask
                        {
                            IdTask = idTask,
                            IdEquipment = findEquipment.IdEquipment,
                            Quantity = equipment.Quantity ?? 0
                        });
                    }
                    else
                    {
                        await _httpClientService.GetHttpClient().PutAsJsonAsync($"Equipments/{id}", equipment);
                        var idEquipment = (await _httpClientService.GetHttpClient().GetFromJsonAsync<Equipment>($"Equipments/one?name={equipment.Name}")).IdEquipment;
                        await _httpClientService.GetHttpClient().PutAsJsonAsync($"EquipmentHasTasks", new EquipmentHasTask
                        {
                            IdTask = idTask,
                            IdEquipment = idEquipment,
                            Quantity = equipment.Quantity ?? 0
                        });
                    }
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
        [Authorize]
        public async Task<IActionResult> Delete(int? id, int idTask)
        {
            if (id == null)
            {
                return NotFound();
            }
                await _httpClientService.GetHttpClient().DeleteAsync($"EquipmentHasTasks/{idTask}/{id}");
            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity(int idTask, int idEquipment, int newQuantity, string action)
        {
            var existingRelation = await _httpClientService.GetHttpClient().GetFromJsonAsync<EquipmentHasTask>($"EquipmentHasTasks/{idTask}/{idEquipment}");

            if (action == "add")
            {
                existingRelation.Quantity += newQuantity;
            }
            else if (action == "replace")
            {
                existingRelation.Quantity = newQuantity;
            }
            await _httpClientService.GetHttpClient().PutAsJsonAsync($"EquipmentHasTasks", existingRelation);
            return RedirectToAction("Details", "Tasks", new { id = idTask });
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.IdEquipment == id);
        }
    }
}

