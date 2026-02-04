using BlazorApp.Data.Entities;

namespace BlazorApp.Services.Interfaces;

/// <summary>
/// Perkhidmatan untuk pengurusan admin
/// </summary>
public interface IAdminService
{
    /// <summary>
    /// Sahkan kata laluan admin
    /// </summary>
    bool ValidatePassword(string password);

    /// <summary>
    /// Dapatkan semua pendaftaran
    /// </summary>
    Task<List<Registration>> GetAllRegistrationsAsync();

    /// <summary>
    /// Dapatkan pendaftaran dengan carian dan penapis
    /// </summary>
    Task<List<Registration>> SearchRegistrationsAsync(string? searchTerm, string? slotFilter);

    /// <summary>
    /// Dapatkan ringkasan bilangan pendaftaran mengikut slot
    /// </summary>
    Task<Dictionary<string, int>> GetSlotSummaryAsync();

    /// <summary>
    /// Kemas kini status pendaftaran
    /// </summary>
    Task<bool> UpdateStatusAsync(Guid id, Data.Enums.RegistrationStatus status);

    /// <summary>
    /// Eksport semua pendaftaran ke format CSV
    /// </summary>
    Task<string> ExportToCsvAsync();
}
