using AutodorInfoSystem.Data;
using AutodorInfoSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AutodorInfoSystem.Services
{
    public class UserService
    {
        private readonly AutodorContext _dbContext;

        public UserService(AutodorContext context)
        {
            _dbContext = context;
        }

        public async Task<User?> UserVerifyAsync(User user)
        {
            var userExist = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

            if (userExist != null && BCrypt.Net.BCrypt.Verify(user.Password, userExist.Password))
            {
                return userExist;
            }

            return null;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == username);
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var token = await _dbContext.RefreshTokens.Include(t => t.User).FirstOrDefaultAsync(t => t.Token == refreshToken);
            return token?.User;
        }
    }
}
