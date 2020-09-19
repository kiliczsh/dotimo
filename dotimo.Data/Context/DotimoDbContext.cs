using dotimo.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace dotimo.Data.Context
{
    public class DotimoDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DotimoDbContext(DbContextOptions<DotimoDbContext> options) : base(options) { }

        public DbSet<Watch> Watches { get; set; }
        public DbSet<CheckUp> CheckUps { get; set; }
        public DbSet<MonitoringRequest> MonitoringRequests { get; set; }
    }
}