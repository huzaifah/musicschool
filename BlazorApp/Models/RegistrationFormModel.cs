using System.ComponentModel.DataAnnotations;
using BlazorApp.Data.Enums;

namespace BlazorApp.Models;

/// <summary>
/// Model borang pendaftaran dengan pengesahan
/// </summary>
public class RegistrationFormModel
{
    // ===== Langkah 1: Maklumat Pelajar =====

    /// <summary>
    /// Nama penuh pelajar
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    [StringLength(255, ErrorMessage = "Nama tidak boleh melebihi 255 aksara")]
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Tarikh lahir pelajar
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    [CustomValidation(typeof(RegistrationFormModel), nameof(ValidateAge))]
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// Jantina pelajar
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    public Gender? Gender { get; set; }

    /// <summary>
    /// Tahun persekolahan
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    public string SchoolYear { get; set; } = string.Empty;

    /// <summary>
    /// Pengalaman muzik
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    public MusicExperience? MusicExperience { get; set; }

    // ===== Langkah 2: Maklumat Penjaga =====

    /// <summary>
    /// Nama penuh ibu/bapa/penjaga
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    [StringLength(255, ErrorMessage = "Nama tidak boleh melebihi 255 aksara")]
    public string GuardianName { get; set; } = string.Empty;

    /// <summary>
    /// Nombor telefon penjaga
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    [RegularExpression(@"^01[0-9]-[0-9]{3,4} ?[0-9]{4}$",
        ErrorMessage = "Sila masukkan nombor telefon yang sah (contoh: 012-345 6789)")]
    public string GuardianPhone { get; set; } = string.Empty;

    /// <summary>
    /// Alamat e-mel penjaga
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    [EmailAddress(ErrorMessage = "Sila masukkan e-mel yang sah")]
    public string GuardianEmail { get; set; } = string.Empty;

    // ===== Langkah 3: Alamat =====

    /// <summary>
    /// Alamat penuh
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Poskod
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Sila masukkan poskod yang sah (5 digit)")]
    public string Postcode { get; set; } = string.Empty;

    /// <summary>
    /// Bandar/Kawasan
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    [StringLength(100, ErrorMessage = "Bandar tidak boleh melebihi 100 aksara")]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Negeri
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Nama kenalan kecemasan
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    [StringLength(255, ErrorMessage = "Nama tidak boleh melebihi 255 aksara")]
    public string EmergencyName { get; set; } = string.Empty;

    /// <summary>
    /// Nombor telefon kenalan kecemasan
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    [RegularExpression(@"^01[0-9]-[0-9]{3,4} ?[0-9]{4}$",
        ErrorMessage = "Sila masukkan nombor telefon yang sah (contoh: 012-345 6789)")]
    public string EmergencyPhone { get; set; } = string.Empty;

    /// <summary>
    /// Hubungan dengan pelajar
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    [StringLength(100, ErrorMessage = "Hubungan tidak boleh melebihi 100 aksara")]
    public string EmergencyRelationship { get; set; } = string.Empty;

    // ===== Langkah 4: Slot Kelas =====

    /// <summary>
    /// Slot kelas yang dipilih
    /// </summary>
    [Required(ErrorMessage = "Sila pilih slot kelas")]
    public string ClassSlot { get; set; } = string.Empty;

    // ===== Langkah 5: Persetujuan =====

    /// <summary>
    /// Bersetuju dengan terma dan syarat
    /// </summary>
    [Range(typeof(bool), "true", "true", ErrorMessage = "Anda mesti bersetuju untuk meneruskan")]
    public bool AgreeTerms { get; set; }

    /// <summary>
    /// Bersetuju dengan penyertaan aktiviti
    /// </summary>
    [Range(typeof(bool), "true", "true", ErrorMessage = "Anda mesti bersetuju untuk meneruskan")]
    public bool AgreeParticipation { get; set; }

    /// <summary>
    /// Bersetuju dengan penggunaan media
    /// </summary>
    [Range(typeof(bool), "true", "true", ErrorMessage = "Anda mesti bersetuju untuk meneruskan")]
    public bool AgreeMedia { get; set; }

    /// <summary>
    /// Nama penandatangan
    /// </summary>
    [Required(ErrorMessage = "Ruangan ini wajib diisi")]
    [StringLength(255, ErrorMessage = "Nama tidak boleh melebihi 255 aksara")]
    public string SignatoryName { get; set; } = string.Empty;

    /// <summary>
    /// Pengesahan umur pelajar (6-12 tahun)
    /// </summary>
    public static ValidationResult? ValidateAge(DateOnly? dateOfBirth, ValidationContext context)
    {
        if (!dateOfBirth.HasValue)
        {
            return new ValidationResult("Ruangan ini wajib diisi");
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - dateOfBirth.Value.Year;

        if (dateOfBirth.Value > today.AddYears(-age))
        {
            age--;
        }

        if (age < 6 || age > 12)
        {
            return new ValidationResult("Umur pelajar mestilah antara 6-12 tahun");
        }

        return ValidationResult.Success;
    }

    /// <summary>
    /// Senarai negeri di Malaysia
    /// </summary>
    public static readonly string[] MalaysianStates =
    [
        "Johor",
        "Kedah",
        "Kelantan",
        "Melaka",
        "Negeri Sembilan",
        "Pahang",
        "Perak",
        "Perlis",
        "Pulau Pinang",
        "Sabah",
        "Sarawak",
        "Selangor",
        "Terengganu",
        "Wilayah Persekutuan Kuala Lumpur",
        "Wilayah Persekutuan Labuan",
        "Wilayah Persekutuan Putrajaya"
    ];

    /// <summary>
    /// Senarai tahun persekolahan
    /// </summary>
    public static readonly string[] SchoolYears =
    [
        "Tahun 1",
        "Tahun 2",
        "Tahun 3",
        "Tahun 4",
        "Tahun 5",
        "Tahun 6"
    ];

    /// <summary>
    /// Slot kelas yang tersedia
    /// </summary>
    public static readonly ClassSlotOption[] AvailableClassSlots =
    [
        new("Sabtu, 11:30 pagi", "Sabtu", "11:30 AM"),
        new("Ahad, 11:30 pagi", "Ahad", "11:30 AM"),
        new("Rabu, 5:00 petang", "Rabu", "5:00 PM")
    ];
}

/// <summary>
/// Pilihan slot kelas
/// </summary>
public record ClassSlotOption(string Display, string Day, string Time);
