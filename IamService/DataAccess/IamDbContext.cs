using System;
using IamService.BusinessLogic.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace IamService.DataAccess
{
    public class IamDbContext : DbContext
    {
        public IamDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("Roles")
                .HasData(
                new Role { Id = 1, Name = "Admin", Description = "Administrative Role" },
                new Role { Id = 2, Name = "Employee", Description = "Employee Role" },
                new Role { Id = 3, Name = "Manager", Description = "Manager Role" }
                );

            var secHlp = this.Database.GetService<SecurityHelper>();
            var pwd = secHlp.HashPassword("tudor_test");

            modelBuilder.Entity<User>().ToTable("Users")
                .HasData(
                    new User { Id = 1, CreatedOn = DateTime.Now, IsActive = true, Password = pwd, UserName = "hoinarut" }
                );

            modelBuilder.Entity<UserRole>()
                .ToTable("UserRoles")
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles);
            modelBuilder.Entity<UserRole>()
                .HasData(
                 new UserRole { UserId = 1, RoleId = 1 }
                );
        }
    }
}
