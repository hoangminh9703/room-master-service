namespace RoomMasterService.DTOs;

public class CreateBookingRequest
{
    public string GuestId { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string? SpecialRequests { get; set; }
}
