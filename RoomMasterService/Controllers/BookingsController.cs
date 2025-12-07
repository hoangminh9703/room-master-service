using Microsoft.AspNetCore.Mvc;
using RoomMasterService.DTOs;
using RoomMasterService.Services;

namespace RoomMasterService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingsController> _logger;

    public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [HttpGet("{bookingId}")]
    public async Task<ActionResult<ApiResponse<object>>> GetBooking(string bookingId)
    {
        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            if (booking == null)
                return NotFound(new ApiResponse { Success = false, Message = "Booking not found" });

            return Ok(new ApiResponse<object> { Success = true, Data = booking });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting booking");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpGet("guest/{guestId}")]
    public async Task<ActionResult<ApiResponse<object>>> GetGuestBookings(string guestId)
    {
        try
        {
            var bookings = await _bookingService.GetBookingsByGuestAsync(guestId);
            return Ok(new ApiResponse<object> { Success = true, Data = bookings });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting guest bookings");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateBooking([FromBody] CreateBookingRequest request)
    {
        try
        {
            var (bookingId, bookingReference) = await _bookingService.CreateBookingAsync(request);
            return Ok(new ApiResponse<object> 
            { 
                Success = true, 
                Message = "Booking created successfully", 
                Data = new { BookingId = bookingId, BookingReference = bookingReference } 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPut("{bookingId}")]
    public async Task<ActionResult<ApiResponse>> UpdateBooking(string bookingId, [FromBody] UpdateBookingRequest request)
    {
        try
        {
            var result = await _bookingService.UpdateBookingAsync(bookingId, request);

            if (result.Code == UpdateBookingResultCode.Success)
                return Ok(new ApiResponse { Success = true, Message = result.Message });

            if (result.Code == UpdateBookingResultCode.BookingNotFound)
                return NotFound(new ApiResponse { Success = false, Message = result.Message });

            if (result.Code == UpdateBookingResultCode.InvalidDateRange)
                return BadRequest(new ApiResponse { Success = false, Message = result.Message });

            if (result.Code == UpdateBookingResultCode.RoomConflict)
                return Conflict(new ApiResponse { Success = false, Message = result.Message });

            if (result.Code == UpdateBookingResultCode.InvalidStatus)
                return BadRequest(new ApiResponse { Success = false, Message = result.Message });

            return BadRequest(new ApiResponse { Success = false, Message = "Unknown error" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost("{bookingId}/cancel")]
    public async Task<ActionResult<ApiResponse>> CancelBooking(string bookingId)
    {
        try
        {
            await _bookingService.CancelBookingAsync(bookingId);
            return Ok(new ApiResponse { Success = true, Message = "Booking cancelled successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost("date-range")]
    public async Task<ActionResult<ApiResponse<object>>> GetBookingsByDateRange([FromBody] DateRangeRequest request)
    {
        try
        {
            var bookings = await _bookingService.GetBookingsByDateRangeAsync(request.StartDate, request.EndDate);
            return Ok(new ApiResponse<object> { Success = true, Data = bookings });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookings by date range");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost("search")]
    public async Task<ActionResult<ApiResponse<object>>> SearchBookings([FromBody] SearchBookingsRequest request)
    {
        try
        {
            var (bookings, total) = await _bookingService.SearchBookingsAsync(
                request.Keyword,
                request.Date,
                request.PageIndex,
                request.PageSize,
                request.Status,
                request.FromDate,
                request.ToDate,
                request.SearchCheckInDate,
                request.SearchCheckOutDate,
                request.Type);

            return Ok(new ApiResponse<object> { Success = true, Data = new { Items = bookings, TotalRows = total } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching bookings");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }
}
