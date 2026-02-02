using BlazorApp.Models;

namespace BlazorApp.Services.Interfaces;

/// <summary>
/// Perkhidmatan untuk menguruskan keadaan borang dalam localStorage
/// </summary>
public interface IFormStateService
{
    /// <summary>
    /// Simpan keadaan borang ke localStorage
    /// </summary>
    Task SaveFormStateAsync(RegistrationFormModel model);

    /// <summary>
    /// Muat keadaan borang dari localStorage
    /// </summary>
    Task<RegistrationFormModel?> LoadFormStateAsync();

    /// <summary>
    /// Padam keadaan borang dari localStorage
    /// </summary>
    Task ClearFormStateAsync();

    /// <summary>
    /// Simpan langkah semasa
    /// </summary>
    Task SaveCurrentStepAsync(int step);

    /// <summary>
    /// Muat langkah semasa
    /// </summary>
    Task<int> LoadCurrentStepAsync();
}
