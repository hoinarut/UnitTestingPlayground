using IamService;
using IamService.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TestCore;

namespace IntegrationTests.Fixtures
{
    public class IntegrationTest
    {
        public HttpClient ApiClient { get; protected set; }

        public IntegrationTest(WebApplicationFactory<Startup> factory)
        {
            ApiClient = factory.CreateClient();
        }
        public async Task AuthenticateAsAdminAsync()
        {
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await LoginAsync(TestConstants.ADMIN_USER_NAME, TestConstants.ADMIN_USER_PASSWORD));
        }
        public async Task AuthenticateAsEmployeeAsync()
        {
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await LoginAsync(TestConstants.EMPLOYEE_USER_NAME, TestConstants.EMPLOYEE_USER_PASSWORD));
        }

        private async Task<string> LoginAsync(string userName, string password)
        {
            var response = await ApiClient.PostAsJsonAsync(ApiRoutes.Login, new LoginModel
            {
                UserName = userName,
                Password = password
            });

            var loginResponse = await response.Content.ReadAsAsync<LoginResponse>();
            return loginResponse.Token;
        }
    }
}
