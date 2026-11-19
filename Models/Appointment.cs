namespace BarberShopBooking.Models;

/// <summary>
/// Thực thể Lịch Hẹn (Appointment) - Quản lý thông tin đặt lịch cắt tóc của Khách hàng.
/// Các Actor liên quan: Khách hàng (User), Thợ cắt tóc (Barber), Quản trị viên (Admin).
/// </summary>
public class Appointment
{
    public int AppointmentId { get; set; }

    // ID Khách hàng thực hiện đặt lịch (Actor: Customer)
    public int UserId { get; set; }

    // ID Barber được chọn phục vụ (Actor: Barber)
    public int BarberId { get; set; }

    // ID Dịch vụ cắt / uốn / nhuộm được chọn
    public int ServiceId { get; set; }

    // Ngày hẹn cắt tóc
    public DateTime AppointmentDate { get; set; }

    // Khung giờ hẹn (vd: 09:00, 10:30)
    public TimeSpan AppointmentTime { get; set; }

    // Trạng thái lịch hẹn (Pending -> Confirmed -> Completed / Cancelled)
    public AppointmentStatus Status { get; private set; } = AppointmentStatus.Pending;

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public decimal PriceAtBooking { get; set; }

    public User? User { get; set; }

    public Barber? Barber { get; set; }

    public Service? Service { get; set; }
    
    public BarberReview? BarberReview { get; set; }

    public TimeSpan GetEndTime(int durationMinutes)
    {
        return AppointmentTime.Add(TimeSpan.FromMinutes(durationMinutes));
    }

    public DateTime GetStartDateTime()
    {
        return AppointmentDate.Date + AppointmentTime;
    }

    public DateTime GetEndDateTime(int durationMinutes)
    {
        return GetStartDateTime().AddMinutes(durationMinutes);
    }

    // State machine logic
    private static readonly Dictionary<AppointmentStatus, AppointmentStatus[]> AllowedTransitions = new()
    {
        [AppointmentStatus.Pending] = new[] { AppointmentStatus.Confirmed, AppointmentStatus.Cancelled },
        [AppointmentStatus.Confirmed] = new[] { AppointmentStatus.Completed, AppointmentStatus.Cancelled },
        [AppointmentStatus.Completed] = Array.Empty<AppointmentStatus>(),
        [AppointmentStatus.Cancelled] = Array.Empty<AppointmentStatus>()
    };

    public bool CanTransitionTo(AppointmentStatus target) =>
        AllowedTransitions.TryGetValue(Status, out var next) && next.Contains(target);

    /// <summary>Chuyển sang trạng thái mới nếu hợp lệ; ném lỗi nếu không.</summary>
    public void ChangeStatus(AppointmentStatus target)
    {
        if (!CanTransitionTo(target))
        {
            throw new InvalidOperationException(
                $"Không thể chuyển lịch hẹn từ '{Status.ToDisplayName()}' sang '{target.ToDisplayName()}'.");
        }

        Status = target;
    }

    public void Confirm() => ChangeStatus(AppointmentStatus.Confirmed);

    public void Cancel() => ChangeStatus(AppointmentStatus.Cancelled);

    public void Complete() => ChangeStatus(AppointmentStatus.Completed);
}