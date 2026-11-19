using BarberShopBooking.Models;

namespace BarberShopBooking.Repositories;

public interface IContactRepository : IRepository<Contact>
{
    Task<List<Contact>> GetAllWithUserAsync();
    Task<List<Contact>> GetByUserIdAsync(int userId);
}

public interface IHairstyleRepository : IRepository<Hairstyle>
{
    Task<List<Hairstyle>> GetActiveAsync(string? category = null);
}
