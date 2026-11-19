namespace BarberShopBooking.Services;

public interface IPasswordResetService
{
    Task<string?> GenerateTokenAsync(string email);
    Task<bool> ValidateTokenAsync(string token);
    Task ResetPasswordAsync(string token, string newPassword);
}