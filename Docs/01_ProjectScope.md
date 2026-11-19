# 01. Phạm Vi Dự Án (Project Scope) - BarberShopBooking

## 1. Giới thiệu tổng quan
Dự án **BarberShopBooking** là một ứng dụng web đặt lịch dịch vụ cắt tóc nam chuyên nghiệp được xây dựng trên nền tảng **ASP.NET Core 9 MVC** theo hướng Phân tích và Thiết kế Hướng đối tượng (**OOAD**). Hệ thống giúp tối ưu hóa quy trình đặt lịch của khách hàng và quản lý vận hành cửa hàng cho ban quản trị (Admin).

## 2. Mục tiêu hệ thống
- Cung cấp giao diện responsive mobile-first hiện đại với tông màu đen (`#1f1f1f`) - vàng (`#f5c542`) đặc trưng.
- Đảm bảo tính nhất quán nghiệp vụ đặt lịch: ngăn chặn trùng lịch thợ cắt tóc, tự động xác định khung giờ làm việc theo thời lượng dịch vụ và lịch hằng tuần của Barber.
- Áp dụng kiến trúc phân tầng rõ ràng (Entities, ViewModels, Repositories, Services, Controllers, Views) tuân thủ các nguyên lý SOLID.
- Bảo mật thông tin người dùng với ASP.NET Core Cookie Authentication và `PasswordHasher<User>`.

## 3. Phạm vi người dùng (Actors)
Hệ thống chỉ có 2 vai trò người dùng duy nhất:
1. **Customer (Khách hàng)**: Đăng ký, đăng nhập, xem thông tin dịch vụ, kiểu tóc, chọn thợ cắt tóc và khung giờ rảnh để đặt lịch, xem lịch sử đặt lịch, hủy lịch hẹn hợp lệ và gửi đánh giá sau khi hoàn thành dịch vụ.
2. **Admin (Quản trị viên)**: Xem dashboard thống kê tổng quan, quản lý danh mục Dịch vụ, quản lý danh sách Barber & Lịch làm việc tuần, bộ sưu tập Kiểu tóc, danh sách Lịch hẹn, Tài khoản người dùng, Đánh giá và Liên hệ khách hàng.

## 4. Những phạm vi KHÔNG triển khai (Out of Scope)
- Không triển khai tài khoản riêng cho Barber hoặc Staff.
- Không tích hợp thanh toán trực tuyến, SMS, Email OTP hay AI Chatbot.
- Không hỗ trợ nhiều chi nhánh cửa hàng, quản lý kho hoặc bán sản phẩm.
