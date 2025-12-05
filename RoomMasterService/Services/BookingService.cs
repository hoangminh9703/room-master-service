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
            request.SpecialRequests
        );
    }

    public async Task UpdateBookingAsync(string bookingId, UpdateBookingRequest request)
    {
        var affected = await _dataAccess.UpdateBookingAsync(bookingId, request);

        if (affected == 0)
            throw new Exception("Booking not found or no fields changed.");
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
}
