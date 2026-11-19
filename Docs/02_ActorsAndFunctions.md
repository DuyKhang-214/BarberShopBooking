# 02. Phân Tích Actor Về Chức Năng (Actors & Functions)

## 1. Actor: Customer (Khách hàng)
| STT | Chức năng | Mô tả chi tiết |
|---|---|---|
| 1 | Xem trang chủ | Xem dịch vụ nổi bật, đội ngũ barber, đánh giá thực tế |
| 2 | Xem danh sách dịch vụ | Xem tên, mô tả, giá tiền và thời lượng thực hiện |
| 3 | Xem bộ sưu tập kiểu tóc | Tra cứu danh mục kiểu tóc đẹp (Sidepart, Undercut, Crop...) |
| 4 | Xem đánh giá công khai | Xem danh sách đánh giá từ các khách hàng khác |
| 5 | Đăng ký tài khoản | Nhập thông tin đăng ký (Username, FullName, Email, Password...) |
| 6 | Đăng nhập / Đăng xuất | Đăng nhập bằng Cookie Auth / Đăng xuất an toàn bằng POST |
| 7 | Xem & Cập nhật hồ sơ | Cập nhật họ tên, email, số điện thoại |
| 8 | Đổi mật khẩu | Đổi mật khẩu với kiểm tra mật khẩu hiện tại |
| 9 | Tìm khung giờ trống | Chọn Barber, Dịch vụ và Ngày -> AJAX tải danh sách giờ trống |
| 10 | Đặt lịch hẹn | Đặt lịch với giá cố định `PriceAtBooking` tại thời điểm đặt |
| 11 | Xem lịch sử đặt lịch | Xem danh sách lịch hẹn (Pending, Confirmed, Completed, Cancelled) |
| 12 | Hủy lịch hẹn | Hủy lịch Pending/Confirmed nếu còn trước lịch hẹn ít nhất 2 giờ |
| 13 | Đánh giá Barber | Gửi đánh giá 1-5 sao và nhận xét cho lịch hẹn Completed (tối đa 1 lần) |
| 14 | Gửi liên hệ | Gửi tin nhắn liên hệ cho ban quản trị và xem phản hồi |

## 2. Actor: Admin (Quản trị viên)
| STT | Chức năng | Mô tả chi tiết |
|---|---|---|
| 1 | Xem Dashboard thống kê | Xem tổng doanh thu, số lịch hẹn, tổng số khách hàng, đánh giá |
| 2 | Quản lý Dịch vụ | Thêm mới, chỉnh sửa, xóa/ngưng hoạt động dịch vụ |
| 3 | Quản lý Barber | Thêm mới, cập nhật thông tin, xóa/ngưng hoạt động thợ cắt tóc |
| 4 | Quản lý Lịch tuần Barber | Thiết lập giờ bắt đầu, kết thúc và ngày nghỉ hằng tuần của Barber |
| 5 | Quản lý Kiểu tóc | Đăng ảnh mẫu kiểu tóc mới, phân loại danh mục, ẩn/hiện |
| 6 | Quản lý Lịch hẹn | Xem toàn bộ lịch hẹn, duyệt Pending->Confirmed, Completed, Cancelled |
| 7 | Quản lý Người dùng | Phân quyền Admin/Customer, khóa/kích hoạt tài khoản (chống hạ quyền Admin cuối) |
| 8 | Quản lý Đánh giá | Xem tất cả đánh giá công khai và xóa đánh giá vi phạm |
| 9 | Quản lý Liên hệ | Xem tin nhắn liên hệ của khách và gửi lời phản hồi |
