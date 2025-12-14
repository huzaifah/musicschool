using BlazorApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<MusicClass> MusicClasses { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Instrument> Instruments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MusicClass>()
            .HasOne(c => c.Instructor)
            .WithMany(i => i.Classes)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.MusicClass)
            .WithOne(c => c.Booking)
            .HasForeignKey<Booking>(b => b.MusicClassId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MusicClass>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Instructor>()
            .Property(i => i.HourlyRate)
            .HasPrecision(18, 2);
    }
}
