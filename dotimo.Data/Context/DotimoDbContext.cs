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

        public DbSet<BaseEntity> BaseEntities { get; set; }
    }
}