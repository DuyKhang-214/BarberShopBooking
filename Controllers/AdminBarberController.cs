using BarberShopBooking.Models;
using BarberShopBooking.Services;
using BarberShopBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

/// <summary>
/// Controller Quản Lý Thợ Cắt Tóc Dành Cho Admin (AdminBarberController).
/// Actor chính: Quản Trị Viên (Admin).
/// Chức năng: Xem danh sách Barber, Thêm thợ mới, Sửa thông tin thợ, Cấu hình lịch làm việc tuần.
/// </summary>
[Authorize(Roles = "Admin")]
public class AdminBarberController : Controller
{
    private readonly IBarberService _barberService;
    private readonly IBarberScheduleService _scheduleService;
    private readonly IWebHostEnvironment _env;

    public AdminBarberController(
        IBarberService barberService,
        IBarberScheduleService scheduleService,
        IWebHostEnvironment env)
    {
        _barberService = barberService;
        _scheduleService = scheduleService;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var barbers = await _barberService.GetAllBarbersAsync();
        return View(barbers);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Barber());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Barber barber, IFormFile? imageFile)
    {
        if (string.IsNullOrWhiteSpace(barber.FullName))
        {
            ModelState.AddModelError("FullName", "Vui lòng nhập họ tên Barber.");
        }

        if (!ModelState.IsValid)
        {
            return View(barber);
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            barber.ImageUrl = await SaveImageFileAsync(imageFile);
        }
        else
        {
            barber.ImageUrl = "/images/barbers/default-barber.jpg";
        }

        await _barberService.CreateBarberAsync(barber);
        TempData["SuccessMessage"] = "Thêm thợ cắt tóc mới thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var barber = await _barberService.GetByIdAsync(id);
        if (barber == null) return NotFound();
        return View(barber);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Barber barber, IFormFile? imageFile)
    {
        if (string.IsNullOrWhiteSpace(barber.FullName))
        {
            ModelState.AddModelError("FullName", "Vui lòng nhập họ tên Barber.");
        }

        if (!ModelState.IsValid)
        {
            return View(barber);
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            barber.ImageUrl = await SaveImageFileAsync(imageFile);
        }

        await _barberService.UpdateBarberAsync(barber);
        TempData["SuccessMessage"] = "Cập nhật thông tin Barber thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _barberService.DeleteBarberAsync(id);
            TempData["SuccessMessage"] = "Đã xử lý xóa/ngừng hoạt động Barber.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Schedule(int id)
    {
        try
        {
            var model = await _scheduleService.GetScheduleForManageAsync(id);
            return View(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Schedule(BarberScheduleManageViewModel model)
    {
        try
        {
            await _scheduleService.UpdateSchedulesAsync(model);
            TempData["SuccessMessage"] = "Cập nhật lịch làm việc tuần thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    private async Task<string> SaveImageFileAsync(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(ext))
        {
            throw new InvalidOperationException("Chỉ hỗ trợ định dạng ảnh (.jpg, .jpeg, .png, .webp).");
        }

        string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "barbers");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        string uniqueFileName = Guid.NewGuid().ToString("N") + ext;
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return $"/images/barbers/{uniqueFileName}";
    }
}