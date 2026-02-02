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
        List<Registration>? registrations = null)
    {
        var context = CreateMockContext();

        if (registrations != null)
        {
            context.Registrations.AddRange(registrations);
        }

        context.SaveChanges();
        return context;
    }
}
