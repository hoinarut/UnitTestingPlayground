using System.Collections.Generic;
using System.Threading.Tasks;
using IamService.BusinessLogic.Services;
using IamService.DataAccess;
using IamService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IamService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("create")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<User>> Create(UserCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.AsDictionary());
            }
            var newUser = await _accountService.CreateAsync(model);
            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginReponse>> Login(LoginModel model)
        {
            var loginResult = await _accountService.LoginAsync(model);
            return Ok(loginResult);
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<Role>>> GetRolesAsync()
        {
            var roles = await _accountService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpGet("userroles/{userId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<List<string>>> GetUserRolesAsync(int userId)
        {
            var userRoles = await _accountService.GetUserRolesAsync(userId);
            return Ok(userRoles);
        }

    }
}
