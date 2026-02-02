using BlazorApp.Data.Entities;
using BlazorApp.Models;

namespace BlazorApp.Services.Interfaces;

/// <summary>
/// Perkhidmatan untuk menguruskan pendaftaran
/// </summary>
public interface IRegistrationService
{
    /// <summary>
    /// Cipta pendaftaran baharu
    /// </summary>
    Task<Registration> CreateRegistrationAsync(RegistrationFormModel model);

    /// <summary>
    /// Dapatkan pendaftaran berdasarkan nombor rujukan
    /// </summary>
    Task<Registration?> GetByReferenceAsync(string referenceNumber);

    /// <summary>
    /// Jana nombor rujukan unik
    /// </summary>
    Task<string> GenerateReferenceNumberAsync();
}
