using System;
using System.Collections.Generic;
using System.Linq;
using IamService.DataAccess.DTOs;

namespace IamService.DataAccess
{
    public static class Extensions
    {
        public static UserDto ToDto(this User user, bool includeDetails)
        {
            var dto = new UserDto
            {
                Id = user.Id,
                CreatedOn = user.CreatedOn,
                IsActive = user.IsActive,
                Password = user.Password,
                UserName = user.UserName
            };
            if (includeDetails)
            {
                if (user.Roles != null)
                {
                    dto.Roles = ((List<UserRole>)user.Roles).Select(r => r.Role.Name).ToList();
                }
                dto.UserProfile = user.Profile.ToDto();
            }
            return dto;
        }

        public static UserProfileDto ToDto(this UserProfile profile)
        {
            return new UserProfileDto
            {
                DateOfBirth = profile.DateOfBirth,
                EmailAddress = profile.EmailAddress,
                FirstName = profile.FirstName,
                LastName = profile.LastName
            };
        }
    }
}
