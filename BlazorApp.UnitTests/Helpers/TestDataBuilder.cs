using BlazorApp.Data.Entities;
using BlazorApp.Data.Enums;
using BlazorApp.Models;

namespace BlazorApp.UnitTests.Helpers;

public static class TestDataBuilder
{
    public static Registration CreateTestRegistration(
        Guid? id = null,
        string referenceNumber = "NR-TEST1234",
        Venue venue = Venue.SriAlAminCherasSelatan,
        string studentName = "Ahmad bin Ali",
        DateOnly? dateOfBirth = null,
        Gender gender = Gender.Lelaki,
        string schoolYear = "Tahun 3",
        MusicExperience musicExperience = MusicExperience.TiadaPengalaman,
        string guardianName = "Ali bin Abu",
        string guardianPhone = "012-345 6789",
        string guardianEmail = "ali@test.com",
        string? classSlot = "Sabtu, 11:30 pagi",
        RegistrationStatus status = RegistrationStatus.Pending)
    {
        return new Registration
        {
            Id = id ?? Guid.NewGuid(),
            ReferenceNumber = referenceNumber,
            Venue = venue,
            StudentName = studentName,
            DateOfBirth = dateOfBirth ?? DateOnly.FromDateTime(DateTime.Today.AddYears(-8)),
            Gender = gender,
            SchoolYear = schoolYear,
            MusicExperience = musicExperience,
            GuardianName = guardianName,
            GuardianPhone = guardianPhone,
            GuardianEmail = guardianEmail,
            Address = "123 Jalan Test",
            Postcode = "40000",
            City = "Shah Alam",
            State = "Selangor",
            EmergencyName = "Fatimah binti Ahmad",
            EmergencyPhone = "013-456 7890",
            EmergencyRelationship = "Nenek",
            ClassSlot = classSlot,
            AgreeTerms = true,
            AgreeParticipation = true,
            AgreeMedia = true,
            SignatoryName = guardianName,
            Status = status,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static RegistrationFormModel CreateTestRegistrationFormModel(
        Venue selectedVenue = Venue.SriAlAminCherasSelatan,
        string studentName = "Ahmad bin Ali",
        DateOnly? dateOfBirth = null,
        Gender gender = Gender.Lelaki,
        string schoolYear = "Tahun 3",
        MusicExperience musicExperience = MusicExperience.TiadaPengalaman,
        string guardianName = "Ali bin Abu",
        string guardianPhone = "012-345 6789",
        string guardianEmail = "ali@test.com",
        string classSlot = "Sabtu, 11:30 pagi")
    {
        return new RegistrationFormModel
        {
            SelectedVenue = selectedVenue,
            StudentName = studentName,
            DateOfBirth = dateOfBirth ?? DateOnly.FromDateTime(DateTime.Today.AddYears(-8)),
            Gender = gender,
            SchoolYear = schoolYear,
            MusicExperience = musicExperience,
            GuardianName = guardianName,
            GuardianPhone = guardianPhone,
            GuardianEmail = guardianEmail,
            Address = "123 Jalan Test",
            Postcode = "40000",
            City = "Shah Alam",
            State = "Selangor",
            EmergencyName = "Fatimah binti Ahmad",
            EmergencyPhone = "013-456 7890",
            EmergencyRelationship = "Nenek",
            ClassSlot = classSlot,
            AgreeTerms = true,
            AgreeParticipation = true,
            AgreeMedia = true,
            SignatoryName = guardianName
        };
    }
}
