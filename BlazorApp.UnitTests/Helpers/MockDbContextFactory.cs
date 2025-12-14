using BlazorApp.Data;
using BlazorApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.UnitTests.Helpers;

public static class MockDbContextFactory
{
    public static ApplicationDbContext CreateMockContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    public static ApplicationDbContext CreateMockContextWithData(
        List<Instructor>? instructors = null,
        List<MusicClass>? classes = null,
        List<Booking>? bookings = null)
    {
        var context = CreateMockContext();

        if (instructors != null)
        {
            context.Instructors.AddRange(instructors);
        }

        if (classes != null)
        {
            context.MusicClasses.AddRange(classes);
        }

        if (bookings != null)
        {
            context.Bookings.AddRange(bookings);
        }

        context.SaveChanges();
        return context;
    }
}
