using BlazorApp.Data.Entities;

namespace BlazorApp.Services.Interfaces;

public interface IInstructorService
{
    Task<List<Instructor>> GetAllInstructorsAsync();
    Task<List<Instructor>> GetActiveInstructorsAsync();
    Task<Instructor?> GetInstructorByIdAsync(int id);
    Task<Instructor> CreateInstructorAsync(Instructor instructor);
    Task<Instructor> UpdateInstructorAsync(Instructor instructor);
    Task DeleteInstructorAsync(int id);
}
