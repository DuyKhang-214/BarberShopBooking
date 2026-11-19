# 04. Quy Tắc Nghiệp Vụ (Business Rules)

## 1. Vòng đời Lịch Hẹn (Appointment State Machine)
Các trạng thái hợp lệ của `AppointmentStatus`:
- `Pending` (Chờ xác nhận): Trạng thái mặc định khi khởi tạo.
- `Confirmed` (Đã xác nhận): Admin đã xác nhận lịch hẹn.
- `Completed` (Hoàn thành): Lịch hẹn đã thực hiện xong. Trạng thái kết thúc (Terminal State).
- `Cancelled` (Đã hủy): Lịch hẹn bị hủy bởi khách hàng hoặc Admin. Trạng thái kết thúc (Terminal State).

### Quy tắc chuyển trạng thái:
- `Pending` -> `Confirmed` hoặc `Cancelled`
- `Confirmed` -> `Completed` hoặc `Cancelled`
- `Completed` -> Không được chuyển sang bất kỳ trạng thái nào khác.
- `Cancelled` -> Không được chuyển sang bất kỳ trạng thái nào khác.

## 2. Quy tắc Đặt Lịch & Kiểm Tra Trùng Lịch
1. **Yêu cầu đăng nhập**: Khách hàng phải đăng nhập tài khoản trước khi tạo lịch.
2. **Kiểm tra thời gian tương lai**: `AppointmentDate + AppointmentTime > DateTime.Now`.
3. **Lịch làm việc của Barber**: Thời gian đặt phải nằm trong khung giờ `StartTime` đến `EndTime` của `BarberSchedule` tương ứng với thứ trong tuần.
4. **Thời lượng dịch vụ (DurationMinutes)**:
   - `EndTime = AppointmentTime + Service.DurationMinutes`.
5. **Công thức kiểm tra trùng lịch (Overlap Check)**:
   Không cho phép đặt lịch mới trùng khoảng thời gian với các lịch hẹn ở trạng thái `Pending` hoặc `Confirmed` của cùng một Barber:
   $$\text{newStart} < \text{existingEnd} \quad \land \quad \text{newEnd} > \text{existingStart}$$
6. **Lịch bị hủy (`Cancelled`)**: Không làm chiếm khung giờ của Barber.
7. **Bảo lưu giá (`PriceAtBooking`)**: Lưu giá thực tế tại thời điểm đặt lịch. Không chấp nhận giá gửi từ phía client trình duyệt.

## 3. Quy tắc Hủy Lịch của Khách Hàng
- Khách hàng chỉ có quyền hủy lịch do chính mình tạo ra (`UserId == currentUserId`).
- Chỉ cho phép hủy lịch ở trạng thái `Pending` hoặc `Confirmed`.
- Phải đảm bảo khoảng cách thời gian từ thời điểm hiện tại đến thời điểm hẹn còn **ít nhất 2 giờ** (`AppointmentDateTime - DateTime.Now >= 2 hours`).

## 4. Quy tắc Đánh Giá
- Chỉ khách hàng sở hữu lịch hẹn mới được gửi đánh giá.
- Lịch hẹn phải ở trạng thái `Completed`.
- Mỗi lịch hẹn chỉ được đánh giá tối đa **1 lần** (ràng buộc bởi Index Unique trên cột `AppointmentId` trong bảng `BarberReview`).
- Điểm đánh giá `Rating` phải nằm trong khoảng từ `1` đến `5`.

## 5. Quy tắc Bảo Mật & Quản Lý Tài Khoản
- **Mật khẩu**: Không lưu hoặc so sánh văn bản thường. Mã hóa bằng `PasswordHasher<User>`.
- **Phân quyền**: Sử dụng Cookie Authentication với Claim `Role`.
- **Chống hạ quyền Admin cuối cùng**: Hệ thống kiểm tra số lượng tài khoản Admin còn hoạt động trước khi cho phép hạ quyền hoặc khóa tài khoản Admin. Không bao giờ cho phép vô tình khóa/hạ quyền Admin duy nhất.
