# 05. Thiết Kế Cơ Sở Dữ Liệu (Database Design)

## 1. Danh sách các Bảng Dữ Liệu
Hệ thống sử dụng SQL Server với các bảng chính:
1. `Users`: Lưu thông tin tài khoản (Khách hàng & Admin).
2. `Barbers`: Lưu thông tin thợ cắt tóc.
3. `Services`: Lưu danh mục dịch vụ cắt, uốn, nhuộm.
4. `BarberSchedules`: Lưu lịch làm việc tuần theo từng thứ của Barber.
5. `Appointments`: Lưu thông tin các cuộc hẹn đặt lịch.
6. `BarberReviews`: Lưu các đánh giá dịch vụ.
7. `Hairstyles`: Lưu bộ sưu tập mẫu kiểu tóc.
8. `Contacts`: Lưu lời nhắn liên hệ khách hàng và phản hồi của Admin.

## 2. Chi Tiết Các Bảng & Quan Hệ

### Bảng `Users`
- `UserId` (int, PK, Identity)
- `Username` (nvarchar(50), Unique Index)
- `Email` (nvarchar(100), Unique Index)
- `FullName` (nvarchar(100))
- `Phone` (nvarchar(20), Nullable)
- `PasswordHash` (nvarchar(max))
- `Role` (nvarchar(20)) -- "Admin" / "Customer"
- `CreatedAt` (datetime2)
- `IsActive` (bit)

### Bảng `Barbers`
- `BarberId` (int, PK, Identity)
- `FullName` (nvarchar(100))
- `Phone` (nvarchar(20), Nullable)
- `SkillDescription` (nvarchar(max), Nullable)
- `ImageUrl` (nvarchar(max), Nullable)
- `ExperienceYears` (int)
- `IsActive` (bit)
- `CreatedAt` (datetime2)

### Bảng `Services`
- `ServiceId` (int, PK, Identity)
- `ServiceName` (nvarchar(100))
- `Description` (nvarchar(max), Nullable)
- `Price` (decimal(18,2))
- `DurationMinutes` (int)
- `IsActive` (bit)
- `IsFeatured` (bit)
- `DisplayOrder` (int)
- `ImageUrl` (nvarchar(max), Nullable)

### Bảng `BarberSchedules`
- `ScheduleId` (int, PK, Identity)
- `BarberId` (int, FK -> Barbers.BarberId, Cascade)
- `DayOfWeek` (int) -- 0=Sunday, 1=Monday...
- `StartTime` (time)
- `EndTime` (time)
- `IsWorkingDay` (bit)

### Bảng `Appointments`
- `AppointmentId` (int, PK, Identity)
- `UserId` (int, FK -> Users.UserId, Restrict)
- `BarberId` (int, FK -> Barbers.BarberId, Restrict)
- `ServiceId` (int, FK -> Services.ServiceId, Restrict)
- `AppointmentDate` (date)
- `AppointmentTime` (time)
- `Status` (nvarchar(50)) -- "Chờ xác nhận", "Đã xác nhận", "Hoàn thành", "Đã hủy"
- `Note` (nvarchar(500), Nullable)
- `PriceAtBooking` (decimal(18,2))
- `CreatedAt` (datetime2)
- *Indexes*: `UserId`, `(BarberId, AppointmentDate, AppointmentTime)`, `Status`.

### Bảng `BarberReviews`
- `ReviewId` (int, PK, Identity)
- `AppointmentId` (int, Unique Index, FK -> Appointments.AppointmentId, Restrict)
- `UserId` (int, FK -> Users.UserId, Restrict)
- `BarberId` (int, FK -> Barbers.BarberId, Restrict)
- `Rating` (int) -- 1..5
- `Comment` (nvarchar(500), Nullable)
- `CreatedAt` (datetime2)

### Bảng `Hairstyles`
- `HairstyleId` (int, PK, Identity)
- `HairstyleName` (nvarchar(100))
- `Description` (nvarchar(max))
- `ImageUrl` (nvarchar(max))
- `Category` (nvarchar(50))
- `IsActive` (bit)
- `CreatedAt` (datetime2)

### Bảng `Contacts`
- `ContactId` (int, PK, Identity)
- `UserId` (int, Nullable, FK -> Users.UserId, SetNull)
- `FullName` (nvarchar(100))
- `Email` (nvarchar(100))
- `Phone` (nvarchar(20), Nullable)
- `Message` (nvarchar(max))
- `ReplyMessage` (nvarchar(max), Nullable)
- `CreatedAt` (datetime2)
- `RepliedAt` (datetime2, Nullable)

## 3. Các Quan Hệ Bắt Buộc
- User 1 — N Appointment (DeleteBehavior.Restrict)
- Barber 1 — N Appointment (DeleteBehavior.Restrict)
- Service 1 — N Appointment (DeleteBehavior.Restrict)
- Appointment 1 — 0..1 BarberReview (Unique Index `AppointmentId`)
- User 1 — N BarberReview (DeleteBehavior.Restrict)
- Barber 1 — N BarberReview (DeleteBehavior.Restrict)
- Barber 1 — N BarberSchedule (DeleteBehavior.Cascade)
