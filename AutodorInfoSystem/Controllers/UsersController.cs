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
using Microsoft.IdentityModel.Tokens;

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
                var createUser = new User
                {
                    Login = user.Login,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
                };

                _context.Users.Add(createUser);
                await _context.SaveChangesAsync();

                var accessToken = _tokenService.CreateAccessToken(createUser);
                var refreshToken = await _tokenService.CreateRefreshToken(createUser);

                Response.Cookies.Append("AccessToken", accessToken);
                Response.Cookies.Append("RefreshToken", refreshToken);

                return RedirectToAction("Index", "Projects");
            }

            return View();
        }

        public IActionResult Login()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = new User
            {
                Login = login,
                Password = password
            };

            var userExist = await _userService.UserVerifyAsync(user);

            if (userExist == null)
            {
                TempData["ErrorMessage"] = "Неверный логин или пароль.";
                return Redirect("~/");
            }

            var accessToken = _tokenService.CreateAccessToken(userExist);
            var refreshToken = await _tokenService.CreateRefreshToken(userExist);

            Response.Cookies.Append("AccessToken", accessToken);
            Response.Cookies.Append("RefreshToken", refreshToken);

            return RedirectToAction("Index", "Projects");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var user = await _userService.GetUserByRefreshTokenAsync(refreshToken);
                if (user != null)
                {
                    await RevokeRefreshTokenAsync(user, refreshToken);
                }
            }

            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");

            return Redirect("~/");
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }

            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(refreshToken);
                var username = principal.Identity.Name;
                var user = await _userService.GetUserByUsernameAsync(username);

                if (user == null || user.RefreshTokens.Any(t => t.Token == refreshToken && t.IsRevoked))
                {
                    return Unauthorized();
                }

                var newAccessToken = _tokenService.CreateAccessToken(user);
                var newRefreshToken = await _tokenService.CreateRefreshToken(user);

                Response.Cookies.Append("AccessToken", newAccessToken);
                Response.Cookies.Append("RefreshToken", newRefreshToken);

                return Ok();
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }

        private async System.Threading.Tasks.Task RevokeRefreshTokenAsync(User user, string refreshToken)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken && t.UserId == user.IdUser);
            if (token != null)
            {
                token.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}