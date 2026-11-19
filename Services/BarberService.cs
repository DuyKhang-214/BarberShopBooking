using BarberShopBooking.Models;
using BarberShopBooking.Repositories;

namespace BarberShopBooking.Services;

public class BarberService : IBarberService
{
    private readonly IBarberRepository _barberRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IBarberScheduleService _scheduleService;

    public BarberService(
        IBarberRepository barberRepository,
        IAppointmentRepository appointmentRepository,
        IBarberScheduleService scheduleService)
    {
        _barberRepository = barberRepository;
        _appointmentRepository = appointmentRepository;
        _scheduleService = scheduleService;
    }

    public async Task<List<Barber>> GetAllBarbersAsync() => await _barberRepository.GetAllAsync();

    public async Task<List<Barber>> GetActiveBarbersAsync() => await _barberRepository.GetActiveAsync();

    public async Task<Barber?> GetByIdAsync(int id) => await _barberRepository.GetByIdAsync(id);

    public async Task CreateBarberAsync(Barber barber)
    {
        barber.FullName = barber.FullName.Trim();
        barber.IsActive = true;
        barber.CreatedAt = DateTime.Now;
        await _barberRepository.AddAsync(barber);
        await _barberRepository.SaveChangesAsync();

        // Create default working schedules for new barber
        await _scheduleService.EnsureDefaultSchedulesAsync(barber.BarberId);
    }

    public async Task UpdateBarberAsync(Barber barber)
    {
        var existing = await _barberRepository.GetByIdAsync(barber.BarberId)
            ?? throw new InvalidOperationException("Barber không tồn tại.");

        existing.FullName = barber.FullName.Trim();
        existing.Phone = barber.Phone?.Trim();
        existing.SkillDescription = barber.SkillDescription?.Trim();
        existing.ExperienceYears = barber.ExperienceYears;
        existing.IsActive = barber.IsActive;
        if (!string.IsNullOrEmpty(barber.ImageUrl))
        {
            existing.ImageUrl = barber.ImageUrl;
        }

        _barberRepository.Update(existing);
        await _barberRepository.SaveChangesAsync();
    }

    public async Task DeleteBarberAsync(int id)
    {
        var barber = await _barberRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Barber không tồn tại.");

        bool hasAppointments = await _appointmentRepository.HasActiveAppointmentForBarberAsync(id);
        if (hasAppointments)
        {
            // Soft delete
            barber.IsActive = false;
            _barberRepository.Update(barber);
        }
        else
        {
            _barberRepository.Delete(barber);
        }

        await _barberRepository.SaveChangesAsync();
    }
}
