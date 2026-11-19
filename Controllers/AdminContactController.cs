using BarberShopBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

[Authorize(Roles = "Admin")]
public class AdminContactController : Controller
{
    private readonly IContactService _contactService;

    public AdminContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var contacts = await _contactService.GetAllContactsAsync();
        return View(contacts);
    }

    [HttpPost]
    public async Task<IActionResult> Reply(int id, string replyMessage)
    {
        if (string.IsNullOrWhiteSpace(replyMessage))
        {
            TempData["ErrorMessage"] = "Nội dung phản hồi không được để trống.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            await _contactService.ReplyContactAsync(id, replyMessage);
            TempData["SuccessMessage"] = "Đã gửi nội dung phản hồi cho khách hàng!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}