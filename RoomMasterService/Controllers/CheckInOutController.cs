using Microsoft.AspNetCore.Mvc;
using RoomMasterService.DTOs;
using RoomMasterService.Services;

namespace RoomMasterService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckInOutController : ControllerBase
{
    private readonly ICheckInOutService _checkInOutService;
    private readonly ILogger<CheckInOutController> _logger;

    public CheckInOutController(ICheckInOutService checkInOutService, ILogger<CheckInOutController> logger)
    {
        _checkInOutService = checkInOutService;
        _logger = logger;
    }

    [HttpPost("check-in")]
    public async Task<ActionResult<ApiResponse>> CheckIn([FromBody] CheckInRequest request)
    {
        try
        {
            await _checkInOutService.CheckInGuestAsync(request.BookingId, request.RoomId);
            return Ok(new ApiResponse { Success = true, Message = "Guest checked in successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking in guest");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost("check-out")]
    public async Task<ActionResult<ApiResponse>> CheckOut([FromBody] CheckOutRequest request)
    {
        try
        {
            await _checkInOutService.CheckOutGuestAsync(request.BookingId, request.RoomId);
            return Ok(new ApiResponse { Success = true, Message = "Guest checked out successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking out guest");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }
}

public class CheckInRequest
{
    public string BookingId { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
}

public class CheckOutRequest
{
    public string BookingId { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
}
