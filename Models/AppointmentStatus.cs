namespace BarberShopBooking.Models;

/// <summary>
/// Trạng thái vòng đời của một lịch hẹn (Appointment).
/// Thay cho việc dùng chuỗi tự do trước đây — giúp kiểm soát được
/// các giá trị hợp lệ và các bước chuyển trạng thái (state machine).
/// </summary>
public enum AppointmentStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled
}

/// <summary>
/// Chuyển đổi qua lại giữa AppointmentStatus và nhãn tiếng Việt
/// đang hiển thị trên giao diện, để không phải đổi text trên View.
/// </summary>
public static class AppointmentStatusExtensions
{
    public static string ToDisplayName(this AppointmentStatus status) => status switch
    {
        AppointmentStatus.Pending => "Chờ xác nhận",
        AppointmentStatus.Confirmed => "Đã xác nhận",
        AppointmentStatus.Completed => "Hoàn thành",
        AppointmentStatus.Cancelled => "Đã hủy",
        _ => status.ToString()
    };

    /// <summary>Tên class CSS dùng cho badge trạng thái (viết thường).</summary>
    public static string ToCssClass(this AppointmentStatus status) => status.ToString().ToLowerInvariant();

    public static AppointmentStatus ParseDisplayName(string? value) => value switch
    {
        "Đã xác nhận" => AppointmentStatus.Confirmed,
        "Hoàn thành" => AppointmentStatus.Completed,
        "Đã hủy" => AppointmentStatus.Cancelled,
        _ => AppointmentStatus.Pending
    };
}
