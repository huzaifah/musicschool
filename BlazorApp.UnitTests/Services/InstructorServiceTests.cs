using BlazorApp.Data;
using BlazorApp.Data.Entities;
using BlazorApp.Services.Implementation;
using BlazorApp.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace BlazorApp.UnitTests.Services;

public class InstructorServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly InstructorService _service;

    public InstructorServiceTests()
    {
        _context = MockDbContextFactory.CreateMockContext();
        _service = new InstructorService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetAllInstructorsAsync_ShouldReturnAllInstructors()
    {
        // Arrange
        var instructor1 = TestDataBuilder.CreateTestInstructor(1, "Alice");
        var instructor2 = TestDataBuilder.CreateTestInstructor(2, "Bob");
        var instructor3 = TestDataBuilder.CreateTestInstructor(3, "Charlie");
        _context.Instructors.AddRange(instructor1, instructor2, instructor3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllInstructorsAsync();

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(i => i.Name == "Alice");
        result.Should().Contain(i => i.Name == "Bob");
        result.Should().Contain(i => i.Name == "Charlie");
    }

    [Fact]
    public async Task GetAllInstructorsAsync_ShouldReturnOrderedByName()
    {
        // Arrange
        var instructor1 = TestDataBuilder.CreateTestInstructor(1, "Zebra");
        var instructor2 = TestDataBuilder.CreateTestInstructor(2, "Apple");
        var instructor3 = TestDataBuilder.CreateTestInstructor(3, "Mango");
        _context.Instructors.AddRange(instructor1, instructor2, instructor3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllInstructorsAsync();

        // Assert
        result.Should().BeInAscendingOrder(i => i.Name);
        result[0].Name.Should().Be("Apple");
        result[1].Name.Should().Be("Mango");
        result[2].Name.Should().Be("Zebra");
    }

    [Fact]
    public async Task GetActiveInstructorsAsync_ShouldReturnOnlyActiveInstructors()
    {
        // Arrange
        var activeInstructor1 = TestDataBuilder.CreateTestInstructor(1, "Active One", isActive: true);
        var activeInstructor2 = TestDataBuilder.CreateTestInstructor(2, "Active Two", isActive: true);
        var inactiveInstructor = TestDataBuilder.CreateTestInstructor(3, "Inactive", isActive: false);
        _context.Instructors.AddRange(activeInstructor1, activeInstructor2, inactiveInstructor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetActiveInstructorsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(i => i.Name == "Active One");
        result.Should().Contain(i => i.Name == "Active Two");
        result.Should().NotContain(i => i.Name == "Inactive");
    }

    [Fact]
    public async Task GetActiveInstructorsAsync_ShouldNotReturnInactiveInstructors()
    {
        // Arrange
        var activeInstructor = TestDataBuilder.CreateTestInstructor(1, "Active", isActive: true);
        var inactiveInstructor1 = TestDataBuilder.CreateTestInstructor(2, "Inactive 1", isActive: false);
        var inactiveInstructor2 = TestDataBuilder.CreateTestInstructor(3, "Inactive 2", isActive: false);
        _context.Instructors.AddRange(activeInstructor, inactiveInstructor1, inactiveInstructor2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetActiveInstructorsAsync();

        // Assert
        result.Should().HaveCount(1);
        result.Should().AllSatisfy(i => i.IsActive.Should().BeTrue());
    }

    [Fact]
    public async Task GetInstructorByIdAsync_ShouldReturnInstructor_WhenExists()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(5, "Test Instructor");
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetInstructorByIdAsync(5);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(5);
        result.Name.Should().Be("Test Instructor");
    }

    [Fact]
    public async Task GetInstructorByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var result = await _service.GetInstructorByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetInstructorByIdAsync_ShouldIncludeClasses()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1, "Instructor With Classes");
        var class1 = TestDataBuilder.CreateTestMusicClass(1, instructorId: 1);
        var class2 = TestDataBuilder.CreateTestMusicClass(2, instructorId: 1);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(class1, class2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetInstructorByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Classes.Should().NotBeNull();
        result.Classes.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateInstructorAsync_ShouldAddInstructorToDatabase()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1, "New Instructor");

        // Act
        var result = await _service.CreateInstructorAsync(instructor);

        // Assert
        var savedInstructor = await _context.Instructors.FindAsync(1);
        savedInstructor.Should().NotBeNull();
        savedInstructor!.Name.Should().Be("New Instructor");
    }

    [Fact]
    public async Task CreateInstructorAsync_ShouldReturnCreatedInstructor()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1, "Created Instructor", hourlyRate: 75.00m);

        // Act
        var result = await _service.CreateInstructorAsync(instructor);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Created Instructor");
        result.HourlyRate.Should().Be(75.00m);
    }

    [Fact]
    public async Task UpdateInstructorAsync_ShouldUpdateInstructor()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1, "Original Name");
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        // Act
        instructor.Name = "Updated Name";
        instructor.HourlyRate = 100.00m;
        var result = await _service.UpdateInstructorAsync(instructor);

        // Assert
        var updatedInstructor = await _context.Instructors.FindAsync(1);
        updatedInstructor.Should().NotBeNull();
        updatedInstructor!.Name.Should().Be("Updated Name");
        updatedInstructor.HourlyRate.Should().Be(100.00m);
    }

    [Fact]
    public async Task UpdateInstructorAsync_ShouldReturnUpdatedInstructor()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1, "Test");
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        // Act
        instructor.Bio = "Updated bio";
        var result = await _service.UpdateInstructorAsync(instructor);

        // Assert
        result.Should().NotBeNull();
        result.Bio.Should().Be("Updated bio");
    }

    [Fact]
    public async Task DeleteInstructorAsync_ShouldRemoveInstructor_WhenExists()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1, "To Be Deleted");
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        // Act
        await _service.DeleteInstructorAsync(1);

        // Assert
        var deletedInstructor = await _context.Instructors.FindAsync(1);
        deletedInstructor.Should().BeNull();
    }

    [Fact]
    public async Task DeleteInstructorAsync_ShouldNotThrow_WhenNotExists()
    {
        // Act
        var act = async () => await _service.DeleteInstructorAsync(999);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GetAllInstructorsAsync_ShouldReturnEmptyList_WhenNoInstructors()
    {
        // Act
        var result = await _service.GetAllInstructorsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetActiveInstructorsAsync_ShouldReturnOrderedByName()
    {
        // Arrange
        var instructor1 = TestDataBuilder.CreateTestInstructor(1, "Zara", isActive: true);
        var instructor2 = TestDataBuilder.CreateTestInstructor(2, "Anna", isActive: true);
        var instructor3 = TestDataBuilder.CreateTestInstructor(3, "Mike", isActive: true);
        _context.Instructors.AddRange(instructor1, instructor2, instructor3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetActiveInstructorsAsync();

        // Assert
        result.Should().BeInAscendingOrder(i => i.Name);
        result[0].Name.Should().Be("Anna");
        result[1].Name.Should().Be("Mike");
        result[2].Name.Should().Be("Zara");
    }
}
