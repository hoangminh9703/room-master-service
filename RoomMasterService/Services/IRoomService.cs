using RoomMasterService.Models;

namespace RoomMasterService.Services;

public interface IRoomService
{
    Task<Room?> GetRoomByIdAsync(string roomId);
    Task<List<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate, string? roomTypeId);
    Task<string> CreateRoomAsync(Room room);
    Task UpdateRoomStatusAsync(string roomId, string status);
}
