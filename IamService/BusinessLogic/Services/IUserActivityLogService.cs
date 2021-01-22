using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IamService.DataAccess.DTOs;

namespace IamService.BusinessLogic.Services
{
    public interface IUserActivityLogService
    {
        public Task<List<UserActivityLogDto>> GetUserActivity(int userId);
        public Task AddUserActivity(int userId, string actionType);
    }
}
