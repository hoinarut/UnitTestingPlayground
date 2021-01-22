using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IamService.DataAccess;
using IamService.DataAccess.DTOs;
using Microsoft.EntityFrameworkCore;

namespace IamService.BusinessLogic.Services
{
    public class UserActivityLogService : IUserActivityLogService
    {
        private readonly IamDbContext _dbContext;

        public UserActivityLogService(IamDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddUserActivity(int userId, string actionType)
        {
            _dbContext.UserActivityLogs.Add(new UserActivityLog
            {
                UserId = userId,
                ActionType = actionType,
                EntryDate = DateTime.Now
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserActivityLogDto>> GetUserActivity(int userId)
        {
            var userActivityLogs = await _dbContext.UserActivityLogs
                .Where(ual => ual.UserId == userId)
                .Select(ual => new UserActivityLogDto
                {
                    Id = ual.Id,
                    ActionType = ual.ActionType,
                    EntryDate = ual.EntryDate
                })
                .ToListAsync();
            return userActivityLogs;
        }
    }
}
