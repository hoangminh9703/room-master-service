using RoomMasterService.DTOs;
using RoomMasterService.Models;

namespace RoomMasterService.Data;

public interface IDataAccess
{
    Task<Guest?> GetGuestByIdAsync(string guestId);
    Task<List<Guest>> SearchGuestAsync(string keyword);
    Task<string> CreateGuestAsync(Guest guest);
    Task UpdateGuestAsync(string guestId, string fullName, string? email, string phone);

    Task<Room?> GetRoomByIdAsync(string roomId);
    Task<List<Room>> GetAllRoomsAsync();
    Task<string> CreateRoomAsync(Room room);
    Task UpdateRoomStatusAsync(string roomId, string status);
    Task<List<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate, string? roomTypeId);

    Task<Booking?> GetBookingByIdAsync(string bookingId);
    Task<(string bookingId, string bookingReference)> CreateBookingAsync(string guestId, string roomId, DateTime checkInDate, DateTime checkOutDate, string? specialRequests);
    Task<int> UpdateBookingAsync(string bookingId, UpdateBookingRequest request);
    Task CancelBookingAsync(string bookingId);
    Task<List<Booking>> GetBookingsByGuestAsync(string guestId);
    Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);

    Task CheckInGuestAsync(string bookingId, string roomId);
    Task CheckOutGuestAsync(string bookingId, string roomId);

    Task<OccupancyStats?> GetOccupancyStatsAsync();
    Task<RevenueReport?> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
}

public class OccupancyStats
{
    public int TotalRooms { get; set; }
    public int AvailableRooms { get; set; }
    public int OccupiedRooms { get; set; }
}

public class RevenueReport
{
    public decimal TotalRevenue { get; set; }
    public int TotalBookings { get; set; }
}
