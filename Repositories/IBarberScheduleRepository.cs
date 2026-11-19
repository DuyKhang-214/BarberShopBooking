using BarberShopBooking.Models;

namespace BarberShopBooking.Repositories;

public interface IBarberScheduleRepository : IRepository<BarberSchedule>
{
    Task<List<BarberSchedule>> GetByBarberIdAsync(int barberId);
    Task<BarberSchedule?> GetByBarberAndDayAsync(int barberId, DayOfWeek dayOfWeek);
}
