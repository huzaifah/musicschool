using BlazorApp.Models;

namespace BlazorApp.Services.Interfaces;

public interface IViewModeService
{
    ViewMode CurrentMode { get; }
    int? CurrentInstructorId { get; set; }
    event Action? OnChange;
    void SetViewMode(ViewMode mode);
}
