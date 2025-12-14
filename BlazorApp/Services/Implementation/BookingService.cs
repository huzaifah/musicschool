using BlazorApp.Data;
using BlazorApp.Data.Entities;
using BlazorApp.Data.Enums;
using BlazorApp.Models.DTOs;
using BlazorApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services.Implementation;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _context;

    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Booking> CreateBookingAsync(BookingRequest request)
    {
        var isValid = await ValidateBookingAsync(request.MusicClassId);
        if (!isValid)
        {
            throw new InvalidOperationException("This class is not available for booking.");
        }

        var booking = new Booking
        {
            MusicClassId = request.MusicClassId,
            StudentName = request.StudentName,
            StudentEmail = request.StudentEmail,
            StudentPhone = request.StudentPhone,
            Notes = request.Notes,
            BookedAt = DateTime.UtcNow,
            Status = BookingStatus.Confirmed
        };

        _context.Bookings.Add(booking);

        var musicClass = await _context.MusicClasses.FindAsync(request.MusicClassId);
        if (musicClass != null)
        {
            musicClass.Status = ClassStatus.Booked;
            _context.MusicClasses.Update(musicClass);
        }

        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<List<Booking>> GetBookingsByInstructorAsync(int instructorId)
    {
        return await _context.Bookings
            .Include(b => b.MusicClass)
                .ThenInclude(c => c.Instructor)
            .Where(b => b.MusicClass.InstructorId == instructorId)
            .OrderByDescending(b => b.BookedAt)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetAllBookingsAsync()
    {
        return await _context.Bookings
            .Include(b => b.MusicClass)
                .ThenInclude(c => c.Instructor)
            .OrderByDescending(b => b.BookedAt)
            .ToListAsync();
    }

    public async Task<Booking?> GetBookingByIdAsync(int id)
    {
        return await _context.Bookings
            .Include(b => b.MusicClass)
                .ThenInclude(c => c.Instructor)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task CancelBookingAsync(int bookingId)
    {
        var booking = await _context.Bookings
            .Include(b => b.MusicClass)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking != null)
        {
            booking.Status = BookingStatus.Cancelled;
            booking.MusicClass.Status = ClassStatus.Available;

            _context.Bookings.Update(booking);
            _context.MusicClasses.Update(booking.MusicClass);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ValidateBookingAsync(int classId)
    {
        var musicClass = await _context.MusicClasses.FindAsync(classId);

        if (musicClass == null)
            return false;

        if (musicClass.Status != ClassStatus.Available)
            return false;

        if (musicClass.ScheduledDateTime <= DateTime.Now)
            return false;

        return true;
    }
}
