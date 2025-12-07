using RoomMasterService.Data;
using RoomMasterService.DTOs;
using RoomMasterService.Models;

namespace RoomMasterService.Services;

public class GuestService : IGuestService
{
    private readonly IDataAccess _dataAccess;

    public GuestService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<Guest?> GetGuestByIdAsync(string guestId)
    {
        return await _dataAccess.GetGuestByIdAsync(guestId);
    }

    public async Task<List<Guest>> SearchGuestAsync(string keyword)
    {
        return await _dataAccess.SearchGuestAsync(keyword);
    }

    public async Task<string> CreateGuestAsync(CreateGuestRequest request)
    {
        var guest = new Guest
        {
            Full_Name = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            IdType = request.IdType,
            IdNumber = request.IdNumber,
            Nationality = request.Nationality,
            Date_Of_Birth = request.DateOfBirth
        };

        return await _dataAccess.CreateGuestAsync(guest);
    }

    public async Task UpdateGuestAsync(string guestId, UpdateGuestRequest request)
    {
        await _dataAccess.UpdateGuestAsync(guestId, request.FullName, request.Email, request.Phone);
    }
}
