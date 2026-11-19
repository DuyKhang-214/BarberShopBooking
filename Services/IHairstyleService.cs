using BarberShopBooking.Models;

namespace BarberShopBooking.Services;

public interface IHairstyleService
{
    Task<List<Hairstyle>> GetAllHairstylesAsync();
    Task<List<Hairstyle>> GetActiveHairstylesAsync(string? category = null);
    Task<Hairstyle?> GetByIdAsync(int id);
    Task CreateHairstyleAsync(Hairstyle hairstyle);
    Task UpdateHairstyleAsync(Hairstyle hairstyle);
    Task DeleteHairstyleAsync(int id);
}
