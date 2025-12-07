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

    public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate)
    {
        using (var connection = GetConnection())
        {
            var rooms = await connection.QueryAsync<Room>(
                "uspGetAvailableRooms",
                new { p_check_in_date = checkInDate, p_check_out_date = checkOutDate},
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

    public async Task<(string bookingId, string bookingReference)> CreateBookingAsync(string guestId, string roomId, DateTime checkInDate, DateTime checkOutDate, string? specialRequests, string? accountId)
    {
        using (var connection = GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("@p_guest_id", guestId);
            parameters.Add("@p_room_id", roomId);
            parameters.Add("@p_check_in_date", checkInDate);
            parameters.Add("@p_check_out_date", checkOutDate);
            parameters.Add("@p_special_requests", specialRequests);
            // pass account id who created the booking (nullable)
            parameters.Add("@p_account_id", accountId);
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

    public async Task<UpdateBookingResult> UpdateBookingAsync(string bookingId, UpdateBookingRequest request)
    {
        using (var connection = GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_booking_id", bookingId);
            parameters.Add("p_room_id", request.Room_Id);
            parameters.Add("p_check_in_date", request.Check_In_Date);
            parameters.Add("p_check_out_date", request.Check_Out_Date);
            parameters.Add("p_special_requests", request.Special_Requests);
            parameters.Add("p_deposit_paid", request.Deposit_Paid);
            parameters.Add("p_total_price", request.Total_Price);
            parameters.Add("p_status", request.Status);

            parameters.Add("o_return_code", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            parameters.Add("o_return_message", dbType: System.Data.DbType.String, size: 255, direction: System.Data.ParameterDirection.Output);

            await connection.ExecuteAsync(
                "uspUpdateBookingFully",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );

            var code = parameters.Get<int>("o_return_code");
            var message = parameters.Get<string>("o_return_message");

            return new UpdateBookingResult
            {
                Code = (UpdateBookingResultCode)code,
                Message = message ?? string.Empty
            };
        }
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

    public async Task<int> CheckInGuestAsync(string bookingId, string roomId)
    {
        using (var connection = GetConnection())
        {
            var rows = await connection.ExecuteAsync(
                "uspCheckInGuest",
                new { p_booking_id = bookingId, p_room_id = roomId },
                commandType: System.Data.CommandType.StoredProcedure
            );
            return rows;
        }
    }

    public async Task<int> CheckOutGuestAsync(string bookingId, string roomId)
    {
        using (var connection = GetConnection())
        {
            var rows = await connection.ExecuteAsync(
                "uspCheckOutGuest",
                new { p_booking_id = bookingId, p_room_id = roomId },
                commandType: System.Data.CommandType.StoredProcedure
            );
            return rows;
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

    public async Task<(List<object> Bookings, int TotalRows)> SearchBookingsAsync(string? keyword, DateTime? date, int pageIndex, int pageSize, string? status, DateTime? fromDate, DateTime? toDate, DateTime? searchCheckInDate, DateTime? searchCheckOutDate, int? type)
    {
        using (var connection = GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_keyword", keyword);
            parameters.Add("p_date", date);
            parameters.Add("p_pageIndex", pageIndex);
            parameters.Add("p_pageSize", pageSize);
            parameters.Add("p_status", status);
            parameters.Add("p_from_date", fromDate);
            parameters.Add("p_to_date", toDate);
            parameters.Add("p_search_check_in_date", searchCheckInDate);
            parameters.Add("p_search_check_out_date", searchCheckOutDate);
            parameters.Add("p_type", type);

            using (var multi = await connection.QueryMultipleAsync(
                "uspSearchBooking",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure))
            {
                var bookings = (await multi.ReadAsync()).Select(r => (object)r).ToList();
                int totalRows = 0;

                if (bookings.Count > 0)
                {
                    var first = bookings.First();
                    var dict = first as IDictionary<string, object>;
                    if (dict != null && dict.ContainsKey("TotalRows"))
                    {
                        totalRows = Convert.ToInt32(dict["TotalRows"]);
                    }
                }

                return (bookings, totalRows);
            }
        }
    }

    public async Task<int> BookingCheckInOutAsync(string bookingId, int type)
    {
        using (var connection = GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_booking_id", bookingId);
            parameters.Add("p_type", type);

            // Execute stored procedure; returns number of affected rows across statements
            var rows = await connection.ExecuteAsync(
                "uspBookingCheckInOut",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return rows;
        }
    }

    public async Task<Account?> GetAccountByUsernameAsync(string username)
    {
        using (var connection = GetConnection())
        {
            var account = await connection.QueryFirstOrDefaultAsync<Account>(
                "SELECT * FROM accounts WHERE username = @Username",
                new { Username = username }
            );
            return account;
        }
    }

    public async Task<string> CreateAccountAsync(Account account)
    {
        using (var connection = GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_username", account.Username);
            parameters.Add("p_password", account.Password); // assume hashed already
            parameters.Add("p_name", account.Name);
            parameters.Add("p_phone", account.Phone);
            parameters.Add("p_role", account.Role);
            parameters.Add("p_account_id", dbType: System.Data.DbType.String, size: 36, direction: System.Data.ParameterDirection.Output);

            await connection.ExecuteAsync(
                "uspCreateAccount",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return parameters.Get<string>("p_account_id");
        }
    }

    public async Task SaveRefreshTokenAsync(string accountId, string refreshToken, DateTime expires)
    {
        using (var connection = GetConnection())
        {
            var query = "UPDATE accounts SET refresh_token = @RefreshToken, refresh_token_expires = @Expires WHERE account_id = @AccountId";
            await connection.ExecuteAsync(query, new { RefreshToken = refreshToken, Expires = expires, AccountId = accountId });
        }
    }

    public async Task<Account?> GetAccountByRefreshTokenAsync(string refreshToken)
    {
        using (var connection = GetConnection())
        {
            var account = await connection.QueryFirstOrDefaultAsync<Account>(
                "SELECT * FROM accounts WHERE refresh_token = @RefreshToken",
                new { RefreshToken = refreshToken }
            );
            return account;
        }
    }
}
