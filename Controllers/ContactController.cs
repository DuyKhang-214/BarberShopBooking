using System.Security.Claims;
using BarberShopBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

public class ContactController : Controller
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            int userId = GetCurrentUserId();
            var myContacts = await _contactService.GetMyContactsAsync(userId);
            ViewBag.MyContacts = myContacts;
        }

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Index(string fullName, string email, string? phone, string message)
    {
        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
        {
            ModelState.AddModelError(string.Empty, "Vui lòng nhập đầy đủ thông tin bắt buộc.");
            if (User.Identity?.IsAuthenticated == true)
            {
                ViewBag.MyContacts = await _contactService.GetMyContactsAsync(GetCurrentUserId());
            }
            return View();
        }

        int? userId = User.Identity?.IsAuthenticated == true ? GetCurrentUserId() : null;
        await _contactService.SubmitContactAsync(userId, fullName, email, phone, message);

        TempData["SuccessMessage"] = "Gửi lời nhắn liên hệ thành công! Ban quản trị sẽ phản hồi sớm nhất.";
        return RedirectToAction(nameof(Index));
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userIdClaim, out int id) ? id : 0;
    }
}