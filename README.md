# Room Master Service

Ứng dụng Room Master Service là một REST API quản lý hoạt động lễ tân khách sạn (quản lý khách, phòng, đặt phòng, check-in/check-out, báo cáo) viết bằng .NET 8 và Dapper.

## Tổng quan
- Ngôn ngữ: C# (.NET 8)
- Kiến trúc: ASP.NET Core Web API
- DB: MySQL (sử dụng `MySql.Data` + `Dapper`)
- Xác thực: JWT (Bearer)
- Tài liệu API: Swagger

## Yêu cầu
- .NET 8 SDK
- MySQL Server 8+
- (Tùy chọn) Visual Studio 2022 hoặc VS Code

## Cấu hình
1. Cài đặt chuỗi kết nối MySQL
   - Mở `RoomMasterService/appsettings.Development.json` hoặc `appsettings.json` và chỉnh `ConnectionStrings:DefaultConnection`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=hoteldesk;Uid=root;Pwd=your_password;"
  }
}
```

2. Cấu hình JWT key
   - Thêm giá trị `Jwt:Key` trong `appsettings.Development.json` hoặc dùng biến môi trường `Jwt__Key`.
   - Ví dụ `appsettings.Development.json`:

```json
{
  "Jwt": {
    "Key": "<your_base64_or_secret_here>"
  }
}
```

3. Tạo key an toàn (khuyến nghị 32 byte trở lên)
   - PowerShell:

```
$bytes = New-Object byte[] 32; [System.Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes); [Convert]::ToBase64String($bytes)
```

   - Linux / macOS:

```
openssl rand -base64 32
```

   - Lưu ý: Không commit key thật lên Git. Dùng `dotnet user-secrets` cho môi trường dev hoặc biến môi trường/Key Vault cho production.

## Cài đặt & chạy
Từ thư mục gốc của repo:

```
cd RoomMasterService
dotnet restore
dotnet build
dotnet run
```

- Ứng dụng chạy ở URL theo `Properties/launchSettings.json` (ví dụ `https://localhost:63887`).
- Swagger UI có sẵn khi môi trường là `Development` tại `/swagger`.

## Endpoints chính (tóm tắt)
- Auth / Tài khoản: `POST /api/accounts/login`, `POST /api/accounts/refresh`
- Guests: `GET/POST/PUT /api/guests`
- Rooms: `GET/POST/PUT /api/rooms`, `POST /api/rooms/available`
- Bookings: `GET/POST/PUT /api/bookings`, `POST /api/bookings/{id}/cancel`
- Check-in/out: `POST /api/checkinout/check-in`, `POST /api/checkinout/check-out`


(Chi tiết payload và route đầy đủ xem trực tiếp trong controllers hoặc Swagger.)

## Cấu trúc dự án (tóm tắt)
- `Controllers/` — endpoint API
- `Services/` — logic nghiệp vụ
- `Data/` — access DB (Dapper + stored procedures)
- `Models/`, `DTOs/` — mô hình dữ liệu và DTOs
- `Program.cs` — cấu hình app, authentication, CORS, Swagger

## Ghi chú bảo mật & vận hành
- Không commit `Jwt:Key` vào kho mã nguồn công khai.
- Dùng HTTPS trong môi trường production.
- Thay đổi cấu hình CORS theo nhu cầu thực tế.

## Mẹo phát triển
- Dùng `dotnet user-secrets` để lưu `Jwt:Key` trong môi trường dev:

```
cd RoomMasterService
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "<your-secret>"
```

- Nếu gặp lỗi kiểu `Microsoft.AspNetCore.Authentication.JwtBearer` missing, chạy:

```
dotnet add RoomMasterService/RoomMasterService.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
```

## Liên hệ & đóng góp
- Mở issue hoặc pull request trên repo để báo lỗi hoặc đề xuất tính năng.

---
Cần bổ sung phần hướng dẫn chi tiết endpoint (Postman/Swagger collection) hoặc script tạo DB không?
