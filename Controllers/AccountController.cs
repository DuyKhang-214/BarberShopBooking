using System.Security.Claims;
using BarberShopBooking.Services;
using BarberShopBooking.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopBooking.Controllers;

/// <summary>
/// Controller xử lý Tài khoản người dùng (Đăng ký, Đăng nhập, Đăng xuất, Đổi mật khẩu).
/// Dành cho cả Tác nhân: Khách Hàng (Customer) và Quản Trị Viên (Admin).
/// </summary>
public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IPasswordResetService _passwordResetService;
    private readonly IEmailSender _emailSender;
    private readonly IWebHostEnvironment _env;

    public AccountController(
        IAuthService authService,
        IPasswordResetService passwordResetService,
        IEmailSender emailSender,
        IWebHostEnvironment env)
    {
        _authService = authService;
        _passwordResetService = passwordResetService;
        _emailSender = emailSender;
        _env = env;
    }

    /// <summary>
    /// [GET] Hiển thị trang Đăng ký tài khoản Khách hàng mới.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(new RegisterViewModel());
    }

    /// <summary>
    /// [POST] Xử lý Đăng ký tài khoản mới cho Khách hàng.
    /// Tác nhân: Khách Hàng (Customer).
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var user = await _authService.RegisterAsync(model);
            await SignInUserAsync(user);

            TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Cảm ơn bạn đã gia nhập BarberShop.";
            return RedirectToAction("Index", "Home");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    /// <summary>
    /// [GET] Hiển thị trang Đăng nhập hệ thống.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    /// <summary>
    /// [POST] Xử lý xác thực Đăng nhập (Phân quyền chuyển hướng Admin / Customer).
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _authService.AuthenticateAsync(model.Username, model.Password);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
            return View(model);
        }

        await SignInUserAsync(user);

        TempData["SuccessMessage"] = $"Xin chào quay trở lại, {user.FullName}!";

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        if (user.Role == "Admin")
        {
            return RedirectToAction("Index", "Admin");
        }

        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// [POST] Đăng xuất tài khoản khỏi Session / Cookie Authentication.
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["SuccessMessage"] = "Đăng xuất tài khoản thành công.";
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// [GET] Xem trang Thông tin cá nhân (Profile).
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        int userId = GetCurrentUserId();
        var user = await _authService.GetByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var model = new EditProfileViewModel
        {
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone
        };

        return View(model);
    }

    /// <summary>
    /// [POST] Cập nhật thông tin cá nhân.
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Profile(EditProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            int userId = GetCurrentUserId();
            await _authService.UpdateProfileAsync(userId, model);
            TempData["SuccessMessage"] = "Cập nhật thông tin cá nhân thành công!";
            return RedirectToAction("Profile");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    /// <summary>
    /// [GET] Trang Đổi mật khẩu.
    /// </summary>
    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordViewModel());
    }

    /// <summary>
    /// [POST] Thực hiện đổi mật khẩu người dùng.
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            int userId = GetCurrentUserId();
            await _authService.ChangePasswordAsync(userId, model);
            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Profile");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }
    
    /// <summary>
    /// [POST] Thực hiện hành động quên mật khẩu.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(new ForgotPasswordViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var token = await _passwordResetService.GenerateTokenAsync(model.Email);
        if (token != null)
        {
            var link = Url.Action("ResetPassword", "Account", new { token }, Request.Scheme)!;

            var body = $@"<p>Xin chào,</p>
<p>Bạn vừa yêu cầu đặt lại mật khẩu tài khoản BarberShop. Bấm vào liên kết bên dưới (có hiệu lực 30 phút):</p>
<p><a href=""{link}"">Đặt lại mật khẩu</a></p>
<p>Nếu bạn không yêu cầu, hãy bỏ qua email này.</p>";

            await _emailSender.SendAsync(model.Email.Trim(), "Đặt lại mật khẩu BarberShop", body);

            // Development: hiện link trực tiếp để test khi chưa cấu hình SMTP
            if (_env.IsDevelopment())
            {
                TempData["DevResetLink"] = link;
            }
        }

        return RedirectToAction(nameof(ForgotPasswordConfirmation));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(string? token)
    {
        if (string.IsNullOrEmpty(token) || !await _passwordResetService.ValidateTokenAsync(token))
        {
            TempData["ErrorMessage"] = "Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.";
            return RedirectToAction(nameof(ForgotPassword));
        }

        return View(new ResetPasswordViewModel { Token = token });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _passwordResetService.ResetPasswordAsync(model.Token, model.NewPassword);
            TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công! Hãy đăng nhập bằng mật khẩu mới.";
            return RedirectToAction(nameof(Login));
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(ForgotPassword));
        }
    }

    /// <summary>
    /// [GET] Trang hiển thị thông báo Không có quyền truy cập (Access Denied).
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private async Task SignInUserAsync(Models.User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userIdClaim, out int id) ? id : 0;
    }
}