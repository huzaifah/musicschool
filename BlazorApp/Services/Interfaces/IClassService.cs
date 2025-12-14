using BlazorApp.Data.Entities;
using BlazorApp.Data.Enums;

namespace BlazorApp.Services.Interfaces;

public interface IClassService
{
    Task<List<MusicClass>> GetAvailableClassesAsync();
    Task<List<MusicClass>> GetClassesByInstructorAsync(int instructorId);
    Task<List<MusicClass>> GetClassesByInstrumentAsync(string instrument);
    Task<List<MusicClass>> GetClassesBySkillLevelAsync(SkillLevel skillLevel);
    Task<MusicClass?> GetClassByIdAsync(int id);
    Task<MusicClass> CreateClassAsync(MusicClass musicClass);
    Task<MusicClass> UpdateClassAsync(MusicClass musicClass);
    Task DeleteClassAsync(int id);
    Task<List<string>> GetAvailableInstrumentsAsync();
}
