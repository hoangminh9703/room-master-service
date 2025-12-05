namespace RoomMasterService.DTOs;

public class CreateGuestRequest
{
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? IdType { get; set; }
    public string? IdNumber { get; set; }
    public string? Nationality { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
