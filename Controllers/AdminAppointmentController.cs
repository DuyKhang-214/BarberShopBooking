using BarberShopBooking.Models;
using BarberShopBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

/// <summary>
/// Controller Quản Lý Lịch Hẹn Dành Cho Admin (AdminAppointmentController).
/// Actor chính: Quản Trị Viên (Admin).
/// Chức năng: Xem toàn bộ lịch hẹn, duyệt lịch, đổi trạng thái (Xác nhận, Hoàn thành, Hủy).
/// </summary>
[Authorize(Roles = "Admin")]
public class AdminAppointmentController : Controller
{
    private readonly IAppointmentService _appointmentService;

    public AdminAppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>
    /// [GET] Danh sách toàn bộ lịch hẹn hệ thống.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var appointments = await _appointmentService.GetAllForAdminAsync();
        return View(appointments);
    }

    /// <summary>
    /// [POST] Đổi trạng thái lịch hẹn (Pending -> Confirmed -> Completed / Cancelled).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(int id, AppointmentStatus status)
    {
        try
        {
            await _appointmentService.ChangeStatusAsync(id, status);
            TempData["SuccessMessage"] = $"Cập nhật trạng thái lịch hẹn thành '{status.ToDisplayName()}' thành công!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
