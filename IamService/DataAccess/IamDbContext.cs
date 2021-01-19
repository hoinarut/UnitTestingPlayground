using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IamService.DataAccess
{
    public class IamDbContext : IdentityDbContext<IdentityUser>
    {
        public IamDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
