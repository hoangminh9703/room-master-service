using RoomMasterService.Data;
using RoomMasterService.Models;

namespace RoomMasterService.Services;

public class RoomService : IRoomService
{
    private readonly IDataAccess _dataAccess;

    public RoomService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<Room?> GetRoomByIdAsync(string roomId)
    {
        return await _dataAccess.GetRoomByIdAsync(roomId);
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await _dataAccess.GetAllRoomsAsync();
    }

    public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate)
    {
        return await _dataAccess.GetAvailableRoomsAsync(checkInDate, checkOutDate);
    }

    public async Task<string> CreateRoomAsync(Room room)
    {
        return await _dataAccess.CreateRoomAsync(room);
    }

    public async Task UpdateRoomStatusAsync(string roomId, string status)
    {
        await _dataAccess.UpdateRoomStatusAsync(roomId, status);
    }
}
