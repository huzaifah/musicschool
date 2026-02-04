using System.Text;
using BlazorApp.Data;
using BlazorApp.Data.Entities;
using BlazorApp.Data.Enums;
using BlazorApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services.Implementation;

/// <summary>
/// Perkhidmatan untuk pengurusan admin
/// </summary>
public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AdminService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public bool ValidatePassword(string password)
    {
        var adminPassword = _configuration["AdminSettings:Password"];
        return !string.IsNullOrEmpty(adminPassword) && password == adminPassword;
    }

    public async Task<List<Registration>> GetAllRegistrationsAsync()
    {
        return await _context.Registrations
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Registration>> SearchRegistrationsAsync(string? searchTerm, string? slotFilter)
    {
        var query = _context.Registrations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(r =>
                r.StudentName.ToLower().Contains(term) ||
                r.GuardianName.ToLower().Contains(term) ||
                r.GuardianPhone.Contains(term) ||
                r.GuardianEmail.ToLower().Contains(term) ||
                r.ReferenceNumber.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(slotFilter) && slotFilter != "all")
        {
            query = query.Where(r => r.ClassSlot == slotFilter);
        }

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Dictionary<string, int>> GetSlotSummaryAsync()
    {
        return await _context.Registrations
            .AsNoTracking()
            .GroupBy(r => r.ClassSlot)
            .Select(g => new { Slot = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Slot, x => x.Count);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, RegistrationStatus status)
    {
        var registration = await _context.Registrations.FindAsync(id);
        if (registration == null)
        {
            return false;
        }

        registration.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string> ExportToCsvAsync()
    {
        var registrations = await GetAllRegistrationsAsync();
        var sb = new StringBuilder();

        // Header
        sb.AppendLine("No,Rujukan,Nama Pelajar,Tarikh Lahir,Jantina,Tahun,Pengalaman Muzik,Nama Penjaga,Telefon,E-mel,Alamat,Poskod,Bandar,Negeri,Kenalan Kecemasan,Telefon Kecemasan,Hubungan,Slot Kelas,Status,Tarikh Daftar");

        int no = 1;
        foreach (var r in registrations)
        {
            sb.AppendLine($"{no}," +
                $"\"{r.ReferenceNumber}\"," +
                $"\"{EscapeCsv(r.StudentName)}\"," +
                $"{r.DateOfBirth:yyyy-MM-dd}," +
                $"{r.Gender}," +
                $"\"{r.SchoolYear}\"," +
                $"{r.MusicExperience}," +
                $"\"{EscapeCsv(r.GuardianName)}\"," +
                $"\"{r.GuardianPhone}\"," +
                $"\"{r.GuardianEmail}\"," +
                $"\"{EscapeCsv(r.Address)}\"," +
                $"\"{r.Postcode}\"," +
                $"\"{EscapeCsv(r.City)}\"," +
                $"\"{r.State}\"," +
                $"\"{EscapeCsv(r.EmergencyName)}\"," +
                $"\"{r.EmergencyPhone}\"," +
                $"\"{EscapeCsv(r.EmergencyRelationship)}\"," +
                $"\"{EscapeCsv(r.ClassSlot)}\"," +
                $"{r.Status}," +
                $"{r.CreatedAt:yyyy-MM-dd HH:mm:ss}");
            no++;
        }

        return sb.ToString();
    }

    private static string EscapeCsv(string value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        return value.Replace("\"", "\"\"").Replace("\n", " ").Replace("\r", "");
    }
}
