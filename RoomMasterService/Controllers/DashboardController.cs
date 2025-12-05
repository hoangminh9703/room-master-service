using Microsoft.AspNetCore.Mvc;
using RoomMasterService.DTOs;
using RoomMasterService.Services;

namespace RoomMasterService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    [HttpGet("occupancy-stats")]
    public async Task<ActionResult<ApiResponse<object>>> GetOccupancyStats()
    {
        try
        {
            var stats = await _dashboardService.GetOccupancyStatsAsync();
            return Ok(new ApiResponse<object> { Success = true, Data = stats });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting occupancy stats");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost("revenue-report")]
    public async Task<ActionResult<ApiResponse<object>>> GetRevenueReport([FromBody] DateRangeRequest request)
    {
        try
        {
            var report = await _dashboardService.GetRevenueReportAsync(request.StartDate, request.EndDate);
            return Ok(new ApiResponse<object> { Success = true, Data = report });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting revenue report");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }
}
