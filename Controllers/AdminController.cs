using BarberShopBooking.Models;
using BarberShopBooking.Services;
using BarberShopBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

/// <summary>
/// Controller Bảng Điều Khiển Quản Trị (AdminController).
/// Actor chính: Quản trị viên (Admin).
/// Yêu cầu quyền: [Authorize(Roles = "Admin")]
/// Quyền hạn: Xem tổng quan hệ thống, doanh thu, danh sách lịch hẹn gần đây và thống kê toàn diện.
/// </summary>
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAppointmentService _appointmentService;
    private readonly IAuthService _authService;
    private readonly IServiceService _serviceService;
    private readonly IBarberService _barberService;
    private readonly IReviewService _reviewService;
    private readonly IContactService _contactService;

    public AdminController(
        IAppointmentService appointmentService,
        IAuthService authService,
        IServiceService serviceService,
        IBarberService barberService,
        IReviewService reviewService,
        IContactService contactService)
    {
        _appointmentService = appointmentService;
        _authService = authService;
        _serviceService = serviceService;
        _barberService = barberService;
        _reviewService = reviewService;
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var appointments = await _appointmentService.GetAllForAdminAsync();
        var users = await _authService.GetAllUsersAsync();
        var services = await _serviceService.GetAllServicesAsync();
        var barbers = await _barberService.GetAllBarbersAsync();
        var reviews = await _reviewService.GetPublicReviewsAsync();
        var contacts = await _contactService.GetAllContactsAsync();

        var completedAppointments = appointments.Where(a => a.Status == AppointmentStatus.Completed).ToList();

        var model = new DashboardViewModel
        {
            TotalUsers = users.Count(u => u.Role == "Customer"),
            TotalAppointments = appointments.Count,
            PendingAppointments = appointments.Count(a => a.Status == AppointmentStatus.Pending),
            ConfirmedAppointments = appointments.Count(a => a.Status == AppointmentStatus.Confirmed),
            CompletedAppointments = completedAppointments.Count,
            CancelledAppointments = appointments.Count(a => a.Status == AppointmentStatus.Cancelled),
            TotalRevenue = completedAppointments.Sum(a => a.PriceAtBooking),
            TotalServices = services.Count,
            TotalBarbers = barbers.Count,
            TotalReviews = reviews.Count,
            PendingContacts = contacts.Count(c => string.IsNullOrEmpty(c.ReplyMessage)),
            RecentAppointments = appointments.Take(8).ToList()
        };

        return View(model);
    }
}