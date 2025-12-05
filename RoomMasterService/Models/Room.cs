namespace RoomMasterService.Models;

public class Room
{
    public string RoomId { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomTypeId { get; set; } = string.Empty;
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public string Status { get; set; } = "Available";
    public string? Features { get; set; }
    public decimal PricePerNight { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
