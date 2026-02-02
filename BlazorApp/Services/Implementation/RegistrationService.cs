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
        var year = DateTime.UtcNow.Year;
        var prefix = $"NADIRITMA-KB-{year}-";

        var lastRegistration = await _context.Registrations
            .AsNoTracking()
            .Where(r => r.ReferenceNumber.StartsWith(prefix))
            .OrderByDescending(r => r.ReferenceNumber)
            .FirstOrDefaultAsync();

        int nextNumber = 1;

        if (lastRegistration != null)
        {
            var lastNumberStr = lastRegistration.ReferenceNumber.Replace(prefix, "");
            if (int.TryParse(lastNumberStr, out var lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"{prefix}{nextNumber:D3}";
    }
}
