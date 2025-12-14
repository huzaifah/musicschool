using BlazorApp.Data;
using BlazorApp.Data.Entities;
using BlazorApp.Data.Enums;
using BlazorApp.Services.Implementation;
using BlazorApp.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace BlazorApp.UnitTests.Services;

public class BookingServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly BookingService _service;

    public BookingServiceTests()
    {
        _context = MockDbContextFactory.CreateMockContext();
        _service = new BookingService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    #region ValidateBookingAsync Tests

    [Fact]
    public async Task ValidateBookingAsync_ShouldReturnTrue_WhenClassIsValid()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Available,
            scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ValidateBookingAsync(1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateBookingAsync_ShouldReturnFalse_WhenClassDoesNotExist()
    {
        // Act
        var result = await _service.ValidateBookingAsync(999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateBookingAsync_ShouldReturnFalse_WhenClassIsBooked()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Booked,
            scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ValidateBookingAsync(1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateBookingAsync_ShouldReturnFalse_WhenClassIsCancelled()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Cancelled,
            scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ValidateBookingAsync(1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateBookingAsync_ShouldReturnFalse_WhenClassIsCompleted()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Completed,
            scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ValidateBookingAsync(1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateBookingAsync_ShouldReturnFalse_WhenClassIsInPast()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Available,
            scheduledDateTime: DateTime.Now.AddDays(-1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ValidateBookingAsync(1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateBookingAsync_ShouldReturnTrue_WhenClassIsInFuture()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Available,
            scheduledDateTime: DateTime.Now.AddHours(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ValidateBookingAsync(1);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region CreateBookingAsync Tests

    [Fact]
    public async Task CreateBookingAsync_ShouldCreateBooking_WhenValid()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Available,
            scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        var request = TestDataBuilder.CreateTestBookingRequest(musicClassId: 1);

        // Act
        var result = await _service.CreateBookingAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.StudentName.Should().Be(request.StudentName);
        result.StudentEmail.Should().Be(request.StudentEmail);
        result.StudentPhone.Should().Be(request.StudentPhone);
        result.Notes.Should().Be(request.Notes);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldSetBookedAtToUtcNow()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Available,
            scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        var request = TestDataBuilder.CreateTestBookingRequest(musicClassId: 1);
        var beforeBooking = DateTime.UtcNow;

        // Act
        var result = await _service.CreateBookingAsync(request);
        var afterBooking = DateTime.UtcNow;

        // Assert
        result.BookedAt.Should().BeOnOrAfter(beforeBooking);
        result.BookedAt.Should().BeOnOrBefore(afterBooking);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldSetStatusToConfirmed()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Available,
            scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        var request = TestDataBuilder.CreateTestBookingRequest(musicClassId: 1);

        // Act
        var result = await _service.CreateBookingAsync(request);

        // Assert
        result.Status.Should().Be(BookingStatus.Confirmed);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldUpdateClassStatusToBooked()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Available,
            scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        var request = TestDataBuilder.CreateTestBookingRequest(musicClassId: 1);

        // Act
        await _service.CreateBookingAsync(request);

        // Assert
        var updatedClass = await _context.MusicClasses.FindAsync(1);
        updatedClass.Should().NotBeNull();
        updatedClass!.Status.Should().Be(ClassStatus.Booked);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowException_WhenClassNotAvailable()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Booked,
            scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        var request = TestDataBuilder.CreateTestBookingRequest(musicClassId: 1);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _service.CreateBookingAsync(request));
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowException_WhenValidationFails()
    {
        // Arrange
        var request = TestDataBuilder.CreateTestBookingRequest(musicClassId: 999);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _service.CreateBookingAsync(request));
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldSaveChanges()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Available,
            scheduledDateTime: DateTime.Now.AddDays(1));

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();

        var request = TestDataBuilder.CreateTestBookingRequest(musicClassId: 1);

        // Act
        await _service.CreateBookingAsync(request);

        // Assert
        var bookings = _context.Bookings.ToList();
        bookings.Should().HaveCount(1);
    }

    #endregion

    #region CancelBookingAsync Tests

    [Fact]
    public async Task CancelBookingAsync_ShouldSetBookingStatusToCancelled()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Booked,
            scheduledDateTime: DateTime.Now.AddDays(1));
        var booking = TestDataBuilder.CreateTestBooking(
            id: 1,
            musicClassId: 1,
            status: BookingStatus.Confirmed);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        await _service.CancelBookingAsync(1);

        // Assert
        var cancelledBooking = await _context.Bookings.FindAsync(1);
        cancelledBooking.Should().NotBeNull();
        cancelledBooking!.Status.Should().Be(BookingStatus.Cancelled);
    }

    [Fact]
    public async Task CancelBookingAsync_ShouldSetClassStatusToAvailable()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Booked,
            scheduledDateTime: DateTime.Now.AddDays(1));
        var booking = TestDataBuilder.CreateTestBooking(
            id: 1,
            musicClassId: 1);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        await _service.CancelBookingAsync(1);

        // Assert
        var availableClass = await _context.MusicClasses.FindAsync(1);
        availableClass.Should().NotBeNull();
        availableClass!.Status.Should().Be(ClassStatus.Available);
    }

    [Fact]
    public async Task CancelBookingAsync_ShouldUpdateBothEntities()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(
            id: 1,
            status: ClassStatus.Booked,
            scheduledDateTime: DateTime.Now.AddDays(1));
        var booking = TestDataBuilder.CreateTestBooking(
            id: 1,
            musicClassId: 1);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        await _service.CancelBookingAsync(1);

        // Assert
        var cancelledBooking = await _context.Bookings.FindAsync(1);
        var availableClass = await _context.MusicClasses.FindAsync(1);

        cancelledBooking!.Status.Should().Be(BookingStatus.Cancelled);
        availableClass!.Status.Should().Be(ClassStatus.Available);
    }

    [Fact]
    public async Task CancelBookingAsync_ShouldNotThrow_WhenBookingNotExists()
    {
        // Act
        var act = async () => await _service.CancelBookingAsync(999);

        // Assert
        await act.Should().NotThrowAsync();
    }

    #endregion

    #region GetBookingsByInstructorAsync Tests

    [Fact]
    public async Task GetBookingsByInstructorAsync_ShouldReturnBookingsForInstructor()
    {
        // Arrange
        var instructor1 = TestDataBuilder.CreateTestInstructor(1, "Instructor 1");
        var instructor2 = TestDataBuilder.CreateTestInstructor(2, "Instructor 2");
        var class1 = TestDataBuilder.CreateTestMusicClass(1, instructorId: 1);
        var class2 = TestDataBuilder.CreateTestMusicClass(2, instructorId: 1);
        var class3 = TestDataBuilder.CreateTestMusicClass(3, instructorId: 2);
        var booking1 = TestDataBuilder.CreateTestBooking(1, musicClassId: 1);
        var booking2 = TestDataBuilder.CreateTestBooking(2, musicClassId: 2);
        var booking3 = TestDataBuilder.CreateTestBooking(3, musicClassId: 3);

        _context.Instructors.AddRange(instructor1, instructor2);
        _context.MusicClasses.AddRange(class1, class2, class3);
        _context.Bookings.AddRange(booking1, booking2, booking3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetBookingsByInstructorAsync(1);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(b => b.MusicClass.InstructorId.Should().Be(1));
    }

    [Fact]
    public async Task GetBookingsByInstructorAsync_ShouldOrderByBookedAtDescending()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1);
        var class1 = TestDataBuilder.CreateTestMusicClass(1, instructorId: 1);
        var class2 = TestDataBuilder.CreateTestMusicClass(2, instructorId: 1);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(class1, class2);
        await _context.SaveChangesAsync();

        var booking1 = new Booking
        {
            Id = 1,
            MusicClassId = 1,
            StudentName = "Student 1",
            StudentEmail = "student1@test.com",
            StudentPhone = "+1-555-1111",
            BookedAt = DateTime.UtcNow.AddHours(-2),
            Status = BookingStatus.Confirmed
        };
        var booking2 = new Booking
        {
            Id = 2,
            MusicClassId = 2,
            StudentName = "Student 2",
            StudentEmail = "student2@test.com",
            StudentPhone = "+1-555-2222",
            BookedAt = DateTime.UtcNow.AddHours(-1),
            Status = BookingStatus.Confirmed
        };

        _context.Bookings.AddRange(booking1, booking2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetBookingsByInstructorAsync(1);

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeInDescendingOrder(b => b.BookedAt);
        result[0].Id.Should().Be(2); // Most recent
        result[1].Id.Should().Be(1); // Older
    }

    [Fact]
    public async Task GetBookingsByInstructorAsync_ShouldIncludeMusicClassAndInstructor()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1, "Test Instructor");
        var musicClass = TestDataBuilder.CreateTestMusicClass(1, instructorId: 1);
        var booking = TestDataBuilder.CreateTestBooking(1, musicClassId: 1);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetBookingsByInstructorAsync(1);

        // Assert
        result.Should().HaveCount(1);
        result[0].MusicClass.Should().NotBeNull();
        result[0].MusicClass.Instructor.Should().NotBeNull();
        result[0].MusicClass.Instructor.Name.Should().Be("Test Instructor");
    }

    [Fact]
    public async Task GetBookingsByInstructorAsync_ShouldReturnEmpty_WhenNoBookings()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1);
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetBookingsByInstructorAsync(1);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region GetAllBookingsAsync Tests

    [Fact]
    public async Task GetAllBookingsAsync_ShouldReturnAllBookings()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var class1 = TestDataBuilder.CreateTestMusicClass(1);
        var class2 = TestDataBuilder.CreateTestMusicClass(2);
        var booking1 = TestDataBuilder.CreateTestBooking(1, musicClassId: 1);
        var booking2 = TestDataBuilder.CreateTestBooking(2, musicClassId: 2);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(class1, class2);
        _context.Bookings.AddRange(booking1, booking2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllBookingsAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllBookingsAsync_ShouldOrderByBookedAtDescending()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var class1 = TestDataBuilder.CreateTestMusicClass(1);
        var class2 = TestDataBuilder.CreateTestMusicClass(2);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.AddRange(class1, class2);
        await _context.SaveChangesAsync();

        var booking1 = new Booking
        {
            Id = 1,
            MusicClassId = 1,
            StudentName = "Student 1",
            StudentEmail = "student1@test.com",
            StudentPhone = "+1-555-1111",
            BookedAt = DateTime.UtcNow.AddDays(-2),
            Status = BookingStatus.Confirmed
        };
        var booking2 = new Booking
        {
            Id = 2,
            MusicClassId = 2,
            StudentName = "Student 2",
            StudentEmail = "student2@test.com",
            StudentPhone = "+1-555-2222",
            BookedAt = DateTime.UtcNow.AddDays(-1),
            Status = BookingStatus.Confirmed
        };

        _context.Bookings.AddRange(booking1, booking2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllBookingsAsync();

        // Assert
        result.Should().BeInDescendingOrder(b => b.BookedAt);
    }

    [Fact]
    public async Task GetAllBookingsAsync_ShouldIncludeNavigationProperties()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1, "Test Instructor");
        var musicClass = TestDataBuilder.CreateTestMusicClass(1, instructorId: 1);
        var booking = TestDataBuilder.CreateTestBooking(1, musicClassId: 1);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllBookingsAsync();

        // Assert
        result.Should().HaveCount(1);
        result[0].MusicClass.Should().NotBeNull();
        result[0].MusicClass.Instructor.Should().NotBeNull();
    }

    #endregion

    #region GetBookingByIdAsync Tests

    [Fact]
    public async Task GetBookingByIdAsync_ShouldReturnBooking_WhenExists()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor();
        var musicClass = TestDataBuilder.CreateTestMusicClass(1);
        var booking = TestDataBuilder.CreateTestBooking(5, musicClassId: 1);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetBookingByIdAsync(5);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(5);
    }

    [Fact]
    public async Task GetBookingByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var result = await _service.GetBookingByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetBookingByIdAsync_ShouldIncludeNavigationProperties()
    {
        // Arrange
        var instructor = TestDataBuilder.CreateTestInstructor(1, "Test Instructor");
        var musicClass = TestDataBuilder.CreateTestMusicClass(1, instructorId: 1, instrument: "Piano");
        var booking = TestDataBuilder.CreateTestBooking(1, musicClassId: 1);

        _context.Instructors.Add(instructor);
        _context.MusicClasses.Add(musicClass);
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetBookingByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.MusicClass.Should().NotBeNull();
        result.MusicClass.Instrument.Should().Be("Piano");
        result.MusicClass.Instructor.Should().NotBeNull();
        result.MusicClass.Instructor.Name.Should().Be("Test Instructor");
    }

    #endregion
}
