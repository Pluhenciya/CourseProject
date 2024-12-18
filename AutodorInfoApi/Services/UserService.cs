using AutodorInfoApi.Data;
using AutodorInfoApi.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AutodorInfoApi.Services
{
    public class UserService(AutodorContext context)
    {
        private readonly AutodorContext _dbContext = context;

        public User? UserVerify(User user)
        {
            var userExist = _dbContext.Users.Include(u => u.Admin).FirstOrDefault(u => u.Login == user.Login);

            if (userExist != null && BCrypt.Net.BCrypt.Verify(user.Password, userExist.Password))
            {
                return userExist;
            }

            return null;
        }
    }
}
