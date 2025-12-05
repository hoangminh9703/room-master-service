namespace RoomMasterService.Models;

public class Booking
{
    public Guid Booking_Id { get; set; } 
    public string Booking_Reference { get; set; } = string.Empty;
    public string Guest_Id { get; set; } = string.Empty;
    public string Room_Id { get; set; } = string.Empty;
    public DateTime Check_In_Date { get; set; }
    public DateTime Check_Out_Date { get; set; }
    public int Number_Of_Nights { get; set; }
    public decimal Total_Price { get; set; }
    public decimal Deposit_Paid { get; set; }
    public string Status { get; set; } = "Pending";
    public string? Special_Requests { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
