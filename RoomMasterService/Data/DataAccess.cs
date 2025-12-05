using MySql.Data.MySqlClient;
using Dapper;
using RoomMasterService.Models;
using RoomMasterService.DTOs;

namespace RoomMasterService.Data;

public class DataAccess : IDataAccess
{
    private readonly string _connectionString;

    public DataAccess(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("DefaultConnection not found");
    }

    private MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    public async Task<Guest?> GetGuestByIdAsync(string guestId)
    {
        using (var connection = GetConnection())
        {
            var guest = await connection.QueryFirstOrDefaultAsync<Guest>(
                "uspGetGuestById",
                new { p_guest_id = guestId },
                commandType: System.Data.CommandType.StoredProcedure
            );
            return guest;
        }
    }

    public async Task<List<Guest>> SearchGuestAsync(string keyword)
    {
        using (var connection = GetConnection())
        {
            var guests = await connection.QueryAsync<Guest>(
                "uspSearchGuest",
                new { p_keyword = keyword },
                commandType: System.Data.CommandType.StoredProcedure
            );
            return guests.ToList();
        }
    }

    public async Task<string> CreateGuestAsync(Guest guest)
    {
        using (var connection = GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("@p_full_name", guest.Full_Name);
            parameters.Add("@p_email", guest.Email);
            parameters.Add("@p_phone", guest.Phone);
            parameters.Add("@p_id_type", guest.IdType);
            parameters.Add("@p_id_number", guest.IdNumber);
            parameters.Add("@p_nationality", guest.Nationality);
            parameters.Add("@p_date_of_birth", guest.Date_Of_Birth);
            parameters.Add("@p_guest_id", dbType: System.Data.DbType.String, size: 36, direction: System.Data.ParameterDirection.Output);

            await connection.ExecuteAsync(
                "uspCreateGuest",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return parameters.Get<string>("@p_guest_id");
        }
    }

    public async Task UpdateGuestAsync(string guestId, string fullName, string? email, string phone)
    {
        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(
                "uspUpdateGuest",
                new { p_guest_id = guestId, p_full_name = fullName, p_email = email, p_phone = phone },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
    }

    public async Task<Room?> GetRoomByIdAsync(string roomId)
    {
        using (var connection = GetConnection())
        {
            var room = await connection.QueryFirstOrDefaultAsync<Room>(
                "uspGetRoom",
                new { p_room_id = roomId },
                commandType: System.Data.CommandType.StoredProcedure
            );
            return room;
        }
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        using (var connection = GetConnection())
        {
            var rooms = await connection.QueryAsync<Room>("SELECT * FROM rooms");
            return rooms.ToList();
        }
    }

    public async Task<string> CreateRoomAsync(Room room)
    {
        using (var connection = GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("@p_room_number", room.Room_Number);
            parameters.Add("@p_room_type_id", room.Room_Type_Id);
            parameters.Add("@p_floor", room.Floor);
            parameters.Add("@p_capacity", room.Capacity);
            parameters.Add("@p_status", room.Status);
            parameters.Add("@p_price_per_night", room.Price_Per_Night);
            parameters.Add("@p_features", room.Features);
            parameters.Add("@p_room_id", dbType: System.Data.DbType.String, size: 36, direction: System.Data.ParameterDirection.Output);

            await connection.ExecuteAsync(
                "uspCreateRoom",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return parameters.Get<string>("@p_room_id");
        }
    }

    public async Task UpdateRoomStatusAsync(string roomId, string status)
    {
        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(
                "uspUpdateRoom",
                new { p_room_id = roomId, p_status = status },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
    }

    public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate, string? roomTypeId)
    {
        using (var connection = GetConnection())
        {
            var rooms = await connection.QueryAsync<Room>(
                "uspGetAvailableRooms",
                new { p_check_in_date = checkInDate, p_check_out_date = checkOutDate, p_room_type_id = roomTypeId },
                commandType: System.Data.CommandType.StoredProcedure
            );
            return rooms.ToList();
        }
    }

    public async Task<Booking?> GetBookingByIdAsync(string bookingId)
    {
        using (var connection = GetConnection())
        {
            var booking = await connection.QueryFirstOrDefaultAsync<Booking>(
                "uspGetBookingById",
                new { p_booking_id = bookingId },
                commandType: System.Data.CommandType.StoredProcedure
            );
            return booking;
        }
    }

    public async Task<(string bookingId, string bookingReference)> CreateBookingAsync(string guestId, string roomId, DateTime checkInDate, DateTime checkOutDate, string? specialRequests)
    {
        using (var connection = GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("@p_guest_id", guestId);
            parameters.Add("@p_room_id", roomId);
            parameters.Add("@p_check_in_date", checkInDate);
            parameters.Add("@p_check_out_date", checkOutDate);
            parameters.Add("@p_special_requests", specialRequests);
            parameters.Add("@p_booking_id", dbType: System.Data.DbType.String, size: 36, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@p_booking_reference", dbType: System.Data.DbType.String, size: 50, direction: System.Data.ParameterDirection.Output);

            await connection.ExecuteAsync(
                "uspCreateBooking",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return (parameters.Get<string>("@p_booking_id"), parameters.Get<string>("@p_booking_reference"));
        }
    }

    public async Task<int> UpdateBookingAsync(string bookingId, UpdateBookingRequest request)
    {
        int rows = 0;
        using (var connection = GetConnection())
        {
            rows = await connection.ExecuteAsync(
                "uspUpdateBookingFully", new
                {
                    p_booking_id = bookingId,
                    p_guest_id = request.Guest_Id,
                    p_room_id = request.Room_Id,
                    p_check_in_date = request.Check_In_Date,
                    p_check_out_date = request.Check_Out_Date,
                    p_special_requests = request.Special_Requests,
                    p_deposit_paid = request.Deposit_Paid,
                    p_total_price = request.Total_Price,
                    p_status = request.Status
                },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
        return rows;
    }

    public async Task CancelBookingAsync(string bookingId)
    {
        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(
                "uspCancelBooking",
                new { p_booking_id = bookingId },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
    }

    public async Task<List<Booking>> GetBookingsByGuestAsync(string guestId)
    {
        using (var connection = GetConnection())
        {
            var query = "SELECT * FROM bookings WHERE guest_id = @GuestId";
            var bookings = await connection.QueryAsync<Booking>(query, new { GuestId = guestId });
            return bookings.ToList();
        }
    }

    public async Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        using (var connection = GetConnection())
        {
            var query = "SELECT * FROM bookings WHERE check_in_date BETWEEN @StartDate AND @EndDate";
            var bookings = await connection.QueryAsync<Booking>(query, new { StartDate = startDate, EndDate = endDate });
            return bookings.ToList();
        }
    }

    public async Task CheckInGuestAsync(string bookingId, string roomId)
    {
        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(
                "uspCheckInGuest",
                new { p_booking_id = bookingId, p_room_id = roomId },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
    }

    public async Task CheckOutGuestAsync(string bookingId, string roomId)
    {
        using (var connection = GetConnection())
        {
            await connection.ExecuteAsync(
                "uspCheckOutGuest",
                new { p_booking_id = bookingId, p_room_id = roomId },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
    }

    public async Task<OccupancyStats?> GetOccupancyStatsAsync()
    {
        using (var connection = GetConnection())
        {
            var stats = await connection.QueryFirstOrDefaultAsync<OccupancyStats>(
                "uspGetOccupancyStats",
                new { p_start_date = DateTime.MinValue, p_end_date = DateTime.MaxValue },
                commandType: System.Data.CommandType.StoredProcedure
            );
            return stats;
        }
    }

    public async Task<RevenueReport?> GetRevenueReportAsync(DateTime startDate, DateTime endDate)
    {
        using (var connection = GetConnection())
        {
            var report = await connection.QueryFirstOrDefaultAsync<RevenueReport>(
                "uspGetRevenueReport",
                new { p_start_date = startDate, p_end_date = endDate },
                commandType: System.Data.CommandType.StoredProcedure
            );
            return report;
        }
    }

   
}
