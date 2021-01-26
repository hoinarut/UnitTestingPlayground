using System;
namespace IamService.Models
{
    public class LoginResponse
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}
