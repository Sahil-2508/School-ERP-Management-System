using Microsoft.EntityFrameworkCore;
using WebApplication_1.Models;

namespace WebApplication_1.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<ExamMaster> ExamMasters { get; set; }
        public DbSet<EvaluationMaster> EvaluationMasters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Students Table Configuration
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");
                entity.HasKey(e => e.StudentId);

                entity.Property(e => e.StudentId)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.ClassName)
                    .HasMaxLength(50);

                entity.Property(e => e.Division)
                    .HasMaxLength(10);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);
            });

            // ExamMaster Table Configuration
            modelBuilder.Entity<ExamMaster>(entity =>
            {
                entity.ToTable("ExamMaster");
                entity.HasKey(e => e.ExaminationID);

                entity.Property(e => e.ExaminationID)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ExamCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.ExamCode)
                    .IsUnique();

                entity.Property(e => e.ExamName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ExamType)
                    .HasMaxLength(50);

                entity.Property(e => e.Semester)
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Weightage)
                    .HasPrecision(5, 2);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.IsPublished)
                    .HasDefaultValue(false);

                entity.Property(e => e.IsResultPublished)
                    .HasDefaultValue(false);

                entity.Property(e => e.IsLocked)
                    .HasDefaultValue(false);
            });

            // EvaluationMaster Table Configuration
            modelBuilder.Entity<EvaluationMaster>(entity =>
            {
                entity.ToTable("EvaluationMaster");
                entity.HasKey(e => e.EvaluationID);

                entity.Property(e => e.EvaluationID)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.EvaluationCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.EvaluationCode)
                    .IsUnique();

                entity.Property(e => e.EvaluationName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.EvaluationType)
                    .HasMaxLength(50);

                entity.Property(e => e.MaximumMarks)
                    .HasPrecision(6, 2);

                entity.Property(e => e.PassMarks)
                    .HasPrecision(6, 2);

                entity.Property(e => e.Weightage)
                    .HasPrecision(5, 2);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
