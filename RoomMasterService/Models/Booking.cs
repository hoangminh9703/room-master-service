namespace RoomMasterService.Models;

public class Booking
{
    public string BookingId { get; set; } = string.Empty;
    public string BookingReference { get; set; } = string.Empty;
    public string GuestId { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfNights { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal DepositPaid { get; set; }
    public string Status { get; set; } = "Pending";
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
