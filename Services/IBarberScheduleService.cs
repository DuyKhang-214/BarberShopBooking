using BarberShopBooking.ViewModels;

namespace BarberShopBooking.Services;

public interface IBarberScheduleService
{
    Task<BarberScheduleManageViewModel> GetScheduleForManageAsync(int barberId);
    Task UpdateSchedulesAsync(BarberScheduleManageViewModel model);
    Task EnsureDefaultSchedulesAsync(int barberId);
}
