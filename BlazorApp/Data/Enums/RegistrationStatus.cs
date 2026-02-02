namespace BlazorApp.Data.Enums;

/// <summary>
/// Status pendaftaran
/// </summary>
public enum RegistrationStatus
{
    /// <summary>
    /// Menunggu pengesahan
    /// </summary>
    Pending,

    /// <summary>
    /// Disahkan
    /// </summary>
    Confirmed,

    /// <summary>
    /// Dibatalkan
    /// </summary>
    Cancelled
}
