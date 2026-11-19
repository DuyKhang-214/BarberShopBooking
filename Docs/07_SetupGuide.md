# 07. Hướng Dẫn Cài Đặt & Chạy Ứng Dụng (Setup Guide)

## 1. Yêu cầu môi trường
- SDK: **.NET 9 SDK** (phiên bản 9.0.100 trở lên).
- Cơ sở dữ liệu: **SQL Server** (LocalDB hoặc SQL Server Express / Standard).
- IDE khuyên dùng: Visual Studio 2022, JetBrains Rider, hoặc VS Code.

## 2. Chuỗi kết nối Database (Connection String)
Cấu hình chuỗi kết nối trong file `BarberShopBooking/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BarberShopBookingDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
```

## 3. Các lệnh thực thi chính

### Bước 1: Restore packages và Build Solution
```bash
dotnet restore
dotnet build BarberShopBooking.sln
```

### Bước 2: Chạy Unit Tests
```bash
dotnet test BarberShopBooking.Tests/BarberShopBooking.Tests.csproj
```

### Bước 3: Cập nhật Database
```bash
cd BarberShopBooking
dotnet ef database update
```

### Bước 4: Khởi chạy ứng dụng Web
```bash
dotnet run --project BarberShopBooking.csproj
```
Ứng dụng sẽ chạy tại địa chỉ: `https://localhost:7001` hoặc `http://localhost:5001`.

## 4. Tài khoản Demo mặc định (Seed Data)
| Vai trò | Tên đăng nhập | Mật khẩu | Mô tả |
|---|---|---|---|
| **Admin** | `admin` | `Admin@123` | Tài khoản quản trị toàn bộ hệ thống |
| **Customer 1** | `khachhang1` | `123456` | Khách hàng Nguyễn Văn A |
| **Customer 2** | `khachhang2` | `123456` | Khách hàng Trần Thị B |
