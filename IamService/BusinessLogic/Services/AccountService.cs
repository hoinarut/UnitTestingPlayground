using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Exceptions;
using IamService.BusinessLogic.Helpers;
using IamService.DataAccess;
using IamService.DataAccess.DTOs;
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

        public async Task<UserDto> CreateAsync(UserCreateModel model)
        {
            if (await _dbContext.Users.AnyAsync(u => u.UserName == model.UserName))
            {
                throw new ValidationException(Constants.ServiceMessages.CREATE_USER_USERNAME_IN_USE);
            }
            if (await _dbContext.UserProfiles.AnyAsync(up => up.EmailAddress == model.Profile.EmailAddress))
            {
                throw new ValidationException(Constants.ServiceMessages.CREATE_USER_EMAIL_IN_USE);
            }

            if (model.Roles == null || !model.Roles.Any())
            {
                throw new ValidationException(Constants.ServiceMessages.CREATE_USER_NO_ROLE);
            }

            var validRoles = await _dbContext.Roles.Select(r => r.Id).ToListAsync();
            if (model.Roles.Except(validRoles).Any())
            {
                throw new ValidationException(Constants.ServiceMessages.CREATE_USER_INVALID_ROLE);
            }

            var userEntity = new User
            {
                UserName = model.UserName,
                Password = _passwordHelper.HashPassword(model.Password),
                CreatedOn = DateTime.Now,
                IsActive = true,
                Roles = model.Roles.Select(r => new UserRole { RoleId = r }).ToList()
            };

            _dbContext.Users.Add(userEntity);
            await _dbContext.SaveChangesAsync();
            return userEntity.ToDto(false);
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
            {
                throw new ValidationException("Invalid UserId");
            }
            var userRoles = await _dbContext.UserRoles
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.Name).ToListAsync();
            return userRoles;
        }

        public async Task<LoginResponse> LoginAsync(LoginModel model)
        {
            var user = await _dbContext.Users
                .Include(u => u.Roles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName == model.UserName);
            if (user == null || user.Password != _passwordHelper.HashPassword(model.Password))
            {
                throw new ValidationException("Invalid username or password");
            }
            var token = _passwordHelper.GenerateJwtToken(user);
            return new LoginResponse { UserName = user.UserName, Token = token, UserId = user.Id };
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async Task<UserDto> GetUserById(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Roles).ThenInclude(r => r.Role)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new ValidationException(Constants.ServiceMessages.GET_USER_INVALID_USER_ID);
            }
            return user.ToDto(true);
        }
    }
}

