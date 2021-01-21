using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Core
{
    public class RoleBasedPolicyHandler : AuthorizationHandler<RoleBasedPolicyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RoleBasedPolicyRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var groups = context.User.FindAll(c => c.Type == ClaimTypes.Role).ToList();

            if (groups.Any(c => c.Value == requirement.RoleName))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
