using AutodorInfoSystem.Data;
using AutodorInfoSystem.Models;
using AutodorInfoSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutodorInfoSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly AutodorContext _context;
        private readonly UserService _userService;
        private readonly TokenService _tokenService;

        public UsersController(AutodorContext context, UserService userService, TokenService tokenService)
        {
            _context = context;
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Include(u => u.Projecter).Include(u => u.Admin).ToListAsync());
        }

        public IActionResult Register()
        {
            return View();
        }

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

            var userExist = _userService.UserVerify(user);

            if (userExist == null)
            {
                TempData["ErrorMessage"] = "Неверный логин или пароль.";
                return Redirect("~/");
            }

            Response.Cookies.Append("A", _tokenService.CreateToken(userExist));

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("A");

            return Redirect("~/");
        }

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
