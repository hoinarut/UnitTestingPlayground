using FluentAssertions;
using IamService;
using IamService.DataAccess;
using IamService.DataAccess.DTOs;
using IamService.Models;
using IntegrationTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TestCore;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class AccountControllerTests : IntegrationTest, IClassFixture<ApiWebApplicationFactory<Startup>>
    {
        private readonly ITestOutputHelper _output;

        public AccountControllerTests(ApiWebApplicationFactory<Startup> factory, ITestOutputHelper output) : base(factory)
        {
            _output = output;
        }

        #region LOGIN
        [Fact]
        public async Task Login_ValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var loginRequest = new LoginModel
            {
                UserName = TestConstants.ADMIN_USER_NAME,
                Password = TestConstants.ADMIN_USER_PASSWORD
            };
            // Act
            var response = await ApiClient.PostAsJsonAsync(ApiRoutes.Login, loginRequest);
            // Assert 
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var loginResponse = await response.Content.ReadAsAsync<LoginResponse>();
            loginResponse.Token.Should().NotBeEmpty();
            _output.WriteLine($"Received token: {loginResponse.Token}");
        }
        [Fact]
        public async Task Login_InvalidCredentials_ShouldReturnBadRequest()
        {
            // Arrange            
            var loginRequest = new LoginModel
            {
                UserName = "abcd",
                Password = "efgh"
            };
            // Act
            var response = await ApiClient.PostAsJsonAsync(ApiRoutes.Login, loginRequest);
            // Assert 
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region CREATE

        [Fact]
        public async Task Create_ValidInput_ShouldReturnUserAndCreatedStatus()
        {
            // Arrange
            await AuthenticateAsync();
            var userCreateModel = new UserCreateModel
            {
                UserName = TestConstants.TEST_USER_1,
                Password = TestConstants.TEST_PASSWORD,
                ConfirmPassword = TestConstants.TEST_PASSWORD,
                Profile = new UserProfileDto
                {
                    DateOfBirth = DateTime.Now,
                    EmailAddress = TestConstants.TEST_EMAIL,
                    FirstName = TestConstants.TEST_USER_FIRST_NAME,
                    LastName = TestConstants.TEST_USER_LAST_NAME
                },
                Roles = new List<int>
                {
                    (int)Enums.Role.Employee
                }
            };
            // Act
            var response = await ApiClient.PostAsJsonAsync(ApiRoutes.CreateAccount, userCreateModel);
            // Assert 
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var newUser = await response.Content.ReadAsAsync<User>();
            newUser.Should().NotBeNull();
            newUser.Id.Should().NotBe(0);
        }

        #endregion

    }
}
