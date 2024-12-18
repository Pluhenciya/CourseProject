using AutodorInfoSystem.Data;
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
        private readonly AutodorContext _context;
        private readonly HttpClientService _httpClientService;

        public UsersController(AutodorContext context, HttpClientService httpClientService)
        {
            _context = context;
            _httpClientService = httpClientService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Include(u => u.Projecter).Include(u => u.Admin).ToListAsync());
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
            if (ModelState.IsValid)
            {
                var createdUser = new User
                {
                    Login = user.Login,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
                };
                if (user.Role == "Admin")
                    createdUser.Admin = new Admin
                    {
                        UsersIdUser = user.IdUser
                    };
                else
                    createdUser.Projecter = new Projecter
                    {
                        IdUser = user.IdUser,
                        Surname = user.Surname,
                        Name = user.Name,
                        Patronymic = user.Patronymic
                    };
                _context.Users.Add(createdUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.Include(u => u.Projecter).FirstOrDefaultAsync(u => u.IdUser == id);

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
            if (ModelState.IsValid)
            {
                // Получаем существующего пользователя из базы данных
                var oldUser = await _context.Users
                    .Include(u => u.Admin) // Включаем связанные данные
                    .Include(u => u.Projecter)
                    .FirstOrDefaultAsync(u => u.IdUser == user.IdUser);

                if (oldUser == null)
                {
                    return NotFound(); // Если пользователь не найден
                }

                // Обновляем свойства пользователя
                oldUser.Login = user.Login;
                oldUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Хешируем новый пароль

                // Определяем старую роль
                string oldRole = oldUser.Admin != null ? "Admin" : "Projecter";

                if (user.Role == "Admin")
                {
                    if (oldRole != "Admin")
                    {
                        // Удаляем проектировщика, если роль меняется
                        if (oldUser.Projecter != null)
                        {
                            _context.Projecters.Remove(oldUser.Projecter);
                        }
                        oldUser.Admin = new Admin
                        {
                            UsersIdUser = oldUser.IdUser
                        };
                    }
                }
                else // Если роль "Projecter"
                {
                    if (oldRole != "Projecter")
                    {
                        // Удаляем администратора, если роль меняется
                        if (oldUser.Admin != null)
                        {
                            _context.Admins.Remove(oldUser.Admin);
                        }
                        oldUser.Projecter = new Projecter
                        {
                            IdUser = oldUser.IdUser,
                            Surname = user.Surname,
                            Name = user.Name,
                            Patronymic = user.Patronymic
                        };
                    }
                    else
                    {
                        // Если роль остается "Projecter", просто обновляем данные
                        oldUser.Projecter.Surname = user.Surname;
                        oldUser.Projecter.Name = user.Name;
                        oldUser.Projecter.Patronymic = user.Patronymic;
                    }
                }

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();
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
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
