using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IamService.DataAccess.DTOs
{
    public class UserProfileDto
    {
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string EmailAddress { get; set; }
    }
}
