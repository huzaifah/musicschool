using BlazorApp.Data.Enums;

namespace BlazorApp.Data.Entities;

public class Booking
{
    public int Id { get; set; }
    public int MusicClassId { get; set; }

    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public string StudentPhone { get; set; } = string.Empty;
    public string? Notes { get; set; }

    public DateTime BookedAt { get; set; } = DateTime.UtcNow;
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    public MusicClass MusicClass { get; set; } = null!;
}
