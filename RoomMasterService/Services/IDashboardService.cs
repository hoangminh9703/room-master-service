using RoomMasterService.Data;

namespace RoomMasterService.Services;

public interface IDashboardService
{
    Task<OccupancyStats?> GetOccupancyStatsAsync();
    Task<RevenueReport?> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
}
