using BarberShopBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

[AllowAnonymous]
public class HairstyleController : Controller
{
    private readonly IHairstyleService _hairstyleService;

    public HairstyleController(IHairstyleService hairstyleService)
    {
        _hairstyleService = hairstyleService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? category, int page = 1)
    {
        ViewBag.SelectedCategory = category;
        ViewBag.CurrentPage = page;

        var hairstyles = await _hairstyleService.GetActiveHairstylesAsync(category);
        return View(hairstyles);
    }
}