using BarberShopBooking.Models;
using BarberShopBooking.Repositories;

namespace BarberShopBooking.Services;

public class HairstyleService : IHairstyleService
{
    private readonly IHairstyleRepository _hairstyleRepository;

    public HairstyleService(IHairstyleRepository hairstyleRepository)
    {
        _hairstyleRepository = hairstyleRepository;
    }

    public async Task<List<Hairstyle>> GetAllHairstylesAsync() => await _hairstyleRepository.GetAllAsync();

    public async Task<List<Hairstyle>> GetActiveHairstylesAsync(string? category = null) => await _hairstyleRepository.GetActiveAsync(category);

    public async Task<Hairstyle?> GetByIdAsync(int id) => await _hairstyleRepository.GetByIdAsync(id);

    public async Task CreateHairstyleAsync(Hairstyle hairstyle)
    {
        hairstyle.HairstyleName = hairstyle.HairstyleName.Trim();
        hairstyle.Description = hairstyle.Description.Trim();
        hairstyle.Category = string.IsNullOrWhiteSpace(hairstyle.Category) ? "Kiểu tóc" : hairstyle.Category.Trim();
        hairstyle.CreatedAt = DateTime.Now;
        hairstyle.IsActive = true;

        await _hairstyleRepository.AddAsync(hairstyle);
        await _hairstyleRepository.SaveChangesAsync();
    }

    public async Task UpdateHairstyleAsync(Hairstyle hairstyle)
    {
        var existing = await _hairstyleRepository.GetByIdAsync(hairstyle.HairstyleId)
            ?? throw new InvalidOperationException("Kiểu tóc không tồn tại.");

        existing.HairstyleName = hairstyle.HairstyleName.Trim();
        existing.Description = hairstyle.Description.Trim();
        existing.Category = string.IsNullOrWhiteSpace(hairstyle.Category) ? "Kiểu tóc" : hairstyle.Category.Trim();
        existing.IsActive = hairstyle.IsActive;
        if (!string.IsNullOrEmpty(hairstyle.ImageUrl))
        {
            existing.ImageUrl = hairstyle.ImageUrl;
        }

        _hairstyleRepository.Update(existing);
        await _hairstyleRepository.SaveChangesAsync();
    }

    public async Task DeleteHairstyleAsync(int id)
    {
        var hairstyle = await _hairstyleRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Kiểu tóc không tồn tại.");

        _hairstyleRepository.Delete(hairstyle);
        await _hairstyleRepository.SaveChangesAsync();
    }
}
