using System;
using System.Collections.Generic;
using IamService;
using IamService.DataAccess.DTOs;
using IamService.Models;
using TestCore;

namespace IntegrationTests
{
    public static class TestHelper
    {
        public static UserCreateModel GetUserCreateModel()
        {
            return new UserCreateModel
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
        }
    }
}
