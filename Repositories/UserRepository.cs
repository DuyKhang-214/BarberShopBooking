using BarberShopBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopBooking.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username) =>
        await Context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

    public async Task<User?> GetByEmailAsync(string email) =>
        await Context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

    public async Task<bool> ExistsUsernameAsync(string username) =>
        await Context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());

    public async Task<bool> ExistsEmailAsync(string email) =>
        await Context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());

    public async Task<int> GetActiveAdminCountAsync() =>
        await Context.Users.CountAsync(u => u.Role == "Admin" && u.IsActive);
}
