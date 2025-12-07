namespace RoomMasterService.Models;

public class Account
{
    public string Account_Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // stored hashed/plain depending on DB
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int Role { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // JWT refresh token fields
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }
}
