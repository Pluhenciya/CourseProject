using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutodorInfoApi.Data;
using AutodorInfoApi.Models;
using AutodorInfoApi.Services;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;

namespace AutodorInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            var userExist = _userService.UserVerify(user);

            if (userExist == null)
            {
                return Unauthorized(); // Возвращаем 401, если пользователь не найден
            }

            var token = _tokenService.CreateToken(userExist);
            return Ok(new { Token = token });
        }
    }
}
