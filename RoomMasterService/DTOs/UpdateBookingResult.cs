namespace RoomMasterService.DTOs;

public enum UpdateBookingResultCode
{
    UnknownError = -1,
    Success = 0,
    BookingNotFound = 1,
    InvalidDateRange = 2,
    RoomConflict = 3,
    InvalidStatus = 4,
    MissingRequiredValue = 5,
    DatabaseError = 6,
    RoomNotFound = 7,
    BookingCancelled = 8,
    InvalidTransition = 9
}

public class UpdateBookingResult
{
    public UpdateBookingResultCode Code { get; set; }
    public string Message { get; set; } = string.Empty;
}
