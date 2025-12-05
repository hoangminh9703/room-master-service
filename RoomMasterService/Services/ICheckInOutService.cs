namespace RoomMasterService.Services;

public interface ICheckInOutService
{
    Task CheckInGuestAsync(string bookingId, string roomId);
    Task CheckOutGuestAsync(string bookingId, string roomId);
}
