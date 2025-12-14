using BlazorApp.Data.Enums;

namespace BlazorApp.Data.Entities;

public class MusicClass
{
    public int Id { get; set; }
    public int InstructorId { get; set; }
    public string Instrument { get; set; } = string.Empty;
    public SkillLevel Level { get; set; }
    public DateTime ScheduledDateTime { get; set; }
    public int DurationMinutes { get; set; } = 60;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public ClassStatus Status { get; set; } = ClassStatus.Available;

    public Instructor Instructor { get; set; } = null!;
    public Booking? Booking { get; set; }
}
