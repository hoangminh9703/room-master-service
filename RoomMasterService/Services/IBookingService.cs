using RoomMasterService.DTOs;
using RoomMasterService.Models;

namespace RoomMasterService.Services;

public interface IBookingService
{
    Task<Booking?> GetBookingByIdAsync(string bookingId);
    Task<(string bookingId, string bookingReference)> CreateBookingAsync(CreateBookingRequest request);
    Task<UpdateBookingResult> UpdateBookingAsync(string bookingId, UpdateBookingRequest request);
    Task CancelBookingAsync(string bookingId);
    Task<List<Booking>> GetBookingsByGuestAsync(string guestId);
    Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);

    Task<(List<object> Bookings, int TotalRows)> SearchBookingsAsync(
        string? keyword, DateTime? date, int pageIndex, int pageSize, string? status,
        DateTime? fromDate, DateTime? toDate, DateTime? searchCheckInDate, DateTime? searchCheckOutDate, int? type);
}
