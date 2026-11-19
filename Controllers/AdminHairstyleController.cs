using BarberShopBooking.Models;
using BarberShopBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

[Authorize(Roles = "Admin")]
public class AdminHairstyleController : Controller
{
    private readonly IHairstyleService _hairstyleService;
    private readonly IWebHostEnvironment _env;

    public AdminHairstyleController(IHairstyleService hairstyleService, IWebHostEnvironment env)
    {
        _hairstyleService = hairstyleService;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var hairstyles = await _hairstyleService.GetAllHairstylesAsync();
        return View(hairstyles);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Hairstyle());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Hairstyle hairstyle, IFormFile? imageFile)
    {
        if (string.IsNullOrWhiteSpace(hairstyle.HairstyleName))
        {
            ModelState.AddModelError("HairstyleName", "Tên kiểu tóc không được để trống.");
        }

        if (!ModelState.IsValid)
        {
            return View(hairstyle);
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            hairstyle.ImageUrl = await SaveImageFileAsync(imageFile);
        }
        else
        {
            hairstyle.ImageUrl = "/images/hairstyles/default-hairstyle.jpg";
        }

        await _hairstyleService.CreateHairstyleAsync(hairstyle);
        TempData["SuccessMessage"] = "Thêm kiểu tóc mới vào bộ sưu tập thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var hairstyle = await _hairstyleService.GetByIdAsync(id);
        if (hairstyle == null) return NotFound();
        return View(hairstyle);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Hairstyle hairstyle, IFormFile? imageFile)
    {
        if (string.IsNullOrWhiteSpace(hairstyle.HairstyleName))
        {
            ModelState.AddModelError("HairstyleName", "Tên kiểu tóc không được để trống.");
        }

        if (!ModelState.IsValid)
        {
            return View(hairstyle);
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            hairstyle.ImageUrl = await SaveImageFileAsync(imageFile);
        }

        await _hairstyleService.UpdateHairstyleAsync(hairstyle);
        TempData["SuccessMessage"] = "Cập nhật thông tin kiểu tóc thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _hairstyleService.DeleteHairstyleAsync(id);
            TempData["SuccessMessage"] = "Đã xóa kiểu tóc khỏi bộ sưu tập.";
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

        // Limit size to 5MB
        if (file.Length > 5 * 1024 * 1024)
        {
            throw new InvalidOperationException("Dung lượng file ảnh không vượt quá 5MB.");
        }

        string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "hairstyles");
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

        return $"/images/hairstyles/{uniqueFileName}";
    }
}