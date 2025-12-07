using Microsoft.AspNetCore.Mvc;
using RoomMasterService.DTOs;
using RoomMasterService.Models;
using RoomMasterService.Services;

namespace RoomMasterService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly ILogger<RoomsController> _logger;

    public RoomsController(IRoomService roomService, ILogger<RoomsController> logger)
    {
        _roomService = roomService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetAllRooms()
    {
        try
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(new ApiResponse<object> { Success = true, Data = rooms });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all rooms");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpGet("{roomId}")]
    public async Task<ActionResult<ApiResponse<object>>> GetRoom(string roomId)
    {
        try
        {
            var room = await _roomService.GetRoomByIdAsync(roomId);
            if (room == null)
                return NotFound(new ApiResponse { Success = false, Message = "Room not found" });

            return Ok(new ApiResponse<object> { Success = true, Data = room });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting room");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost("available")]
    public async Task<ActionResult<ApiResponse<object>>> GetAvailableRooms([FromBody] AvailableRoomsRequest request)
    {
        try
        {
            var rooms = await _roomService.GetAvailableRoomsAsync(request.CheckInDate, request.CheckOutDate); 
            return Ok(new ApiResponse<object> { Success = true, Data = rooms });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting available rooms");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateRoom([FromBody] Room room)
    {
        try
        {
            var roomId = await _roomService.CreateRoomAsync(room);
            return Ok(new ApiResponse<object> { Success = true, Message = "Room created successfully", Data = new { RoomId = roomId } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating room");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPut("{roomId}/status")]
    public async Task<ActionResult<ApiResponse>> UpdateRoomStatus(string roomId, [FromBody] UpdateRoomStatusRequest request)
    {
        try
        {
            await _roomService.UpdateRoomStatusAsync(roomId, request.Status);
            return Ok(new ApiResponse { Success = true, Message = "Room status updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating room status");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }
}

