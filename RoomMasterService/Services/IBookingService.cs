using RoomMasterService.DTOs;
using RoomMasterService.Models;

namespace RoomMasterService.Services;

public interface IBookingService
{
    Task<Booking?> GetBookingByIdAsync(string bookingId);
    Task<(string bookingId, string bookingReference)> CreateBookingAsync(CreateBookingRequest request);
    Task UpdateBookingAsync(string bookingId, UpdateBookingRequest request);
    Task CancelBookingAsync(string bookingId);
    Task<List<Booking>> GetBookingsByGuestAsync(string guestId);
    Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
}
