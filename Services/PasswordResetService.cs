using System.Security.Cryptography;
using System.Text;
using BarberShopBooking.Models;
using BarberShopBooking.Repositories;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;

namespace BarberShopBooking.Services;

public class PasswordResetService : IPasswordResetService
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(30);

    private readonly IUserRepository _userRepository;
    private readonly ITimeLimitedDataProtector _protector;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public PasswordResetService(IUserRepository userRepository, IDataProtectionProvider dataProtectionProvider)
    {
        _userRepository = userRepository;
        _protector = dataProtectionProvider
            .CreateProtector("BarberShopBooking.PasswordReset")
            .ToTimeLimitedDataProtector();
    }

    public async Task<string?> GenerateTokenAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email.Trim());
        if (user == null || !user.IsActive)
        {
            return null;
        }

        return _protector.Protect($"{user.UserId}|{Fingerprint(user)}", TokenLifetime);
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        return await ResolveUserAsync(token) != null;
    }

    public async Task ResetPasswordAsync(string token, string newPassword)
    {
        var user = await ResolveUserAsync(token)
            ?? throw new InvalidOperationException("Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.");

        user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }

    private async Task<User?> ResolveUserAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        string payload;
        try
        {
            payload = _protector.Unprotect(token); // lỗi nếu sai chữ ký hoặc hết hạn
        }
        catch (CryptographicException)
        {
            return null;
        }

        var parts = payload.Split('|');
        if (parts.Length != 2 || !int.TryParse(parts[0], out var userId))
        {
            return null;
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || !user.IsActive || Fingerprint(user) != parts[1])
        {
            return null;
        }

        return user;
    }

    private static string Fingerprint(User user)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(user.PasswordHash));
        return Convert.ToHexString(bytes)[..16];
    }
}