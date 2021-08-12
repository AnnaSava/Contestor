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

        public DbSet<ContestLog> ContestLogs { get; set; }

        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Participant>()
                .HasOne(m => m.User)
                .WithMany(m => m.Participants)
                .HasForeignKey(m => m.UserId);

            builder.Entity<Participant>()
                .HasOne(m => m.Contest)
                .WithMany(m => m.Participants)
                .HasForeignKey(m => m.ContestId);

            builder.Entity<Participant>()
                .HasKey(m => new { m.UserId, m.ContestId });

            builder.Entity<Work>()
                .HasOne(m => m.Participant)
                .WithMany(m => m.Works)
                .HasForeignKey(m => new { m.ParticipantId, m.ContestId });

            builder.Entity<Work>()
                .HasOne(m => m.Contest)
                .WithMany(m => m.Works)
                .HasForeignKey(m => m.ContestId);

            builder.Entity<Vote>()
                .HasOne(m => m.Voter)
                .WithMany(m => m.Votes)
                .HasForeignKey(m => m.VoterId);

            builder.Entity<Vote>()
                .HasOne(m => m.Work)
                .WithMany(m => m.Votes)
                .HasForeignKey(m => m.WorkId);

            builder.Entity<Vote>()
                .HasKey(m => new { m.VoterId, m.WorkId });
        }
    }
}
