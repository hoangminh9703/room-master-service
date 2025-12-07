namespace RoomMasterService.Models;

public class Transaction
{
    public string TransactionId { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = "Completed";
    public string? ReferenceNo { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
