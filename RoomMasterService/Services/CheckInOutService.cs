using RoomMasterService.Data;

namespace RoomMasterService.Services;

public class CheckInOutService : ICheckInOutService
{
    private readonly IDataAccess _dataAccess;

    public CheckInOutService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<int> CheckInGuestAsync(string bookingId, string roomId)
    {
        return await _dataAccess.CheckInGuestAsync(bookingId, roomId);
    }

    public async Task<int> CheckOutGuestAsync(string bookingId, string roomId)
    {
        return await _dataAccess.CheckOutGuestAsync(bookingId, roomId);
    }

    public async Task<int> BookingCheckInOutAsync(string bookingId, int type)
    {
        return await _dataAccess.BookingCheckInOutAsync(bookingId, type);
    }
}
