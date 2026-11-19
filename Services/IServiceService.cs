using BarberShopBooking.Models;

namespace BarberShopBooking.Services;

public interface IServiceService
{
    Task<List<Service>> GetAllServicesAsync();
    Task<List<Service>> GetActiveServicesAsync();
    Task<Service?> GetByIdAsync(int id);
    Task CreateServiceAsync(Service service);
    Task UpdateServiceAsync(Service service);
    Task DeleteServiceAsync(int id);
}
