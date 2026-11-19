using System.ComponentModel.DataAnnotations;
using BarberShopBooking.Models;

namespace BarberShopBooking.ViewModels;

public class AvailableSlotViewModel
{
    public TimeSpan Time { get; set; }
    public string DisplayTime { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public string? Reason { get; set; }
}

public class AppointmentCreateViewModel
{
    [Required(ErrorMessage = "Vui lòng chọn dịch vụ.")]
    [Display(Name = "Dịch vụ")]
    public int ServiceId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn thợ cắt tóc (barber).")]
    [Display(Name = "Barber")]
    public int BarberId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn ngày làm việc.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày hẹn")]
    public DateTime AppointmentDate { get; set; } = DateTime.Today.AddDays(1);

    [Required(ErrorMessage = "Vui lòng chọn khung giờ trống.")]
    [Display(Name = "Giờ hẹn")]
    public TimeSpan AppointmentTime { get; set; }

    [StringLength(500, ErrorMessage = "Ghi chú không vượt quá 500 ký tự.")]
    [Display(Name = "Ghi chú")]
    public string? Note { get; set; }

    // Dropdown lists
    public List<Service> Services { get; set; } = new();
    public List<Barber> Barbers { get; set; } = new();
    public List<AvailableSlotViewModel> AvailableSlots { get; set; } = new();

    public Service? SelectedService { get; set; }
    public Barber? SelectedBarber { get; set; }
}
