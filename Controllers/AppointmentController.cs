using System.Security.Claims;
using BarberShopBooking.Services;
using BarberShopBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

/// <summary>
/// Controller Quản Lý Đặt Lịch Hẹn (AppointmentController).
/// Actor chính: Khách hàng (Customer) đăng nhập hệ thống để thực hiện:
/// - Chọn Dịch vụ, Barber và Khung giờ khả dụng (Engine AJAX).
/// - Xác nhận Đặt lịch (Kiểm tra chống trùng lịch & tính hợp lệ).
/// - Xem Danh sách Lịch hẹn đã đặt & Hủy lịch hẹn (Quy tắc trước 2 giờ).
/// </summary>
[Authorize]
public class AppointmentController : Controller
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>Trang hiển thị Form Đặt Lịch Hẹn</summary>
    [HttpGet]
    public async Task<IActionResult> Create(int? serviceId, int? barberId)
    {
        var services = await _appointmentService.GetActiveServicesAsync();
        var barbers = await _appointmentService.GetActiveBarbersAsync();

        var model = new AppointmentCreateViewModel
        {
            ServiceId = serviceId ?? (services.FirstOrDefault()?.ServiceId ?? 0),
            BarberId = barberId ?? (barbers.FirstOrDefault()?.BarberId ?? 0),
            AppointmentDate = DateTime.Today.AddDays(1),
            Services = services,
            Barbers = barbers
        };

        if (model.ServiceId > 0 && model.BarberId > 0)
        {
            model.AvailableSlots = await _appointmentService.GetAvailableSlotsAsync(
                model.BarberId, model.ServiceId, model.AppointmentDate);
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AppointmentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return View(model);
        }

        try
        {
            int userId = GetCurrentUserId();
            if (userId <= 0)
            {
                TempData["ErrorMessage"] = "Phiên đăng nhập không hợp lệ. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }

            await _appointmentService.CreateAppointmentAsync(userId, model);
            TempData["SuccessMessage"] = "Đặt lịch hẹn thành công! Vui lòng chờ nhân viên xác nhận.";
            return RedirectToAction(nameof(MyAppointments));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateDropdownsAsync(model);
            return View(model);
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAvailableSlots(int barberId, int serviceId, string date)
    {
        if (barberId <= 0 || serviceId <= 0 || !DateTime.TryParse(date, out DateTime parsedDate))
        {
            return Json(new List<object>());
        }

        var slots = await _appointmentService.GetAvailableSlotsAsync(barberId, serviceId, parsedDate);
        return Json(slots.Select(s => new
        {
            time = s.Time.ToString(@"hh\:mm"),
            displayTime = s.DisplayTime,
            isAvailable = s.IsAvailable,
            reason = s.Reason
        }));
    }

    [HttpGet]
    public async Task<IActionResult> MyAppointments()
    {
        int userId = GetCurrentUserId();
        var appointments = await _appointmentService.GetMyAppointmentsAsync(userId);
        return View(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            int userId = GetCurrentUserId();
            await _appointmentService.CancelByCustomerAsync(userId, id);
            TempData["SuccessMessage"] = "Hủy lịch hẹn thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(MyAppointments));
    }

    private async Task PopulateDropdownsAsync(AppointmentCreateViewModel model)
    {
        model.Services = await _appointmentService.GetActiveServicesAsync();
        model.Barbers = await _appointmentService.GetActiveBarbersAsync();
        if (model.ServiceId > 0 && model.BarberId > 0 && model.AppointmentDate >= DateTime.Today)
        {
            model.AvailableSlots = await _appointmentService.GetAvailableSlotsAsync(
                model.BarberId, model.ServiceId, model.AppointmentDate);
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userIdClaim, out int id) ? id : 0;
    }
}
