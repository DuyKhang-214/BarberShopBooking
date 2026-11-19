# 06. Danh Sách Kịch Bản Kiểm Thử (Test Cases)

## 1. Test Suite: Vòng Đời Lịch Hẹn & Chuyển Trạng Thái
| Mã TC | Tên Test Case | Đầu vào | Kết quả kỳ vọng | Trạng thái |
|---|---|---|---|---|
| TC01 | Chuyển trạng thái hợp lệ | Pending -> Confirmed | Thành công, Status = Confirmed | Pass |
| TC02 | Từ chối chuyển trạng thái không hợp lệ | Completed -> Pending | Ném `InvalidOperationException` | Pass |
| TC03 | Từ chối đặt lịch trong quá khứ | Ngày hẹn = Hôm qua | Báo lỗi "Thời gian phải ở tương lai" | Pass |
| TC04 | Đặt lịch ngoài giờ làm việc | Giờ hẹn = 22:00 (lịch làm đến 19:00) | Báo lỗi "Nằm ngoài giờ làm việc" | Pass |

## 2. Test Suite: Khung Giờ Trống & Chống Trùng Lịch
| Mã TC | Tên Test Case | Đầu vào | Kết quả kỳ vọng | Trạng thái |
|---|---|---|---|---|
| TC05 | Phát hiện hai khoảng thời gian bị trùng | Đặt 09:00 - 09:30 khi đã có lịch 09:00 - 09:45 | Từ chối đặt lịch, ném ngoại lệ trùng lịch | Pass |
| TC06 | Cho phép đặt sát giờ lịch trước kết thúc | Đặt 09:30 - 10:00 khi lịch trước kết thúc lúc 09:30 | Thành công | Pass |
| TC07 | Lịch Cancelled không chiếm khung giờ | Barber A có lịch 09:00 - 09:30 bị Cancelled | Giờ 09:00 xuất hiện trong danh sách giờ trống | Pass |
| TC08 | Lưu giá đúng thời điểm đặt | Service có giá 100.000 VNĐ | `PriceAtBooking` lưu chính xác 100.000 VNĐ | Pass |

## 3. Test Suite: Hủy Lịch & Đánh Giá
| Mã TC | Tên Test Case | Đầu vào | Kết quả kỳ vọng | Trạng thái |
|---|---|---|---|---|
| TC09 | Từ chối đánh giá khi lịch chưa Completed | Lịch hẹn `Pending` | Báo lỗi "Chỉ đánh giá khi hoàn thành" | Pass |
| TC10 | Từ chối đánh giá 2 lần | Lịch hẹn đã có `BarberReview` | Báo lỗi "Lịch hẹn đã được đánh giá" | Pass |
| TC11 | Khách hàng không thể hủy lịch người khác | Customer A hủy lịch của Customer B | Ném `UnauthorizedAccessException` | Pass |
| TC12 | Khách hàng hủy lịch trước < 2 giờ | Lịch hẹn cách hiện tại 1 giờ | Báo lỗi "Phải hủy trước ít nhất 2 giờ" | Pass |
| TC13 | Bảo vệ tài khoản Admin cuối | Hạ quyền/Khóa Admin duy nhất | Báo lỗi "Không thể khóa/hạ quyền Admin duy nhất" | Pass |
