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
            await AuthenticateAsAdminAsync();
            var userCreateModel = TestHelper.GetUserCreateModel();
            // Act
            var response = await ApiClient.PostAsJsonAsync(ApiRoutes.CreateAccount, userCreateModel);
            // Assert 
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var newUser = await response.Content.ReadAsAsync<User>();
            newUser.Should().NotBeNull();
            newUser.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task Create_NotAdmin_ShouldReturnForbidden()
        {
            // Arrange
            await AuthenticateAsEmployeeAsync();
            var userCreateModel = TestHelper.GetUserCreateModel();
            // Act
            var response = await ApiClient.PostAsJsonAsync(ApiRoutes.CreateAccount, userCreateModel);
            // Assert 
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_NoToken_ShouldReturnUnauthorized()
        {
            // Arrange
            var userCreateModel = TestHelper.GetUserCreateModel();
            // Act
            var response = await ApiClient.PostAsJsonAsync(ApiRoutes.CreateAccount, userCreateModel);
            // Assert 
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_InvalidInput_ShouldReturnBadRequest()
        {
            // Arrange
            await AuthenticateAsAdminAsync();
            var userCreateModel = new UserCreateModel
            {
                UserName = "bad"
            };
            // Act
            var response = await ApiClient.PostAsJsonAsync(ApiRoutes.CreateAccount, userCreateModel);
            // Assert 
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region USER ROLES

        [Fact]
        public async Task GetUserRoles_ValidUserIdAndToken_ShouldReturnUserRolesList()
        {
            // Arrange
            await AuthenticateAsAdminAsync();
            // Act
            var response = await ApiClient.GetAsync($"{ ApiRoutes.UserRoles}/{TestConstants.ADMIN_USER_ID}");
            // Assert             
            var roles = await response.Content.ReadAsAsync<List<string>>();
            roles.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetUserRoles_NotAdmin_ShouldReturnForbidden()
        {
            // Arrange
            await AuthenticateAsEmployeeAsync();
            // Act
            var response = await ApiClient.GetAsync($"{ ApiRoutes.UserRoles}/{TestConstants.ADMIN_USER_ID}");
            // Assert             
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetUserRoles_NoToken_ShouldReturnUnauthorized()
        {
            // Arrange
            // Act
            var response = await ApiClient.GetAsync($"{ ApiRoutes.UserRoles}/{TestConstants.ADMIN_USER_ID}");
            // Assert             
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetUserRoles_InvalidUserId_ShouldReturnBadRequest()
        {
            // Arrange
            await AuthenticateAsAdminAsync();
            // Act
            var response = await ApiClient.GetAsync($"{ ApiRoutes.UserRoles}/{TestConstants.INVALID_USER_ID}");
            // Assert             
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region ROLES
        [Fact]
        public async Task GetRoles_ShouldReturnRolesList()
        {
            // Arrange
            // Act
            var response = await ApiClient.GetAsync(ApiRoutes.Roles);
            // Assert             
            var roles = await response.Content.ReadAsAsync<List<Role>>();
            roles.Should().NotBeEmpty();
        }
        #endregion

        #region USER ACTIVITY
        [Fact]
        public async Task GetUserActivity_ValidUserIdAndToken_ShouldReturnUserActivityLogDtoList()
        {
            // Arrange
            await AuthenticateAsAdminAsync();
            // Act
            var response = await ApiClient.GetAsync($"{ ApiRoutes.UserActivity}/{TestConstants.ADMIN_USER_ID}");
            // Assert             
            var roles = await response.Content.ReadAsAsync<List<UserActivityLogDto>>();
            roles.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetUserActivity_NotAdmin_ShouldReturnForbidden()
        {
            // Arrange
            await AuthenticateAsEmployeeAsync();
            // Act
            var response = await ApiClient.GetAsync($"{ ApiRoutes.UserActivity}/{TestConstants.ADMIN_USER_ID}");
            // Assert             
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetUserActivity_NoToken_ShouldReturnUnauthorized()
        {
            // Arrange
            // Act
            var response = await ApiClient.GetAsync($"{ ApiRoutes.UserActivity}/{TestConstants.ADMIN_USER_ID}");
            // Assert             
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetUserActivity_InvalidUserId_ShouldReturnBadRequest()
        {
            // Arrange
            await AuthenticateAsAdminAsync();
            // Act
            var response = await ApiClient.GetAsync($"{ ApiRoutes.UserActivity}/{TestConstants.INVALID_USER_ID}");
            // Assert             
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
    }
}
