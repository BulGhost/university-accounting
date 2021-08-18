using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentAccounting.Models;

namespace StudentAccounting.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Course> Courses { get; set; }

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
        }
    }
}
