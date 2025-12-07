namespace RoomMasterService.Models;

public class Guest
{
    public Guid Guest_Id { get; set; } 
    public string Full_Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? IdType { get; set; }
    public string? IdNumber { get; set; }
    public string? Nationality { get; set; }
    public DateTime? Date_Of_Birth { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
