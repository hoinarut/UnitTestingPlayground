using System;
using Microsoft.AspNetCore.Authorization;

namespace Core
{
    public class RoleBasedPolicyRequirement : IAuthorizationRequirement
    {
        public string RoleName { get; }

        public RoleBasedPolicyRequirement(string roleName)
        {
            RoleName = roleName;
        }
    }
}
