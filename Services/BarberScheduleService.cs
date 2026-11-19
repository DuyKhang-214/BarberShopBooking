using BarberShopBooking.Models;
using BarberShopBooking.Repositories;
using BarberShopBooking.ViewModels;

namespace BarberShopBooking.Services;

public class BarberScheduleService : IBarberScheduleService
{
    private readonly IBarberScheduleRepository _scheduleRepository;
    private readonly IBarberRepository _barberRepository;

    public BarberScheduleService(
        IBarberScheduleRepository scheduleRepository,
        IBarberRepository barberRepository)
    {
        _scheduleRepository = scheduleRepository;
        _barberRepository = barberRepository;
    }

    public async Task EnsureDefaultSchedulesAsync(int barberId)
    {
        var existing = await _scheduleRepository.GetByBarberIdAsync(barberId);
        if (existing.Count == 0)
        {
            foreach (DayOfWeek day in Enum.GetValues<DayOfWeek>())
            {
                await _scheduleRepository.AddAsync(new BarberSchedule
                {
                    BarberId = barberId,
                    DayOfWeek = day,
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = day == DayOfWeek.Sunday ? new TimeSpan(17, 0, 0) : new TimeSpan(19, 0, 0),
                    IsWorkingDay = true
                });
            }
            await _scheduleRepository.SaveChangesAsync();
        }
    }

    public async Task<BarberScheduleManageViewModel> GetScheduleForManageAsync(int barberId)
    {
        var barber = await _barberRepository.GetByIdAsync(barberId)
            ?? throw new InvalidOperationException("Không tìm thấy barber.");

        await EnsureDefaultSchedulesAsync(barberId);

        var schedules = await _scheduleRepository.GetByBarberIdAsync(barberId);

        var dayNames = new Dictionary<DayOfWeek, string>
        {
            [DayOfWeek.Monday] = "Thứ Hai",
            [DayOfWeek.Tuesday] = "Thứ Ba",
            [DayOfWeek.Wednesday] = "Thứ Tư",
            [DayOfWeek.Thursday] = "Thứ Năm",
            [DayOfWeek.Friday] = "Thứ Sáu",
            [DayOfWeek.Saturday] = "Thứ Bảy",
            [DayOfWeek.Sunday] = "Chủ Nhật"
        };

        var model = new BarberScheduleManageViewModel
        {
            BarberId = barber.BarberId,
            BarberName = barber.FullName,
            Schedules = schedules.Select(s => new BarberScheduleItemViewModel
            {
                DayOfWeek = s.DayOfWeek,
                DayOfWeekName = dayNames.GetValueOrDefault(s.DayOfWeek, s.DayOfWeek.ToString()),
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsWorkingDay = s.IsWorkingDay
            }).OrderBy(x => (int)x.DayOfWeek == 0 ? 7 : (int)x.DayOfWeek).ToList()
        };

        return model;
    }

    public async Task UpdateSchedulesAsync(BarberScheduleManageViewModel model)
    {
        var schedules = await _scheduleRepository.GetByBarberIdAsync(model.BarberId);
        foreach (var item in model.Schedules)
        {
            var schedule = schedules.FirstOrDefault(s => s.DayOfWeek == item.DayOfWeek);
            if (schedule != null)
            {
                schedule.StartTime = item.StartTime;
                schedule.EndTime = item.EndTime;
                schedule.IsWorkingDay = item.IsWorkingDay;
                _scheduleRepository.Update(schedule);
            }
        }

        await _scheduleRepository.SaveChangesAsync();
    }
}
