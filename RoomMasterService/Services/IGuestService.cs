using RoomMasterService.DTOs;
using RoomMasterService.Models;

namespace RoomMasterService.Services;

public interface IGuestService
{
    Task<Guest?> GetGuestByIdAsync(string guestId);

    Task<List<Guest>> SearchGuestAsync(string keyword);
    Task<string> CreateGuestAsync(CreateGuestRequest request);
    Task UpdateGuestAsync(string guestId, UpdateGuestRequest request);
}
