namespace BlazorApp.Models.DTOs;

public class BookingRequest
{
    public int MusicClassId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public string StudentPhone { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
