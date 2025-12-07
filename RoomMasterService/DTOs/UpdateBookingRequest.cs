namespace RoomMasterService.DTOs;

public class UpdateBookingRequest
{
    public string Guest_Id { get; set; } = string.Empty;
    public string Room_Id { get; set; } = string.Empty;
    public DateTime? Check_In_Date { get; set; }
    public DateTime? Check_Out_Date { get; set; }
    public string? Special_Requests { get; set; }
    public decimal? Deposit_Paid { get; set; }
    public decimal? Total_Price { get; set; }
    public string? Status { get; set; }
}

