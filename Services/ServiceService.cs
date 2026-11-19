using BarberShopBooking.Models;
using BarberShopBooking.Repositories;

namespace BarberShopBooking.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public ServiceService(
        IServiceRepository serviceRepository,
        IAppointmentRepository appointmentRepository)
    {
        _serviceRepository = serviceRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<List<Service>> GetAllServicesAsync() => await _serviceRepository.GetAllAsync();

    public async Task<List<Service>> GetActiveServicesAsync() => await _serviceRepository.GetActiveAsync();

    public async Task<Service?> GetByIdAsync(int id) => await _serviceRepository.GetByIdAsync(id);

    public async Task CreateServiceAsync(Service service)
    {
        service.ServiceName = service.ServiceName.Trim();
        service.IsActive = true;
        await _serviceRepository.AddAsync(service);
        await _serviceRepository.SaveChangesAsync();
    }

    public async Task UpdateServiceAsync(Service service)
    {
        var existing = await _serviceRepository.GetByIdAsync(service.ServiceId)
            ?? throw new InvalidOperationException("Dịch vụ không tồn tại.");

        existing.ServiceName = service.ServiceName.Trim();
        existing.Description = service.Description?.Trim();
        existing.Price = service.Price;
        existing.DurationMinutes = service.DurationMinutes;
        existing.IsFeatured = service.IsFeatured;
        existing.DisplayOrder = service.DisplayOrder;
        existing.IsActive = service.IsActive;
        if (!string.IsNullOrEmpty(service.ImageUrl))
        {
            existing.ImageUrl = service.ImageUrl;
        }

        _serviceRepository.Update(existing);
        await _serviceRepository.SaveChangesAsync();
    }

    public async Task DeleteServiceAsync(int id)
    {
        var service = await _serviceRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Dịch vụ không tồn tại.");

        bool hasAppointments = await _appointmentRepository.HasActiveAppointmentForServiceAsync(id);
        if (hasAppointments)
        {
            // Soft delete
            service.IsActive = false;
            _serviceRepository.Update(service);
        }
        else
        {
            _serviceRepository.Delete(service);
        }

        await _serviceRepository.SaveChangesAsync();
    }
}
