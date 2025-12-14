using BlazorApp.Models;
using BlazorApp.Services.Interfaces;

namespace BlazorApp.Services.Implementation;

public class ViewModeService : IViewModeService
{
    private ViewMode _currentMode = ViewMode.Public;
    public ViewMode CurrentMode => _currentMode;

    public int? CurrentInstructorId { get; set; }

    public event Action? OnChange;

    public void SetViewMode(ViewMode mode)
    {
        if (_currentMode != mode)
        {
            _currentMode = mode;
            if (mode != ViewMode.Instructor)
            {
                CurrentInstructorId = null;
            }
            OnChange?.Invoke();
        }
    }
}
