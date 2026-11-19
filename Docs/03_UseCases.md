# 03. Đặt Tả Use Case (Use Case Specifications)

## Use Case 1: Đặt Lịch Hẹn (Customer)
- **Actor**: Customer
- **Tiền điều kiện**: Customer đã đăng nhập vào hệ thống.
- **Luồng chính (Main Flow)**:
  1. Customer truy cập trang `/Appointment/Create`.
  2. Chọn Dịch vụ (Service) và Barber mong muốn.
  3. Chọn Ngày hẹn (`AppointmentDate`).
  4. Hệ thống gọi AJAX API `GetAvailableSlots` để tính toán và trả về danh sách khung giờ trống (mỗi khung giờ 30 phút).
  5. Customer chọn một khung giờ khả dụng và nhập Ghi chú (nếu có).
  6. Nhấn "Xác nhận đặt lịch".
  7. Hệ thống kiểm tra ModelState, trùng lịch server-side, khởi tạo lịch hẹn với giá `PriceAtBooking = Service.Price` và trạng thái `Pending`.
  8. Hệ thống lưu lịch hẹn vào CSDL và chuyển hướng về trang `/Appointment/MyAppointments` với thông báo thành công.
- **Ngoại lệ (Exceptions)**:
  - Nếu thời gian chọn ở quá khứ hoặc nằm ngoài lịch làm việc của Barber: Hệ thống hiển thị lỗi.
  - Nếu khung giờ bị người khác đặt trước trong lúc chọn: Hệ thống thông báo trùng lịch và yêu cầu chọn lại.

## Use Case 2: Hủy Lịch Hẹn (Customer)
- **Actor**: Customer
- **Tiền điều kiện**: Customer đã đăng nhập và có lịch hẹn ở trạng thái `Pending` hoặc `Confirmed`.
- **Luồng chính**:
  1. Customer truy cập trang `/Appointment/MyAppointments`.
  2. Hệ thống hiển thị nút "Hủy lịch" đối với các lịch hẹn thỏa điều kiện:
     - Trạng thái là `Pending` hoặc `Confirmed`.
     - Thời điểm hẹn cách thời điểm hiện tại `>= 2 giờ`.
  3. Customer nhấn "Hủy lịch" và xác nhận.
  4. Hệ thống chuyển trạng thái lịch hẹn thành `Cancelled`.

## Use Case 3: Đánh Giá Barber (Customer)
- **Actor**: Customer
- **Tiền điều kiện**: Lịch hẹn của Customer đã hoàn thành (`Completed`) và chưa được đánh giá trước đó.
- **Luồng chính**:
  1. Customer nhấn nút "Đánh giá" tại lịch hẹn tương ứng trong trang Lịch sử.
  2. Hệ thống hiển thị form đánh giá với số sao (1-5) và ô nhập nhận xét.
  3. Customer chọn số sao, nhập nhận xét và gửi form.
  4. Hệ thống kiểm tra tính duy nhất (`AppointmentId` không trùng trong `BarberReview`), lưu bản ghi đánh giá và cập nhật giao diện.

## Use Case 4: Cập Nhật Trạng Thái Lịch Hẹn (Admin)
- **Actor**: Admin
- **Luồng chính**:
  1. Admin vào trang Quản lý Lịch hẹn (`/AdminAppointment`).
  2. Danh sách hiển thị các nút thao tác tương ứng với State Machine hợp lệ:
     - Pending -> Confirmed hoặc Cancelled.
     - Confirmed -> Completed hoặc Cancelled.
  3. Admin nhấn nút chuyển trạng thái.
  4. Domain Object `Appointment.ChangeStatus()` kiểm tra chuyển trạng thái hợp lệ và lưu thay đổi vào CSDL.
