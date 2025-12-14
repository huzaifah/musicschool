using BlazorApp.Data.Entities;
using BlazorApp.Data.Enums;
using BlazorApp.Models.DTOs;

namespace BlazorApp.UnitTests.Helpers;

public static class TestDataBuilder
{
    public static Instructor CreateTestInstructor(
        int id = 1,
        string name = "Test Instructor",
        bool isActive = true,
        decimal hourlyRate = 50.00m)
    {
        return new Instructor
        {
            Id = id,
            Name = name,
            Email = $"{name.Replace(" ", ".").ToLower()}@test.com",
            Phone = "+1-555-0000",
            Bio = "Test bio",
            Specialization = "Piano",
            HourlyRate = hourlyRate,
            IsActive = isActive,
            ImageUrl = null
        };
    }

    public static MusicClass CreateTestMusicClass(
        int id = 1,
        int instructorId = 1,
        ClassStatus status = ClassStatus.Available,
        DateTime? scheduledDateTime = null,
        string instrument = "Piano",
        SkillLevel level = SkillLevel.Beginner)
    {
        return new MusicClass
        {
            Id = id,
            InstructorId = instructorId,
            Instrument = instrument,
            Level = level,
            ScheduledDateTime = scheduledDateTime ?? DateTime.Now.AddDays(7),
            DurationMinutes = 60,
            Price = 50.00m,
            Description = "Test class description",
            Status = status
        };
    }

    public static Booking CreateTestBooking(
        int id = 1,
        int musicClassId = 1,
        BookingStatus status = BookingStatus.Confirmed,
        string studentName = "Test Student",
        string studentEmail = "test@student.com")
    {
        return new Booking
        {
            Id = id,
            MusicClassId = musicClassId,
            StudentName = studentName,
            StudentEmail = studentEmail,
            StudentPhone = "+1-555-1111",
            Notes = "Test notes",
            BookedAt = DateTime.UtcNow,
            Status = status
        };
    }

    public static BookingRequest CreateTestBookingRequest(
        int musicClassId = 1,
        string studentName = "Test Student",
        string studentEmail = "test@student.com")
    {
        return new BookingRequest
        {
            MusicClassId = musicClassId,
            StudentName = studentName,
            StudentEmail = studentEmail,
            StudentPhone = "+1-555-1111",
            Notes = "Test notes"
        };
    }
}
