using System.ComponentModel.DataAnnotations;

namespace BarberShopBooking.Models;

/// <summary>
/// Lưu lịch làm việc hằng tuần cho từng barber theo thứ trong tuần.
/// </summary>
public class BarberSchedule
{
    [Key]
    public int ScheduleId { get; set; }

    public int BarberId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeSpan StartTime { get; set; } = new TimeSpan(8, 0, 0); // 08:00 AM default

    public TimeSpan EndTime { get; set; } = new TimeSpan(18, 0, 0); // 06:00 PM default

    public bool IsWorkingDay { get; set; } = true;

    public Barber? Barber { get; set; }
}
