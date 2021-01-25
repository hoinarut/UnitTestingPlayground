using System;
using System.Threading.Tasks;
using Core.Exceptions;
using IamService.BusinessLogic.Helpers;
using IamService.BusinessLogic.Services;
using IamService.DataAccess;
using IamService.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace UnitTests
{
    public class AccountServiceTests : IClassFixture<DIFixture>
    {
        public const string ADMIN_USER_NAME = "hoinarut";
        public const string ADMIN_USER_PASSWORD = "tudor_test";
        public const string INVALID_USER_OR_PASSWORD = "Invalid username or password";
        private readonly ServiceProvider _serviceProvider;

        public AccountServiceTests(DIFixture diFixture)
        {
            _serviceProvider = diFixture.ServiceProvider;
        }

        private AccountService GetAccountService()
        {
            return new AccountService(_serviceProvider.GetService<IamDbContext>(), _serviceProvider.GetService<SecurityHelper>());
        }

        [Fact]
        public async Task Login_ValidUser_ShouldReturnLoginResponse()
        {
            // Arrange
            var accountService = GetAccountService();
            var loginModel = new LoginModel
            {
                UserName = ADMIN_USER_NAME,
                Password = ADMIN_USER_PASSWORD
            };
            // Act
            var loginResult = await accountService.LoginAsync(loginModel);
            // Assert
            Assert.IsType<LoginReponse>(loginResult);
            Assert.NotNull(loginResult.Token);
        }

        [Fact]
        public async Task Login_InvalidUser_ShouldThrowValidationException()
        {
            // Arrange
            var accountService = GetAccountService();
            var loginModel = new LoginModel
            {
                UserName = "abc",
                Password = ADMIN_USER_PASSWORD
            };
            // Act

            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await accountService.LoginAsync(loginModel));
            Assert.Equal(INVALID_USER_OR_PASSWORD, exception.Message);
        }

        [Fact]
        public async Task Login_InvalidPassword_ShouldThrowValidationException()
        {
            // Arrange
            var accountService = GetAccountService();
            var loginModel = new LoginModel
            {
                UserName = ADMIN_USER_NAME,
                Password = "abc"
            };
            // Act

            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await accountService.LoginAsync(loginModel));
            Assert.Equal(INVALID_USER_OR_PASSWORD, exception.Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetUserRoles_ValidUserId_ShouldReturnUserRoles(int userId)
        {
            // Arrange
            var accountService = GetAccountService();
            // Act
            var userRoles = await accountService.GetUserRolesAsync(userId);
            // Assert
            Assert.NotEmpty(userRoles);
        }

        [Theory]
        [InlineData(2)]
        public async Task GetUserRoles_InvalidUserId_ShouldReturnUserRoles(int userId)
        {
            // Arrange
            var accountService = GetAccountService();
            // Act
            var userRoles = await accountService.GetUserRolesAsync(userId);
            // Assert
            Assert.NotEmpty(userRoles);
        }
    }
}
