using System.Security.Claims;
using BarberShopBooking.Services;
using BarberShopBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

public class ReviewController : Controller
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> PublicReviews()
    {
        var reviews = await _reviewService.GetPublicReviewsAsync();
        return View(reviews);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Create(int id)
    {
        try
        {
            int userId = GetCurrentUserId();
            var model = await _reviewService.PrepareReviewViewModelAsync(userId, id);
            return View(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("MyAppointments", "Appointment");
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(ReviewCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            int userId = GetCurrentUserId();
            await _reviewService.CreateReviewAsync(userId, model);
            TempData["SuccessMessage"] = "Cảm ơn bạn đã gửi đánh giá cho dịch vụ!";
            return RedirectToAction("MyAppointments", "Appointment");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userIdClaim, out int id) ? id : 0;
    }
}