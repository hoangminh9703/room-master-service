namespace RoomMasterService.DTOs;

public class SearchBookingsRequest
{
    public string? Keyword { get; set; }
    public DateTime? Date { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Status { get; set; }

    // New filters
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? SearchCheckInDate { get; set; }
    public DateTime? SearchCheckOutDate { get; set; }
    public int? Type { get; set; }
}
