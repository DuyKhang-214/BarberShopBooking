using System.ComponentModel.DataAnnotations;
using BarberShopBooking.Models;

namespace BarberShopBooking.ViewModels;

public class ReviewCreateViewModel
{
    public int AppointmentId { get; set; }
    public int BarberId { get; set; }
    public string BarberName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn số sao đánh giá (1-5).")]
    [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao.")]
    [Display(Name = "Đánh giá (1-5 sao)")]
    public int Rating { get; set; } = 5;

    [StringLength(500, ErrorMessage = "Nhận xét không quá 500 ký tự.")]
    [Display(Name = "Nhận xét của bạn")]
    public string? Comment { get; set; }
}

public class BarberScheduleItemViewModel
{
    public DayOfWeek DayOfWeek { get; set; }
    public string DayOfWeekName { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsWorkingDay { get; set; }
}

public class BarberScheduleManageViewModel
{
    public int BarberId { get; set; }
    public string BarberName { get; set; } = string.Empty;
    public List<BarberScheduleItemViewModel> Schedules { get; set; } = new();
}
