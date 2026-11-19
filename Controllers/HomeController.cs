using System.Diagnostics;
using BarberShopBooking.Models;
using BarberShopBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IServiceService _serviceService;
    private readonly IBarberService _barberService;
    private readonly IReviewService _reviewService;

    public HomeController(
        IServiceService serviceService,
        IBarberService barberService,
        IReviewService reviewService)
    {
        _serviceService = serviceService;
        _barberService = barberService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var activeServices = (await _serviceService.GetActiveServicesAsync()).ToList();
        ViewBag.FeaturedServices = activeServices.Take(3).ToList();
        ViewBag.Barbers = (await _barberService.GetActiveBarbersAsync()).Take(3).ToList();
        ViewBag.RecentReviews = (await _reviewService.GetPublicReviewsAsync()).Take(4).ToList();

        return View(activeServices);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}