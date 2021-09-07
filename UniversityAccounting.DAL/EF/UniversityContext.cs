using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniversityAccounting.DAL.Entities;

namespace UniversityAccounting.DAL.EF
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            const string connectionString = @"Server=.;Database=University;Trusted_Connection=True;MultipleActiveResultSets=True";
            optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
                entity.HasIndex(e => new {e.Name})
                    .IsUnique());

            modelBuilder.Entity<Group>(entity =>
                entity.HasIndex(e => new {e.Name})
                    .IsUnique());

            modelBuilder.Entity<Student>(entity =>
                entity.HasIndex(e => new {e.LastName, e.FirstName, e.DateOfBirth})
                    .IsUnique());

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<Group>()
                .Property(g => g.StudentsQuantity)
                .HasComputedColumnSql("dbo.FindStudentsQuantity([Id])");
        }
    }
}
    