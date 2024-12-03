using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutodorInfoSystem.Data;
using AutodorInfoSystem.Models;
using AutodorInfoSystem.Services;

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

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Login, Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var createdUser = new User
                {
                    Login = user.Login,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
                };

                _context.Users.Add(createdUser);
                await _context.SaveChangesAsync();
                return Redirect("~/");
            }
            return Redirect("~/");
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

            return RedirectToAction("Index", "Projects");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("A");

            return Redirect("~/");
        }
    }
}
