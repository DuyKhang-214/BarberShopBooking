using BarberShopBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopBooking.Repositories;

public class ServiceRepository : Repository<Service>, IServiceRepository
{
    public ServiceRepository(AppDbContext context) : base(context)
    {
    }

    public List<Service> GetActive() =>
        Context.Services.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).ToList();

    public async Task<List<Service>> GetActiveAsync() =>
        await Context.Services.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).ToListAsync();
}
