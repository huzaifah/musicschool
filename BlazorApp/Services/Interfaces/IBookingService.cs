using BlazorApp.Data.Entities;
using BlazorApp.Models.DTOs;

namespace BlazorApp.Services.Interfaces;

public interface IBookingService
{
    Task<Booking> CreateBookingAsync(BookingRequest request);
    Task<List<Booking>> GetBookingsByInstructorAsync(int instructorId);
    Task<List<Booking>> GetAllBookingsAsync();
    Task<Booking?> GetBookingByIdAsync(int id);
    Task CancelBookingAsync(int bookingId);
    Task<bool> ValidateBookingAsync(int classId);
}
