using BlazorApp.Data;
using BlazorApp.Data.Entities;
using BlazorApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services.Implementation;

public class InstructorService : IInstructorService
{
    private readonly ApplicationDbContext _context;

    public InstructorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Instructor>> GetAllInstructorsAsync()
    {
        return await _context.Instructors
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    public async Task<List<Instructor>> GetActiveInstructorsAsync()
    {
        return await _context.Instructors
            .Where(i => i.IsActive)
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    public async Task<Instructor?> GetInstructorByIdAsync(int id)
    {
        return await _context.Instructors
            .Include(i => i.Classes)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Instructor> CreateInstructorAsync(Instructor instructor)
    {
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();
        return instructor;
    }

    public async Task<Instructor> UpdateInstructorAsync(Instructor instructor)
    {
        _context.Instructors.Update(instructor);
        await _context.SaveChangesAsync();
        return instructor;
    }

    public async Task DeleteInstructorAsync(int id)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor != null)
        {
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
        }
    }
}
