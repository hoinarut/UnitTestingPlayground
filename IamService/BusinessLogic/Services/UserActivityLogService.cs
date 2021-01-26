using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Exceptions;
using IamService.DataAccess;
using IamService.DataAccess.DTOs;
using Microsoft.EntityFrameworkCore;
using static IamService.Enums;

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
            if (!Enum.TryParse(actionType, out UserActionType actionTypeParsed))
            {
                throw new ValidationException(Constants.ServiceMessages.ADD_USER_ACTIVITY_INVALID_ACTION_TYPE);
            }

            if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
            {
                throw new ValidationException(Constants.ServiceMessages.ADD_USER_ACTIVITY_INVALID_USER);
            }

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
            if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
            {
                throw new ValidationException(Constants.ServiceMessages.ADD_USER_ACTIVITY_INVALID_USER);
            }

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
