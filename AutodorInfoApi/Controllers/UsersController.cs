using AutodorInfoApi.Data;
using AutodorInfoApi.Models;
using AutodorInfoApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.Include(u => u.Projecter).Include(u => u.Admin).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Admin) // Включаем связанные данные
                    .Include(u => u.Projecter)
                    .FirstOrDefaultAsync(u => u.IdUser == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user, [FromQuery] int? idProjecter, [FromQuery] string? role, [FromQuery] string? surname, [FromQuery] string? name, [FromQuery] string? patronymic)
        {
            try
            {
                // Получаем существующего пользователя из базы данных
                var oldUser = await _context.Users.Include(u=>u.Admin).Include(u => u.Projecter).FirstOrDefaultAsync(u => u.IdUser == user.IdUser);

                if (oldUser == null)
                {
                    return NotFound(); // Если пользователь не найден
                }

                // Обновляем свойства пользователя
                oldUser.Login = user.Login;
                oldUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Хешируем новый пароль

                // Определяем старую роль
                string oldRole = oldUser.Admin != null ? "Admin" : "Projecter";

                if (role == "admin")
                {
                    if (oldRole != "Admin")
                    {
                        // Удаляем проектировщика, если роль меняется
                        if (oldUser.Projecter != null)
                        {
                            oldUser.Projecter = null;
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
                            oldUser.Admin = null;
                        }
                        oldUser.Projecter = new Projecter
                        {
                            IdUser = oldUser.IdUser,
                            Surname = surname,
                            Name = name,
                            Patronymic = patronymic
                        };
                    }
                    else
                    {
                        // Если роль остается "Projecter", просто обновляем данные
                        oldUser.Projecter.Surname = surname;
                        oldUser.Projecter.Name = name;
                        oldUser.Projecter.Patronymic = patronymic;
                    }
                }
                _context.Users.Update(oldUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user, [FromQuery] string? role, [FromQuery] string? surname, [FromQuery] string? name, [FromQuery] string? patronymic)
        {
            if (role == "admin")
                user.Admin = new Admin
                {
                    UsersIdUser = user.IdUser
                };
            else if (role == "projecter")
                user.Projecter = new Projecter
                {
                    IdUser = user.IdUser,
                    Surname = surname,
                    Name = name,
                    Patronymic = patronymic
                };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.IdUser }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.IdUser == id);
        }
    }
}
