using Microsoft.AspNetCore.Mvc;
using RoomMasterService.DTOs;
using RoomMasterService.Services;

namespace RoomMasterService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestsController : ControllerBase
{
    private readonly IGuestService _guestService;
    private readonly ILogger<GuestsController> _logger;

    public GuestsController(IGuestService guestService, ILogger<GuestsController> logger)
    {
        _guestService = guestService;
        _logger = logger;
    }

    [HttpGet("{guestId}")]
    public async Task<ActionResult<ApiResponse<object>>> GetGuest(string guestId)
    {
        try
        {
            var guest = await _guestService.GetGuestByIdAsync(guestId);
            if (guest == null)
                return NotFound(new ApiResponse { Success = false, Message = "Guest not found" });

            return Ok(new ApiResponse<object> { Success = true, Data = guest });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting guest");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpGet("search/{keyword}")]
    public async Task<ActionResult<ApiResponse<object>>> SearchGuest(string keyword)
    {
        try
        {
            var guests = await _guestService.SearchGuestAsync(keyword);
            return Ok(new ApiResponse<object> { Success = true, Data = guests });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching guests");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateGuest([FromBody] CreateGuestRequest request)
    {
        try
        {
            var guestId = await _guestService.CreateGuestAsync(request);
            return Ok(new ApiResponse<object> { Success = true, Message = "Guest created successfully", Data = new { GuestId = guestId } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating guest");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPut("{guestId}")]
    public async Task<ActionResult<ApiResponse>> UpdateGuest(string guestId, [FromBody] UpdateGuestRequest request)
    {
        try
        {
            await _guestService.UpdateGuestAsync(guestId, request);
            return Ok(new ApiResponse { Success = true, Message = "Guest updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating guest");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }
}
