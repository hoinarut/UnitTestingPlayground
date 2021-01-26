using System;
using System.Threading.Tasks;
using Core.Exceptions;
using IamService;
using IamService.BusinessLogic.Services;
using IamService.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace UnitTests
{
    [Collection("Database collection")]
    public class UserActivityLogServiceTests : IClassFixture<DIFixture>
    {
        private readonly ServiceProvider _serviceProvider;

        public UserActivityLogServiceTests(DIFixture diFixture)
        {
            _serviceProvider = diFixture.ServiceProvider;
        }

        private UserActivityLogService GetUserActivityLogService()
        {
            return new UserActivityLogService(_serviceProvider.GetService<IamDbContext>());
        }

        [Theory]
        [InlineData(TestConstants.ADMIN_USER_ID)]
        public async Task GetUserActivity_ValidUserId_ShouldReturnUserActivityLogList(int userId)
        {
            // Arrange
            var userActivityLogService = GetUserActivityLogService();
            // Act
            var userActivityLogs = await userActivityLogService.GetUserActivity(userId);
            // Assert
            Assert.NotEmpty(userActivityLogs);
        }

        [Fact]
        public async Task GetUserActivity_InvalidUserId_ShouldThrowValidationException()
        {
            // Arrange
            var userActivityLogService = GetUserActivityLogService();
            // Act            
            // Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await userActivityLogService.GetUserActivity(TestConstants.INVALID_USER_ID));
        }

        [Fact]
        public async Task AddUserActivity_ValidInput_ShouldNotThrowAnyException()
        {
            // Arrange
            var userActivityLogService = GetUserActivityLogService();
            // Act
            var exception = await Record.ExceptionAsync(async () => await userActivityLogService.AddUserActivity(TestConstants.ADMIN_USER_ID, Enums.UserActionType.Login.ToString()));
            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task AddUserActivity_InvalidUserId_ShouldThrowValidationExceptionAndMessage()
        {
            // Arrange
            var userActivityLogService = GetUserActivityLogService();
            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await userActivityLogService.AddUserActivity(TestConstants.INVALID_USER_ID, Enums.UserActionType.Login.ToString()));
            // Assert
            Assert.Equal(Constants.ServiceMessages.ADD_USER_ACTIVITY_INVALID_USER, exception.Message);
        }

        [Fact]
        public async Task AddUserActivity_InvalidActionType_ShouldThrowValidationExceptionAndMessage()
        {
            // Arrange
            var userActivityLogService = GetUserActivityLogService();
            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await userActivityLogService.AddUserActivity(TestConstants.ADMIN_USER_ID, "bad"));
            // Assert
            Assert.Equal(Constants.ServiceMessages.ADD_USER_ACTIVITY_INVALID_ACTION_TYPE, exception.Message);
        }
    }
}
