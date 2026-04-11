using BlazorApp.Data.Enums;

namespace BlazorApp.Models;

/// <summary>
/// Konfigurasi untuk setiap tempat kelas muzik
/// </summary>
public class VenueConfiguration
{
    /// <summary>
    /// Pengecam tempat
    /// </summary>
    public Venue Venue { get; init; }

    /// <summary>
    /// Nama penuh tempat
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Nama ringkas tempat
    /// </summary>
    public string ShortName { get; init; } = string.Empty;

    /// <summary>
    /// Alamat penuh tempat
    /// </summary>
    public string Address { get; init; } = string.Empty;

    /// <summary>
    /// Adakah tempat ini mempunyai pilihan slot kelas
    /// </summary>
    public bool HasSlotSelection { get; init; }

    /// <summary>
    /// Teks paparan jadual kelas
    /// </summary>
    public string ScheduleDisplay { get; init; } = string.Empty;

    /// <summary>
    /// Senarai slot kelas yang tersedia (kosong jika tiada pilihan slot)
    /// </summary>
    public ClassSlotOption[] AvailableSlots { get; init; } = [];

    /// <summary>
    /// Senarai semua konfigurasi tempat
    /// </summary>
    public static readonly VenueConfiguration[] AllVenues =
    [
        new VenueConfiguration
        {
            Venue = Venue.SriAlAminCherasSelatan,
            Name = "SRI AL-AMIN Cheras Selatan",
            ShortName = "Cheras Selatan",
            Address = "43200 Cheras, Selangor",
            HasSlotSelection = true,
            ScheduleDisplay = "Rabu (3:00 ptg - 3:30 ptg)\nSabtu (12:00 tghari - 12:30 tghari)",
            AvailableSlots =
            [
                new ClassSlotOption("Rabu, 3:00 ptg - 3:30 ptg", "Rabu", "3:00 PM - 3:30 PM"),
                new ClassSlotOption("Sabtu, 12:00 tghari - 12:30 tghari", "Sabtu", "12:00 PM - 12:30 PM")
            ]
        },
        new VenueConfiguration
        {
            Venue = Venue.PrimaSaujanaKajang,
            Name = "Prima Saujana Kajang",
            ShortName = "Kajang",
            Address = "43000 Kajang, Selangor",
            HasSlotSelection = false,
            ScheduleDisplay = "Jadual akan dimaklumkan",
            AvailableSlots = []
        }
    ];

    /// <summary>
    /// Dapatkan konfigurasi tempat berdasarkan enum Venue
    /// </summary>
    public static VenueConfiguration? GetVenue(Venue venue)
    {
        return AllVenues.FirstOrDefault(v => v.Venue == venue);
    }
}
