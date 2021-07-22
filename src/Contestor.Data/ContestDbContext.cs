using Contestor.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Contestor.Data
{
    public class ContestDbContext : IdentityDbContext<User, Role, long>
    {
        public ContestDbContext(DbContextOptions<ContestDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<Contest> Contests { get; set; }

        public DbSet<Work> Works { get; set; }

        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Participant>()
                .HasKey(m => new { m.UserId, m.ContestId });
        }
    }
}
