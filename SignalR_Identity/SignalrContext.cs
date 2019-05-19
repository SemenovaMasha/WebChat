using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalR_Identity.Models;

namespace SignalR_Identity
{
    public class SignalrContext : IdentityDbContext<SignalrUser>
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        public SignalrContext(DbContextOptions<SignalrContext> options)
            : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    public static class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<SignalrUser> userManager)
        {
            Random r = new Random();

            for (int i = 1; i <= 200; i++)
            {
                if (userManager.FindByNameAsync("TestUser" + i).Result == null)
                {
                    var user = new SignalrUser()
                    { UserName = "TestUser" + i, BirthDate = new DateTime(r.Next(1970, 2007), r.Next(1, 12), r.Next(1, 28)), CreatingDate = DateTime.Now ,EmailConfirmed = true};

                    IdentityResult result = userManager.CreateAsync(user, "testuser" + i).Result;

                }
            }

        }
    }

}
