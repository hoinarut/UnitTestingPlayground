using System;
using System.Collections.Generic;
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
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserActivityLog> UserActivityLogs { get; set; }

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
                    new User
                    {
                        Id = 1,
                        CreatedOn = DateTime.Now,
                        IsActive = true,
                        Password = pwd,
                        UserName = "hoinarut"
                    }
                );

            modelBuilder.Entity<UserRole>()
                .ToTable("UserRoles")
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.Roles);
            modelBuilder.Entity<UserRole>()
                .HasData(new UserRole
                {
                    RoleId = 1,
                    UserId = 1
                });

            modelBuilder.Entity<UserProfile>().ToTable("UserProfiles")
                .HasData(
                new UserProfile
                {
                    Id = 1,
                    FirstName = "Tudor",
                    LastName = "Hoinaru",
                    DateOfBirth = DateTime.Parse("10-04-1985"),
                    UserId = 1,
                    EmailAddress = "tudor.hoinaru@gmail.com"
                });
            modelBuilder.Entity<UserActivityLog>().ToTable("UserActivityLogs")
                .HasOne(ual => ual.User)
                .WithMany();
            modelBuilder.Entity<UserActivityLog>()
                .HasData(
                new UserActivityLog
                {
                    ActionType = Enums.UserActionType.CreateAccount.ToString(),
                    EntryDate = DateTime.Now,
                    Id = 1,
                    UserId = 1
                });

        }
    }
}
