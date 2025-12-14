using BlazorApp.Data;
using BlazorApp.Data.Entities;
using BlazorApp.Data.Enums;
using BlazorApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services.Implementation;

public class ClassService : IClassService
{
    private readonly ApplicationDbContext _context;

    public ClassService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MusicClass>> GetAvailableClassesAsync()
    {
        return await _context.MusicClasses
            .Include(c => c.Instructor)
            .Where(c => c.Status == ClassStatus.Available && c.ScheduledDateTime > DateTime.Now)
            .OrderBy(c => c.ScheduledDateTime)
            .ToListAsync();
    }

    public async Task<List<MusicClass>> GetClassesByInstructorAsync(int instructorId)
    {
        return await _context.MusicClasses
            .Include(c => c.Instructor)
            .Include(c => c.Booking)
            .Where(c => c.InstructorId == instructorId && c.ScheduledDateTime > DateTime.Now)
            .OrderBy(c => c.ScheduledDateTime)
            .ToListAsync();
    }

    public async Task<List<MusicClass>> GetClassesByInstrumentAsync(string instrument)
    {
        return await _context.MusicClasses
            .Include(c => c.Instructor)
            .Where(c => c.Instrument == instrument &&
                       c.Status == ClassStatus.Available &&
                       c.ScheduledDateTime > DateTime.Now)
            .OrderBy(c => c.ScheduledDateTime)
            .ToListAsync();
    }

    public async Task<List<MusicClass>> GetClassesBySkillLevelAsync(SkillLevel skillLevel)
    {
        return await _context.MusicClasses
            .Include(c => c.Instructor)
            .Where(c => c.Level == skillLevel &&
                       c.Status == ClassStatus.Available &&
                       c.ScheduledDateTime > DateTime.Now)
            .OrderBy(c => c.ScheduledDateTime)
            .ToListAsync();
    }

    public async Task<MusicClass?> GetClassByIdAsync(int id)
    {
        return await _context.MusicClasses
            .Include(c => c.Instructor)
            .Include(c => c.Booking)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<MusicClass> CreateClassAsync(MusicClass musicClass)
    {
        _context.MusicClasses.Add(musicClass);
        await _context.SaveChangesAsync();
        return musicClass;
    }

    public async Task<MusicClass> UpdateClassAsync(MusicClass musicClass)
    {
        _context.MusicClasses.Update(musicClass);
        await _context.SaveChangesAsync();
        return musicClass;
    }

    public async Task DeleteClassAsync(int id)
    {
        var musicClass = await _context.MusicClasses.FindAsync(id);
        if (musicClass != null)
        {
            _context.MusicClasses.Remove(musicClass);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<string>> GetAvailableInstrumentsAsync()
    {
        return await _context.MusicClasses
            .Where(c => c.Status == ClassStatus.Available && c.ScheduledDateTime > DateTime.Now)
            .Select(c => c.Instrument)
            .Distinct()
            .OrderBy(i => i)
            .ToListAsync();
    }
}
