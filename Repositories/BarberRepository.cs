using BarberShopBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopBooking.Repositories;

public class BarberRepository : Repository<Barber>, IBarberRepository
{
    public BarberRepository(AppDbContext context) : base(context)
    {
    }

    public List<Barber> GetActive() =>
        Context.Barbers.Where(x => x.IsActive).ToList();

    public async Task<List<Barber>> GetActiveAsync() =>
        await Context.Barbers.Where(x => x.IsActive).ToListAsync();
}
