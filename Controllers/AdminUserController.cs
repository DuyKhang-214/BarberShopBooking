using BarberShopBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

[Authorize(Roles = "Admin")]
public class AdminUserController : Controller
{
    private readonly IAuthService _authService;

    public AdminUserController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _authService.GetAllUsersAsync();
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        try
        {
            await _authService.ToggleUserActiveStatusAsync(id);
            TempData["SuccessMessage"] = "Cập nhật trạng thái tài khoản thành công!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeRole(int id, string role)
    {
        try
        {
            await _authService.ChangeUserRoleAsync(id, role);
            TempData["SuccessMessage"] = "Cập nhật vai trò người dùng thành công!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}