using BarberShopBooking.Models;

namespace BarberShopBooking.Services;

public interface IBarberService
{
    Task<List<Barber>> GetAllBarbersAsync();
    Task<List<Barber>> GetActiveBarbersAsync();
    Task<Barber?> GetByIdAsync(int id);
    Task CreateBarberAsync(Barber barber);
    Task UpdateBarberAsync(Barber barber);
    Task DeleteBarberAsync(int id);
}
