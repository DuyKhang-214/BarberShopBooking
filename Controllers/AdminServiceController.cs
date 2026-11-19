using BarberShopBooking.Models;
using BarberShopBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

[Authorize(Roles = "Admin")]
public class AdminServiceController : Controller
{
    private readonly IServiceService _serviceService;
    private readonly IWebHostEnvironment _env;

    public AdminServiceController(IServiceService serviceService, IWebHostEnvironment env)
    {
        _serviceService = serviceService;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var services = await _serviceService.GetAllServicesAsync();
        return View(services);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Service());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Service service, IFormFile? imageFile)
    {
        if (string.IsNullOrWhiteSpace(service.ServiceName))
        {
            ModelState.AddModelError("ServiceName", "Tên dịch vụ không được để trống.");
        }

        if (service.Price <= 0)
        {
            ModelState.AddModelError("Price", "Giá dịch vụ phải lớn hơn 0.");
        }

        if (service.DurationMinutes <= 0)
        {
            ModelState.AddModelError("DurationMinutes", "Thời lượng thực hiện phải lớn hơn 0 phút.");
        }

        if (!ModelState.IsValid)
        {
            return View(service);
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            service.ImageUrl = await SaveImageFileAsync(imageFile);
        }
        else
        {
            service.ImageUrl = "/images/services/default-service.jpg";
        }

        await _serviceService.CreateServiceAsync(service);
        TempData["SuccessMessage"] = "Thêm dịch vụ mới thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var service = await _serviceService.GetByIdAsync(id);
        if (service == null) return NotFound();
        return View(service);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Service service, IFormFile? imageFile)
    {
        if (string.IsNullOrWhiteSpace(service.ServiceName))
        {
            ModelState.AddModelError("ServiceName", "Tên dịch vụ không được để trống.");
        }

        if (service.Price <= 0)
        {
            ModelState.AddModelError("Price", "Giá dịch vụ phải lớn hơn 0.");
        }

        if (service.DurationMinutes <= 0)
        {
            ModelState.AddModelError("DurationMinutes", "Thời lượng thực hiện phải lớn hơn 0 phút.");
        }

        if (!ModelState.IsValid)
        {
            return View(service);
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            service.ImageUrl = await SaveImageFileAsync(imageFile);
        }

        await _serviceService.UpdateServiceAsync(service);
        TempData["SuccessMessage"] = "Cập nhật dịch vụ thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _serviceService.DeleteServiceAsync(id);
            TempData["SuccessMessage"] = "Đã xử lý xóa/chuyển trạng thái ngưng hoạt động cho dịch vụ.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SaveImageFileAsync(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(ext))
        {
            throw new InvalidOperationException("Chỉ hỗ trợ định dạng ảnh (.jpg, .jpeg, .png, .webp).");
        }

        string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "services");
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

        return $"/images/services/{uniqueFileName}";
    }
}