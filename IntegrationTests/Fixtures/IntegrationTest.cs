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
        public async Task AuthenticateAsync()
        {
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await LoginAsync());
        }

        private async Task<string> LoginAsync()
        {
            var response = await ApiClient.PostAsJsonAsync(ApiRoutes.Login, new LoginModel
            {
                UserName = TestConstants.ADMIN_USER_NAME,
                Password = TestConstants.ADMIN_USER_PASSWORD
            });

            var loginResponse = await response.Content.ReadAsAsync<LoginResponse>();
            return loginResponse.Token;
        }
    }
}
