namespace RoomMasterService.Models;

public class Room
{
    public string Room_Id { get; set; } 
    public string Room_Number { get; set; } 
    public string Room_Type_Id { get; set; } 
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public string Status { get; set; } = "Available";
    public string? Features { get; set; }
    public decimal Price_Per_Night { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
