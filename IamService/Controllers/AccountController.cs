using System.Collections.Generic;
using System.Threading.Tasks;
using IamService.BusinessLogic.Services;
using IamService.DataAccess;
using IamService.DataAccess.DTOs;
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
        private readonly IUserActivityLogService _userActivityLogService;

        public AccountController(IAccountService accountService, IUserActivityLogService userActivityLogService)
        {
            _accountService = accountService;
            this._userActivityLogService = userActivityLogService;
        }

        [HttpPost("create")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<UserDto>> Create(UserCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.AsDictionary());
            }
            var newUser = await _accountService.CreateAsync(model);

            await _userActivityLogService.AddUserActivity(newUser.Id, Enums.UserActionType.CreateAccount.ToString());

            return CreatedAtRoute(nameof(GetById), new { userId = newUser.Id }, newUser);
        }

        [HttpGet("{userId}", Name = nameof(GetById))]
        public async Task<ActionResult<User>> GetById(int userId)
        {
            var user = await _accountService.GetUserById(userId);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginModel model)
        {
            var loginResult = await _accountService.LoginAsync(model);
            await _userActivityLogService.AddUserActivity(loginResult.UserId, Enums.UserActionType.Login.ToString());
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

        [HttpGet("activity/{userId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<List<UserActivityLogDto>>> GetUserActivity(int userId)
        {
            var userActivity = await _userActivityLogService.GetUserActivity(userId);
            return Ok(userActivity);
        }
    }
}
