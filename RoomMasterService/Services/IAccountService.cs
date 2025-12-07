using RoomMasterService.Models;
using RoomMasterService.DTOs;

namespace RoomMasterService.Services;

public interface IAccountService
{
    Task<(AuthResponse? Response, string? Error)> AuthenticateAsync(string username, string password);
    Task<string> CreateAccountAsync(Account account);
    Task<(AuthResponse? Response, string? Error)> RefreshTokenAsync(string refreshToken);
}
