# RoomMasterService - Hotel Management API

A comprehensive .NET Web API for hotel front-desk operations, including guest management, booking system, room management, check-in/check-out, and reporting.

## Prerequisites

- .NET 8.0 SDK or later
- MySQL Server 8.0+
- Visual Studio 2022 or VS Code

## Setup Instructions

### 1. Database Setup

1. Create the MySQL database using the schema file:
```bash
mysql -u root -p < db-room-master.sql
```

2. Create the stored procedures:
```bash
mysql -u root -p HotelDesk < sp-room-master.sql
```

### 2. Configuration

Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=HotelDesk;Uid=root;Pwd=your_password;"
}
```

### 3. Run the Application

```bash
cd RoomMasterService
dotnet restore
dotnet build
dotnet run
```

The API will be available at `https://localhost:5001` and `http://localhost:5000`

Swagger documentation will be available at `http://localhost:5000/swagger`

## API Endpoints

### Guest Management

#### Get Guest by ID
```
GET /api/guests/{guestId}
Response: { success: boolean, data: Guest }
```

#### Search Guests
```
GET /api/guests/search/{keyword}
Response: { success: boolean, data: Guest[] }
```

#### Create Guest
```
POST /api/guests
Body: {
  "fullName": "string",
  "email": "string",
  "phone": "string",
  "idType": "Passport|ID_Card|Driver_License",
  "idNumber": "string",
  "nationality": "string",
  "dateOfBirth": "date"
}
Response: { success: boolean, message: string, data: { guestId: string } }
```

#### Update Guest
```
PUT /api/guests/{guestId}
Body: {
  "fullName": "string",
  "email": "string",
  "phone": "string"
}
Response: { success: boolean, message: string }
```

### Booking Management

#### Get Booking by ID
```
GET /api/bookings/{bookingId}
Response: { success: boolean, data: Booking }
```

#### Get Guest Bookings
```
GET /api/bookings/guest/{guestId}
Response: { success: boolean, data: Booking[] }
```

#### Get Bookings by Date Range
```
POST /api/bookings/date-range
Body: {
  "startDate": "date",
  "endDate": "date"
}
Response: { success: boolean, data: Booking[] }
```

#### Create Booking
```
POST /api/bookings
Body: {
  "guestId": "string",
  "roomId": "string",
  "checkInDate": "date",
  "checkOutDate": "date",
  "specialRequests": "string"
}
Response: { success: boolean, message: string, data: { bookingId: string, bookingReference: string } }
```

#### Update Booking
```
PUT /api/bookings/{bookingId}
Body: {
  "checkInDate": "date",
  "checkOutDate": "date"
}
Response: { success: boolean, message: string }
```

#### Cancel Booking
```
POST /api/bookings/{bookingId}/cancel
Response: { success: boolean, message: string }
```

### Room Management

#### Get Room by ID
```
GET /api/rooms/{roomId}
Response: { success: boolean, data: Room }
```

#### Get Available Rooms
```
POST /api/rooms/available
Body: {
  "checkInDate": "date",
  "checkOutDate": "date",
  "roomTypeId": "string" (optional)
}
Response: { success: boolean, data: Room[] }
```

#### Create Room
```
POST /api/rooms
Body: {
  "roomNumber": "string",
  "roomTypeId": "string",
  "floor": int,
  "capacity": int,
  "status": "Available|Occupied|Cleaning|Maintenance",
  "pricePerNight": decimal,
  "features": "json"
}
Response: { success: boolean, message: string, data: { roomId: string } }
```

#### Update Room Status
```
PUT /api/rooms/{roomId}/status
Body: {
  "status": "Available|Occupied|Cleaning|Maintenance"
}
Response: { success: boolean, message: string }
```

### Check-in/Check-out

#### Check-in Guest
```
POST /api/checkinout/check-in
Body: {
  "bookingId": "string",
  "roomId": "string"
}
Response: { success: boolean, message: string }
```

#### Check-out Guest
```
POST /api/checkinout/check-out
Body: {
  "bookingId": "string",
  "roomId": "string"
}
Response: { success: boolean, message: string }
```

### Dashboard & Reporting

#### Get Occupancy Stats
```
GET /api/dashboard/occupancy-stats
Response: { success: boolean, data: { totalRooms: int, availableRooms: int, occupiedRooms: int } }
```

#### Get Revenue Report
```
POST /api/dashboard/revenue-report
Body: {
  "startDate": "date",
  "endDate": "date"
}
Response: { success: boolean, data: { totalRevenue: decimal, totalBookings: int } }
```

## Project Structure

```
RoomMasterService/
├── Controllers/          # API endpoints
│   ├── GuestsController.cs
│   ├── BookingsController.cs
│   ├── RoomsController.cs
│   ├── CheckInOutController.cs
│   └── DashboardController.cs
├── Services/            # Business logic layer
│   ├── IGuestService.cs & GuestService.cs
│   ├── IBookingService.cs & BookingService.cs
│   ├── IRoomService.cs & RoomService.cs
│   ├── ICheckInOutService.cs & CheckInOutService.cs
│   └── IDashboardService.cs & DashboardService.cs
├── Data/               # Data access layer
│   ├── IDataAccess.cs
│   └── DataAccess.cs
├── Models/             # Database models
│   ├── User.cs
│   ├── Guest.cs
│   ├── Room.cs
│   ├── RoomType.cs
│   ├── Booking.cs
│   ├── Transaction.cs
│   ├── DamageReport.cs
│   └── AuditLog.cs
├── DTOs/              # Request/Response objects
│   ├── CreateGuestRequest.cs
│   ├── UpdateGuestRequest.cs
│   ├── CreateBookingRequest.cs
│   ├── UpdateBookingRequest.cs
│   └── ApiResponse.cs
├── Program.cs         # Application startup
├── appsettings.json
└── appsettings.Development.json
```

## Key Features

✅ **Guest Management** - Create, update, and search guest profiles  
✅ **Room Management** - Track room availability and status  
✅ **Booking System** - Create, modify, and cancel bookings  
✅ **Check-in/Check-out** - Automated guest workflows  
✅ **Dashboard** - Real-time occupancy and revenue metrics  
✅ **Stored Procedures** - All data operations use SQL stored procedures  
✅ **Role-Based Access** - Support for Admin, Manager, Receptionist roles  
✅ **CORS Support** - Cross-origin requests enabled  
✅ **Swagger Documentation** - Interactive API documentation  

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **ORM**: Dapper (for stored procedure calls)
- **Database**: MySQL 8.0+
- **Documentation**: Swagger/OpenAPI
- **Logging**: Built-in ASP.NET Core logging

## Error Handling

All endpoints return standardized responses:

**Success Response:**
```json
{
  "success": true,
  "message": "Operation successful",
  "data": {}
}
```

**Error Response:**
```json
{
  "success": false,
  "message": "Error description",
  "errors": ["error1", "error2"]
}
```

## Notes

- Connection strings must be updated in `appsettings.json` before running
- All stored procedures are called via Dapper ORM
- Database operations are fully asynchronous
- CORS is enabled for development; configure appropriately for production
- Password hashing should use bcrypt (currently placeholder in sample data)

## Future Enhancements

- Authentication and authorization (JWT)
- Payment processing integration
- Email notifications
- SMS notifications
- Advanced reporting and analytics
- Multi-property support
- Mobile app API
