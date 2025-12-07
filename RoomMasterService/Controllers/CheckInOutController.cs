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
            var rows = await _checkInOutService.BookingCheckInOutAsync(request.BookingId, 1);
            if (rows > 0)
                return Ok(new ApiResponse { Success = true, Message = "Guest checked in successfully" });

            return BadRequest(new ApiResponse { Success = false, Message = "Check-in failed: booking not found or already checked-in" });
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
            var rows = await _checkInOutService.BookingCheckInOutAsync(request.BookingId, 2);
            if (rows > 0)
                return Ok(new ApiResponse { Success = true, Message = "Guest checked out successfully" });

            return BadRequest(new ApiResponse { Success = false, Message = "Check-out failed: booking not found or already checked-out" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking out guest");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }
}


