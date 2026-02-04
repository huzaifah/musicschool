using BlazorApp.Data.Enums;

namespace BlazorApp.Data.Entities;

/// <summary>
/// Entiti pendaftaran kelas muzik keyboard
/// </summary>
public class Registration
{
    /// <summary>
    /// Pengecam unik pendaftaran
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombor rujukan pendaftaran (format: NADIRITMA-KB-2026-XXX)
    /// </summary>
    public string ReferenceNumber { get; set; } = null!;

    // ===== Maklumat Pelajar =====

    /// <summary>
    /// Nama penuh pelajar
    /// </summary>
    public string StudentName { get; set; } = null!;

    /// <summary>
    /// Tarikh lahir pelajar
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Jantina pelajar
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Tahun persekolahan (contoh: Tahun 1, Tahun 2, dll)
    /// </summary>
    public string SchoolYear { get; set; } = null!;

    /// <summary>
    /// Pengalaman muzik pelajar
    /// </summary>
    public MusicExperience MusicExperience { get; set; }

    // ===== Maklumat Penjaga =====

    /// <summary>
    /// Nama penuh ibu/bapa/penjaga
    /// </summary>
    public string GuardianName { get; set; } = null!;

    /// <summary>
    /// Nombor telefon penjaga (format: 01X-XXX XXXX)
    /// </summary>
    public string GuardianPhone { get; set; } = null!;

    /// <summary>
    /// Alamat e-mel penjaga
    /// </summary>
    public string GuardianEmail { get; set; } = null!;

    // ===== Alamat =====

    /// <summary>
    /// Alamat penuh
    /// </summary>
    public string Address { get; set; } = null!;

    /// <summary>
    /// Poskod (5 digit)
    /// </summary>
    public string Postcode { get; set; } = null!;

    /// <summary>
    /// Bandar/Kawasan
    /// </summary>
    public string City { get; set; } = null!;

    /// <summary>
    /// Negeri
    /// </summary>
    public string State { get; set; } = null!;

    // ===== Kenalan Kecemasan =====

    /// <summary>
    /// Nama kenalan kecemasan
    /// </summary>
    public string EmergencyName { get; set; } = null!;

    /// <summary>
    /// Nombor telefon kenalan kecemasan
    /// </summary>
    public string EmergencyPhone { get; set; } = null!;

    /// <summary>
    /// Hubungan dengan pelajar
    /// </summary>
    public string EmergencyRelationship { get; set; } = null!;

    // ===== Slot Kelas =====

    /// <summary>
    /// Slot kelas yang dipilih
    /// </summary>
    public string ClassSlot { get; set; } = null!;

    // ===== Persetujuan =====

    /// <summary>
    /// Bersetuju dengan terma dan syarat
    /// </summary>
    public bool AgreeTerms { get; set; }

    /// <summary>
    /// Bersetuju dengan penyertaan aktiviti
    /// </summary>
    public bool AgreeParticipation { get; set; }

    /// <summary>
    /// Bersetuju dengan penggunaan media
    /// </summary>
    public bool AgreeMedia { get; set; }

    /// <summary>
    /// Nama penandatangan (ibu/bapa/penjaga)
    /// </summary>
    public string SignatoryName { get; set; } = null!;

    // ===== Meta =====

    /// <summary>
    /// Status pendaftaran
    /// </summary>
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;

    /// <summary>
    /// Tarikh dan masa pendaftaran dibuat
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
