using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
