using BarberShopBooking.Models;
using BarberShopBooking.ViewModels;

namespace BarberShopBooking.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(RegisterViewModel model);
    Task<User?> AuthenticateAsync(string username, string password);
    Task<User?> GetByIdAsync(int userId);
    Task UpdateProfileAsync(int userId, EditProfileViewModel model);
    Task ChangePasswordAsync(int userId, ChangePasswordViewModel model);
    Task<bool> CanModifyUserRoleOrStatusAsync(int targetUserId, string newRole, bool newIsActive);
    Task<List<User>> GetAllUsersAsync();
    Task ToggleUserActiveStatusAsync(int targetUserId);
    Task ChangeUserRoleAsync(int targetUserId, string newRole);
}
