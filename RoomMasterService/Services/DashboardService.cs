using RoomMasterService.Data;

namespace RoomMasterService.Services;

public class DashboardService : IDashboardService
{
    private readonly IDataAccess _dataAccess;

    public DashboardService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<OccupancyStats?> GetOccupancyStatsAsync()
    {
        return await _dataAccess.GetOccupancyStatsAsync();
    }

    public async Task<RevenueReport?> GetRevenueReportAsync(DateTime startDate, DateTime endDate)
    {
        return await _dataAccess.GetRevenueReportAsync(startDate, endDate);
    }
}
