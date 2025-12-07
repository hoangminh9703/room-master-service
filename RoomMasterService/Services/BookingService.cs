using RoomMasterService.Data;
using RoomMasterService.DTOs;
using RoomMasterService.Models;

namespace RoomMasterService.Services;

public class BookingService : IBookingService
{
    private readonly IDataAccess _dataAccess;

    public BookingService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<Booking?> GetBookingByIdAsync(string bookingId)
    {
        return await _dataAccess.GetBookingByIdAsync(bookingId);
    }

    public async Task<(string bookingId, string bookingReference)> CreateBookingAsync(CreateBookingRequest request)
    {
        return await _dataAccess.CreateBookingAsync(
            request.GuestId,
            request.RoomId,
            request.CheckInDate,
            request.CheckOutDate,
            request.SpecialRequests,
            request.AccountId
        );
    }

    public async Task<UpdateBookingResult> UpdateBookingAsync(string bookingId, UpdateBookingRequest request)
    {
        var result = await _dataAccess.UpdateBookingAsync(bookingId, request);

        // Do not throw here; let controller handle different result codes
        return result;
    }

    public async Task CancelBookingAsync(string bookingId)
    {
        await _dataAccess.CancelBookingAsync(bookingId);
    }

    public async Task<List<Booking>> GetBookingsByGuestAsync(string guestId)
    {
        return await _dataAccess.GetBookingsByGuestAsync(guestId);
    }

    public async Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dataAccess.GetBookingsByDateRangeAsync(startDate, endDate);
    }

    public async Task<(List<object> Bookings, int TotalRows)> SearchBookingsAsync(string? keyword, DateTime? date, int pageIndex, int pageSize, string? status, DateTime? fromDate, DateTime? toDate, DateTime? searchCheckInDate, DateTime? searchCheckOutDate, int? type)
    {
        return await _dataAccess.SearchBookingsAsync(keyword, date, pageIndex, pageSize, status, fromDate, toDate, searchCheckInDate, searchCheckOutDate, type);
    }
}
