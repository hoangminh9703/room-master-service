using RoomMasterService.Data;

namespace RoomMasterService.Services;

public class CheckInOutService : ICheckInOutService
{
    private readonly IDataAccess _dataAccess;

    public CheckInOutService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task CheckInGuestAsync(string bookingId, string roomId)
    {
        await _dataAccess.CheckInGuestAsync(bookingId, roomId);
    }

    public async Task CheckOutGuestAsync(string bookingId, string roomId)
    {
        await _dataAccess.CheckOutGuestAsync(bookingId, roomId);
    }
}
