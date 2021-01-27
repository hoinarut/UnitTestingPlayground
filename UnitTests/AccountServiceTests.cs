using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Exceptions;
using IamService;
using IamService.BusinessLogic.Helpers;
using IamService.BusinessLogic.Services;
using IamService.DataAccess;
using IamService.DataAccess.DTOs;
using IamService.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace UnitTests
{
    [Collection("Database collection")]
    public class AccountServiceTests
    {        
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
                UserName = TestConstants.ADMIN_USER_NAME,
                Password = TestConstants.ADMIN_USER_PASSWORD
            };
            // Act
            var loginResult = await accountService.LoginAsync(loginModel);
            // Assert
            Assert.IsType<LoginResponse>(loginResult);
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
                Password = TestConstants.ADMIN_USER_PASSWORD
            };
            // Act

            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await accountService.LoginAsync(loginModel));
            Assert.Equal(Constants.ServiceMessages.CREATE_USER_INVALID_USER_OR_PASSWORD, exception.Message);
        }

        [Fact]
        public async Task Login_InvalidPassword_ShouldThrowValidationException()
        {
            // Arrange
            var accountService = GetAccountService();
            var loginModel = new LoginModel
            {
                UserName = TestConstants.ADMIN_USER_NAME,
                Password = "abc"
            };
            // Act

            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await accountService.LoginAsync(loginModel));
            Assert.Equal(Constants.ServiceMessages.CREATE_USER_INVALID_USER_OR_PASSWORD, exception.Message);
        }

        [Theory]
        [InlineData(TestConstants.ADMIN_USER_ID)]
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
        [InlineData(TestConstants.INVALID_USER_ID)]
        public async Task GetUserRoles_InvalidUserId_ShouldThrowValidationException(int userId)
        {
            // Arrange
            var accountService = GetAccountService();
            // Act            
            // Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await accountService.GetUserRolesAsync(userId));
        }

        [Fact]
        public async Task GetRoles_ShouldNotBeEmptyAndShouldHaveThreeItems()
        {
            // Arrange
            var accountService = GetAccountService();
            // Act
            var roles = await accountService.GetRolesAsync();
            // Assert
            Assert.NotEmpty(roles);
            Assert.Equal(TestConstants.SEEDED_ROLES, roles.Count);
        }

        [Fact]
        public async Task Create_ValidInput_ShouldReturnUserDtoObject()
        {
            // Arrange
            var accountService = GetAccountService();
            var userCreateModel = new UserCreateModel
            {
                UserName = "test_user",
                Password = "test_password",
                ConfirmPassword = "test_password",
                Profile = new UserProfileDto
                {
                    DateOfBirth = DateTime.Now,
                    EmailAddress = "test@test.com",
                    FirstName = "Test",
                    LastName = "User"
                },
                Roles = new List<int> { (int)Enums.Role.Employee }
            };
            // Act
            var newUser = await accountService.CreateAsync(userCreateModel);
            // Assert
            Assert.IsType<UserDto>(newUser);
            Assert.NotEqual(0, newUser.Id);
        }

        [Fact]
        public async Task Create_DuplicateEmail_ShouldThrowValidationExceptionAndMessage()
        {
            // Arrange
            var accountService = GetAccountService();
            var userCreateModel = new UserCreateModel
            {
                UserName = "test_user_duplicate_email",
                Password = "test_password",
                ConfirmPassword = "test_password",
                Profile = new UserProfileDto
                {
                    DateOfBirth = DateTime.Now,
                    EmailAddress = "tudor.hoinaru@gmail.com",
                    FirstName = "Test",
                    LastName = "User"
                },
                Roles = new List<int> { (int)Enums.Role.Employee }
            };
            // Act            
            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await accountService.CreateAsync(userCreateModel));
            Assert.Equal(Constants.ServiceMessages.CREATE_USER_EMAIL_IN_USE, exception.Message);
        }
        [Fact]
        public async Task Create_DuplicateUsername_ShouldThrowValidationExceptionAndMessage()
        {
            // Arrange
            var accountService = GetAccountService();
            var userCreateModel = new UserCreateModel
            {
                UserName = "hoinarut",
                Password = "test_password",
                ConfirmPassword = "test_password",
                Profile = new UserProfileDto
                {
                    DateOfBirth = DateTime.Now,
                    EmailAddress = "test@test.com",
                    FirstName = "Test",
                    LastName = "User"
                },
                Roles = new List<int> { (int)Enums.Role.Employee }
            };
            // Act            
            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await accountService.CreateAsync(userCreateModel));
            Assert.Equal(Constants.ServiceMessages.CREATE_USER_USERNAME_IN_USE, exception.Message);
        }

        [Fact]
        public async Task Create_InvalidRole_ShouldThrowValidationExceptionAndMessage()
        {
            // Arrange
            var accountService = GetAccountService();
            var userCreateModel = new UserCreateModel
            {
                UserName = "test_user_invalid_role",
                Password = "test_password",
                ConfirmPassword = "test_password",
                Profile = new UserProfileDto
                {
                    DateOfBirth = DateTime.Now,
                    EmailAddress = "test@test.com",
                    FirstName = "Test",
                    LastName = "User"
                },
                Roles = new List<int> { 9 }
            };
            // Act            
            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await accountService.CreateAsync(userCreateModel));
            Assert.Equal(Constants.ServiceMessages.CREATE_USER_INVALID_ROLE, exception.Message);
        }

        [Fact]
        public async Task Create_NoRole_ShouldThrowValidationExceptionAndMessage()
        {
            // Arrange
            var accountService = GetAccountService();
            var userCreateModel = new UserCreateModel
            {
                UserName = "test_user_no_role",
                Password = "test_password",
                ConfirmPassword = "test_password",
                Profile = new UserProfileDto
                {
                    DateOfBirth = DateTime.Now,
                    EmailAddress = "test@test.com",
                    FirstName = "Test",
                    LastName = "User"
                }
            };
            // Act            
            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await accountService.CreateAsync(userCreateModel));
            Assert.Equal(Constants.ServiceMessages.CREATE_USER_NO_ROLE, exception.Message);
        }
    }
}
