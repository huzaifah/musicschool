using System.Security.Cryptography;
using BlazorApp.Data;
using BlazorApp.Data.Entities;
using BlazorApp.Data.Enums;
using BlazorApp.Models;
using BlazorApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services.Implementation;

/// <summary>
/// Perkhidmatan untuk menguruskan pendaftaran
/// </summary>
public class RegistrationService : IRegistrationService
{
    private readonly ApplicationDbContext _context;
    private const string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

    public RegistrationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Registration> CreateRegistrationAsync(RegistrationFormModel model)
    {
        var referenceNumber = await GenerateReferenceNumberAsync();

        var registration = new Registration
        {
            Id = Guid.NewGuid(),
            ReferenceNumber = referenceNumber,
            StudentName = model.StudentName,
            DateOfBirth = model.DateOfBirth!.Value,
            Gender = model.Gender!.Value,
            SchoolYear = model.SchoolYear,
            MusicExperience = model.MusicExperience!.Value,
            GuardianName = model.GuardianName,
            GuardianPhone = model.GuardianPhone,
            GuardianEmail = model.GuardianEmail,
            Address = model.Address,
            Postcode = model.Postcode,
            City = model.City,
            State = model.State,
            EmergencyName = model.EmergencyName,
            EmergencyPhone = model.EmergencyPhone,
            EmergencyRelationship = model.EmergencyRelationship,
            ClassSlot = model.ClassSlot,
            AgreeTerms = model.AgreeTerms,
            AgreeParticipation = model.AgreeParticipation,
            AgreeMedia = model.AgreeMedia,
            SignatoryName = model.SignatoryName,
            Status = RegistrationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();

        return registration;
    }

    public async Task<Registration?> GetByReferenceAsync(string referenceNumber)
    {
        return await _context.Registrations
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ReferenceNumber == referenceNumber);
    }

    public async Task<string> GenerateReferenceNumberAsync()
    {
        string referenceNumber;
        bool exists;

        do
        {
            referenceNumber = GenerateSecureReference();
            exists = await _context.Registrations
                .AsNoTracking()
                .AnyAsync(r => r.ReferenceNumber == referenceNumber);
        } while (exists);

        return referenceNumber;
    }

    private static string GenerateSecureReference()
    {
        var bytes = RandomNumberGenerator.GetBytes(8);
        var chars = new char[8];

        for (int i = 0; i < 8; i++)
        {
            chars[i] = AllowedChars[bytes[i] % AllowedChars.Length];
        }

        return $"NR-{new string(chars)}";
    }
}
