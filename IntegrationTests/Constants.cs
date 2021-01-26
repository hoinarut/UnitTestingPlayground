using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public static class ApiRoutes
    {
        private const string ACCOUNT_API_ROOT = "/Account";
        public static string Login = $"{ACCOUNT_API_ROOT}/login";
        public static string CreateAccount = $"{ACCOUNT_API_ROOT}/create";
    }
}
