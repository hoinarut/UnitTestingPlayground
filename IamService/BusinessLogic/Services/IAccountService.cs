﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IamService.DataAccess;
using IamService.Models;

namespace IamService.BusinessLogic.Services
{
    public interface IAccountService
    {
        public Task<LoginReponse> LoginAsync(LoginModel model);
        public Task<List<Role>> GetRolesAsync();
        public Task<List<string>> GetUserRolesAsync(int userId);
        public Task<User> CreateAsync(UserCreateModel model);
    }
}