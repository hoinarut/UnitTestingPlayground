using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core;
using IamService.DataAccess.DTOs;

namespace IamService.Models
{
    public class UserCreateModel
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(15)]
        [MinLength(2)]
        public string UserName { get; set; }
        [MaxLength(15)]
        [MinLength(2)]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public List<int> Roles { get; set; }
        [Required]
        public UserProfileDto Profile { get; set; }
    }
}

