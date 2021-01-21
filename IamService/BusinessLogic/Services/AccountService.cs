using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Exceptions;
using IamService.BusinessLogic.Helpers;
using IamService.DataAccess;
using IamService.Models;
using Microsoft.EntityFrameworkCore;

namespace IamService.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IamDbContext _dbContext;
        private readonly SecurityHelper _passwordHelper;
        public AccountService(IamDbContext dbContext, SecurityHelper passwordHelper)
        {
            _dbContext = dbContext;
            _passwordHelper = passwordHelper;
        }

        public async Task<User> CreateAsync(UserCreateModel model)
        {
            var userEntity = new User
            {
                UserName = model.UserName,
                Password = _passwordHelper.HashPassword(model.Password),
                CreatedOn = DateTime.Now,
                IsActive = true,
                UserRoles = model.Roles.Select(r => new UserRole { RoleId = r }).ToList()
            };

            _dbContext.Users.Add(userEntity);
            await _dbContext.SaveChangesAsync();
            return userEntity;
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            var userRoles = await _dbContext.UserRoles
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.Name).ToListAsync();
            return userRoles;
        }

        public async Task<LoginReponse> LoginAsync(LoginModel model)
        {
            var user = await _dbContext.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName == model.UserName);
            if (user == null || user.Password != _passwordHelper.HashPassword(model.Password))
            {
                throw new ValidationException("Invalid username or password");
            }
            var token = _passwordHelper.GenerateJwtToken(user);
            return new LoginReponse { UserName = user.UserName, Token = token };
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _dbContext.Roles.ToListAsync();
        }
    }
}

