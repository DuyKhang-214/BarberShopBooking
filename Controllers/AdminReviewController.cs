using BarberShopBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

[Authorize(Roles = "Admin")]
public class AdminReviewController : Controller
{
    private readonly IReviewService _reviewService;

    public AdminReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var reviews = await _reviewService.GetPublicReviewsAsync();
        return View(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _reviewService.DeleteReviewAsync(id);
            TempData["SuccessMessage"] = "Đã xóa đánh giá thành công!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}