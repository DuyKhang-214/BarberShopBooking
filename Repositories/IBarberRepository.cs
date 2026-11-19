using BarberShopBooking.Models;

namespace BarberShopBooking.Repositories;

public interface IBarberRepository : IRepository<Barber>
{
    List<Barber> GetActive();
    Task<List<Barber>> GetActiveAsync();
}
