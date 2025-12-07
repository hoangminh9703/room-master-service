namespace RoomMasterService.Services;

public interface ICheckInOutService
{
    Task<int> CheckInGuestAsync(string bookingId, string roomId);
    Task<int> CheckOutGuestAsync(string bookingId, string roomId);
    Task<int> BookingCheckInOutAsync(string bookingId, int type);
}
