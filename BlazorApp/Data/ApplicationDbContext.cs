using BlazorApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Registration> Registrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasIndex(r => r.ReferenceNumber)
                .IsUnique();

            entity.Property(r => r.ReferenceNumber)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(r => r.StudentName)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(r => r.Gender)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(r => r.SchoolYear)
                .HasMaxLength(10)
                .IsRequired();

            entity.Property(r => r.MusicExperience)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(r => r.GuardianName)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(r => r.GuardianPhone)
                .HasMaxLength(15)
                .IsRequired();

            entity.Property(r => r.GuardianEmail)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(r => r.Postcode)
                .HasMaxLength(5)
                .IsRequired();

            entity.Property(r => r.City)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(r => r.State)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(r => r.EmergencyName)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(r => r.EmergencyPhone)
                .HasMaxLength(15)
                .IsRequired();

            entity.Property(r => r.EmergencyRelationship)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(r => r.Venue)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(r => r.ClassSlot)
                .HasMaxLength(100)
                .IsRequired(false);

            entity.Property(r => r.SignatoryName)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(r => r.Status)
                .HasConversion<string>();
        });
    }
}
