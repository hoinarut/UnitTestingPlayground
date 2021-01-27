using System;
using System.Collections.Generic;

namespace IamService.DataAccess.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; }
        public UserProfileDto UserProfile { get; set; }
    }
}
