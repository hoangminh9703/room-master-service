namespace RoomMasterService.Models;

public class DamageReport
{
    public string ReportId { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal EstimatedCost { get; set; }
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
