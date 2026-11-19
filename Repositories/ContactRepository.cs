using BarberShopBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopBooking.Repositories;

public class ContactRepository : Repository<Contact>, IContactRepository
{
    public ContactRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Contact>> GetAllWithUserAsync() =>
        await Context.Contacts
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

    public async Task<List<Contact>> GetByUserIdAsync(int userId) =>
        await Context.Contacts
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
}

public class HairstyleRepository : Repository<Hairstyle>, IHairstyleRepository
{
    public HairstyleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Hairstyle>> GetActiveAsync(string? category = null)
    {
        var query = Context.Hairstyles.Where(h => h.IsActive);
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(h => h.Category.ToLower() == category.ToLower() || h.Category.ToLower().Contains(category.ToLower()));
        }
        return await query.OrderByDescending(h => h.CreatedAt).ToListAsync();
    }
}
