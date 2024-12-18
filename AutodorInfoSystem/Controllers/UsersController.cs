using AutodorInfoSystem.Models;
using AutodorInfoSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AutodorInfoSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClientService _httpClientService;

        public UsersController(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _httpClientService.GetHttpClient().GetFromJsonAsync<List<User>>("Users"));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Surname");
            ModelState.Remove("Patronimyc");
            if (ModelState.IsValid)
            {
                var createdUser = new User
                {
                    Login = user.Login,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
                };
                if (user.Role == "Admin")
                    await _httpClientService.GetHttpClient().PostAsJsonAsync("Users?role=admin", user);
                else
                    await _httpClientService.GetHttpClient().PostAsJsonAsync($"Users?role=projecter&surname={user.Surname}&name={user.Name}&patronymic={user.Patronymic}", user);
                await _httpClientService.GetHttpClient().PostAsJsonAsync("Users", user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _httpClientService.GetHttpClient().GetFromJsonAsync<User>($"Users/{id}");

            user.Role = "Admin";
            if (user.Projecter != null)
            {
                user.Role = "Projecter";
                user.Surname = user.Projecter.Surname;
                user.Name = user.Projecter.Name;
                user.Patronymic = user.Projecter.Patronymic;
            }
            user.Password = null;
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Surname");
            ModelState.Remove("Patronimyc");
            if (ModelState.IsValid)
            {
                // Получаем существующего пользователя из базы данных
                var oldUser = await _httpClientService.GetHttpClient().GetFromJsonAsync<User>($"Users/{user.IdUser}");
                
                // Сохраняем изменения в базе данных
                await _httpClientService.GetHttpClient().PutAsJsonAsync($"Users/{oldUser.IdUser}?role={user.Role.ToLower()}&surname={user.Surname}&name={user.Name}&patronymic={user.Patronymic}", user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = new User
            {
                Login = login,
                Password = password
            };
            var response = await _httpClientService.GetHttpClient().PostAsJsonAsync("Users/login", user);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                Response.Cookies.Append("A", tokenResponse.Token); // Предполагается, что токен находится в свойстве Token
            }
            else
            {
                TempData["ErrorMessage"] = "Неверный логин или пароль.";
                return Redirect("~/");
            }
            return Redirect("~/");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("A");

            return Redirect("~/");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            await _httpClientService.GetHttpClient().DeleteAsync($"Users/{id}");
            return RedirectToAction("Index");
        }

    }
}
