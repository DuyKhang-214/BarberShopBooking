using BarberShopBooking.Models;

namespace BarberShopBooking.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsUsernameAsync(string username);
    Task<bool> ExistsEmailAsync(string email);
    Task<int> GetActiveAdminCountAsync();
}
