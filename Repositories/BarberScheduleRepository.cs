using BarberShopBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopBooking.Repositories;

public class BarberScheduleRepository : Repository<BarberSchedule>, IBarberScheduleRepository
{
    public BarberScheduleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<BarberSchedule>> GetByBarberIdAsync(int barberId) =>
        await Context.BarberSchedules
            .Where(s => s.BarberId == barberId)
            .OrderBy(s => s.DayOfWeek)
            .ToListAsync();

    public async Task<BarberSchedule?> GetByBarberAndDayAsync(int barberId, DayOfWeek dayOfWeek) =>
        await Context.BarberSchedules
            .FirstOrDefaultAsync(s => s.BarberId == barberId && s.DayOfWeek == dayOfWeek);
}
