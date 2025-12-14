using BlazorApp.Data;
using BlazorApp.Data.Entities;
using BlazorApp.Data.Enums;
using BlazorApp.Services.Implementation;
using BlazorApp.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace BlazorApp.UnitTests.Services;

public class ClassServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly ClassService _service;

    public ClassServiceTests()
    {
        _context = MockDbContextFactory.CreateMockContext();
        _service = new ClassService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    #region GetAvailableClassesAsync Tests

    [Fact]
    public async Task GetAvailableClassesAsync_ShouldReturnOnlyAvailableClasses()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var availableClass = TestDataBuilder.CreateTestMusicClass(1, status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));
        var bookedClass = TestDataBuilder.CreateTestMusicClass(2, status: ClassStatus.Booked, scheduledDateTime: DateTime.Now.AddDays(2));
        var cancelledClass = TestDataBuilder.CreateTestMusicClass(3, status: ClassStatus.Cancelled, scheduledDateTime: DateTime.Now.AddDays(3));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(availableClass, bookedClass, cancelledClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAvailableClassesAsync();

        // Assert
        result.Should().HaveCount(1);
        result[0].Status.Should().Be(ClassStatus.Available);
    }

    [Fact]
    public async Task GetAvailableClassesAsync_ShouldReturnOnlyFutureClasses()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var futureClass = TestDataBuilder.CreateTestMusicClass(1, status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));
        var pastClass = TestDataBuilder.CreateTestMusicClass(2, status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(-1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(futureClass, pastClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAvailableClassesAsync();

        // Assert
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(1);
        result[0].ScheduledDateTime.Should().BeAfter(DateTime.Now);
    }

    [Fact]
    public async Task GetAvailableClassesAsync_ShouldExcludePastClasses()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var pastClass = TestDataBuilder.CreateTestMusicClass(1, status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddHours(-1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(pastClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAvailableClassesAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAvailableClassesAsync_ShouldExcludeBookedClasses()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var bookedClass = TestDataBuilder.CreateTestMusicClass(1, status: ClassStatus.Booked, scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(bookedClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAvailableClassesAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAvailableClassesAsync_ShouldOrderByScheduledDateTime()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var class1 = TestDataBuilder.CreateTestMusicClass(1, status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(3));
        var class2 = TestDataBuilder.CreateTestMusicClass(2, status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));
        var class3 = TestDataBuilder.CreateTestMusicClass(3, status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(2));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(class1, class2, class3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAvailableClassesAsync();

        // Assert
        result.Should().HaveCount(3);
        result.Should().BeInAscendingOrder(c => c.ScheduledDateTime);
        result[0].Id.Should().Be(2); // AddDays(1)
        result[1].Id.Should().Be(3); // AddDays(2)
        result[2].Id.Should().Be(1); // AddDays(3)
    }

    [Fact]
    public async Task GetAvailableClassesAsync_ShouldIncludeInstructor()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1, "Test Instructor");
        var musicClass = TestDataBuilder.CreateTestMusicClass(1, instructorId: 1, status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAvailableClassesAsync();

        // Assert
        result.Should().HaveCount(1);
        result[0].Instructor.Should().NotBeNull();
        result[0].Instructor.Name.Should().Be("Test Instructor");
    }

    #endregion

    #region GetClassesByInstructorAsync Tests

    [Fact]
    public async Task GetClassesByInstructorAsync_ShouldReturnClassesForInstructor()
    {
        // Arrange
        var instructor1 = TestDataBuilder.CreateTestInstructor(1, "Instructor 1");
        var instructor2 = TestDataBuilder.CreateTestInstructor(2, "Instructor 2");
        var class1 = TestDataBuilder.CreateTestMusicClass(1, instructorId: 1, scheduledDateTime: DateTime.Now.AddDays(1));
        var class2 = TestDataBuilder.CreateTestMusicClass(2, instructorId: 1, scheduledDateTime: DateTime.Now.AddDays(2));
        var class3 = TestDataBuilder.CreateTestMusicClass(3, instructorId: 2, scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.AddRange(instructor1, instructor2);
        _context.MusicClasses.AddRange(class1, class2, class3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetClassesByInstructorAsync(1);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(c => c.InstructorId.Should().Be(1));
    }

    [Fact]
    public async Task GetClassesByInstructorAsync_ShouldReturnOnlyFutureClasses()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1);
        var futureClass = TestDataBuilder.CreateTestMusicClass(1, instructorId: 1, scheduledDateTime: DateTime.Now.AddDays(1));
        var pastClass = TestDataBuilder.CreateTestMusicClass(2, instructorId: 1, scheduledDateTime: DateTime.Now.AddDays(-1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(futureClass, pastClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetClassesByInstructorAsync(1);

        // Assert
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(1);
    }

    [Fact]
    public async Task GetClassesByInstructorAsync_ShouldIncludeBooking()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1);
        var musicClass = TestDataBuilder.CreateTestMusicClass(1, instructorId: 1, status: ClassStatus.Booked, scheduledDateTime: DateTime.Now.AddDays(1));
        var booking = TestDataBuilder.CreateTestBooking(1, musicClassId: 1);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetClassesByInstructorAsync(1);

        // Assert
        result.Should().HaveCount(1);
        result[0].Booking.Should().NotBeNull();
    }

    [Fact]
    public async Task GetClassesByInstructorAsync_ShouldReturnEmpty_WhenNoClassesExist()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1);
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetClassesByInstructorAsync(1);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region GetClassesByInstrumentAsync Tests

    [Fact]
    public async Task GetClassesByInstrumentAsync_ShouldFilterByInstrument()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var pianoClass = TestDataBuilder.CreateTestMusicClass(1, instrument: "Piano", status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));
        var guitarClass = TestDataBuilder.CreateTestMusicClass(2, instrument: "Guitar", status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(pianoClass, guitarClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetClassesByInstrumentAsync("Piano");

        // Assert
        result.Should().HaveCount(1);
        result[0].Instrument.Should().Be("Piano");
    }

    [Fact]
    public async Task GetClassesByInstrumentAsync_ShouldReturnOnlyAvailableClasses()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var availablePiano = TestDataBuilder.CreateTestMusicClass(1, instrument: "Piano", status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));
        var bookedPiano = TestDataBuilder.CreateTestMusicClass(2, instrument: "Piano", status: ClassStatus.Booked, scheduledDateTime: DateTime.Now.AddDays(2));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(availablePiano, bookedPiano);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetClassesByInstrumentAsync("Piano");

        // Assert
        result.Should().HaveCount(1);
        result[0].Status.Should().Be(ClassStatus.Available);
    }

    [Theory]
    [InlineData("Piano")]
    [InlineData("Guitar")]
    [InlineData("Violin")]
    public async Task GetClassesByInstrumentAsync_ShouldWorkForDifferentInstruments(string instrument)
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(1, instrument: instrument, status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetClassesByInstrumentAsync(instrument);

        // Assert
        result.Should().HaveCount(1);
        result[0].Instrument.Should().Be(instrument);
    }

    #endregion

    #region GetClassesBySkillLevelAsync Tests

    [Theory]
    [InlineData(SkillLevel.Beginner)]
    [InlineData(SkillLevel.Intermediate)]
    [InlineData(SkillLevel.Advanced)]
    public async Task GetClassesBySkillLevelAsync_ShouldFilterByLevel(SkillLevel level)
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(1, level: level, status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetClassesBySkillLevelAsync(level);

        // Assert
        result.Should().HaveCount(1);
        result[0].Level.Should().Be(level);
    }

    [Fact]
    public async Task GetClassesBySkillLevelAsync_ShouldReturnOnlyAvailableClasses()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var availableClass = TestDataBuilder.CreateTestMusicClass(1, level: SkillLevel.Beginner, status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));
        var bookedClass = TestDataBuilder.CreateTestMusicClass(2, level: SkillLevel.Beginner, status: ClassStatus.Booked, scheduledDateTime: DateTime.Now.AddDays(2));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(availableClass, bookedClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetClassesBySkillLevelAsync(SkillLevel.Beginner);

        // Assert
        result.Should().HaveCount(1);
        result[0].Status.Should().Be(ClassStatus.Available);
    }

    #endregion

    #region GetClassByIdAsync Tests

    [Fact]
    public async Task GetClassByIdAsync_ShouldReturnClass_WhenExists()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(5);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetClassByIdAsync(5);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(5);
    }

    [Fact]
    public async Task GetClassByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var result = await _service.GetClassByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetClassByIdAsync_ShouldIncludeInstructorAndBooking()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1, "Test Instructor");
        var musicClass = TestDataBuilder.CreateTestMusicClass(1, instructorId: 1, status: ClassStatus.Booked);
        var booking = TestDataBuilder.CreateTestBooking(1, musicClassId: 1);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetClassByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Instructor.Should().NotBeNull();
        result.Instructor.Name.Should().Be("Test Instructor");
        result.Booking.Should().NotBeNull();
    }

    #endregion

    #region CRUD Tests

    [Fact]
    public async Task CreateClassAsync_ShouldAddClassToDatabase()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        var musicClass = TestDataBuilder.CreateTestMusicClass(1);

        // Act
        var result = await _service.CreateClassAsync(musicClass);

        // Assert
        var savedClass = await _context.MusicClasses.FindAsync(1);
        savedClass.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateClassAsync_ShouldPersistChanges()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(1, instrument: "Piano");

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        musicClass.Instrument = "Guitar";
        musicClass.Price = 75.00m;
        await _service.UpdateClassAsync(musicClass);

        // Assert
        var updatedClass = await _context.MusicClasses.FindAsync(1);
        updatedClass.Should().NotBeNull();
        updatedClass!.Instrument.Should().Be("Guitar");
        updatedClass.Price.Should().Be(75.00m);
    }

    [Fact]
    public async Task DeleteClassAsync_ShouldRemoveClass_WhenExists()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(1);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        await _service.DeleteClassAsync(1);

        // Assert
        var deletedClass = await _context.MusicClasses.FindAsync(1);
        deletedClass.Should().BeNull();
    }

    [Fact]
    public async Task DeleteClassAsync_ShouldNotThrow_WhenNotExists()
    {
        // Act
        var act = async () => await _service.DeleteClassAsync(999);

        // Assert
        await act.Should().NotThrowAsync();
    }

    #endregion

    #region GetAvailableInstrumentsAsync Tests

    [Fact]
    public async Task GetAvailableInstrumentsAsync_ShouldReturnDistinctInstruments()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var piano1 = TestDataBuilder.CreateTestMusicClass(1, instrument: "Piano", status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));
        var piano2 = TestDataBuilder.CreateTestMusicClass(2, instrument: "Piano", status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(2));
        var guitar = TestDataBuilder.CreateTestMusicClass(3, instrument: "Guitar", status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(piano1, piano2, guitar);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAvailableInstrumentsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain("Piano");
        result.Should().Contain("Guitar");
    }

    [Fact]
    public async Task GetAvailableInstrumentsAsync_ShouldReturnOnlyFromAvailableClasses()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var availablePiano = TestDataBuilder.CreateTestMusicClass(1, instrument: "Piano", status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));
        var bookedViolin = TestDataBuilder.CreateTestMusicClass(2, instrument: "Violin", status: ClassStatus.Booked, scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(availablePiano, bookedViolin);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAvailableInstrumentsAsync();

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain("Piano");
        result.Should().NotContain("Violin");
    }

    [Fact]
    public async Task GetAvailableInstrumentsAsync_ShouldReturnOrderedAlphabetically()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var violin = TestDataBuilder.CreateTestMusicClass(1, instrument: "Violin", status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));
        var guitar = TestDataBuilder.CreateTestMusicClass(2, instrument: "Guitar", status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));
        var piano = TestDataBuilder.CreateTestMusicClass(3, instrument: "Piano", status: ClassStatus.Available, scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(violin, guitar, piano);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAvailableInstrumentsAsync();

        // Assert
        result.Should().BeInAscendingOrder();
        result[0].Should().Be("Guitar");
        result[1].Should().Be("Piano");
        result[2].Should().Be("Violin");
    }

    #endregion
}
