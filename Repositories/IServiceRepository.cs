using BarberShopBooking.Models;

namespace BarberShopBooking.Repositories;

public interface IServiceRepository : IRepository<Service>
{
    List<Service> GetActive();
    Task<List<Service>> GetActiveAsync();
}
