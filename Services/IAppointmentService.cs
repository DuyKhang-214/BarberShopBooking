using BarberShopBooking.Models;
using BarberShopBooking.ViewModels;

namespace BarberShopBooking.Services;

public interface IAppointmentService
{
    Task<Appointment> CreateAppointmentAsync(int userId, AppointmentCreateViewModel model);
    Task<List<Appointment>> GetMyAppointmentsAsync(int userId);
    Task<List<Appointment>> GetAllForAdminAsync();
    Task<Appointment?> GetByIdAsync(int appointmentId);
    Task ChangeStatusAsync(int appointmentId, AppointmentStatus status);
    Task CancelByCustomerAsync(int userId, int appointmentId);
    Task<List<AvailableSlotViewModel>> GetAvailableSlotsAsync(int barberId, int serviceId, DateTime date);
    Task<List<Service>> GetActiveServicesAsync();
    Task<List<Barber>> GetActiveBarbersAsync();
    Task<bool> HasActiveAppointmentForServiceAsync(int serviceId);
    Task<bool> HasActiveAppointmentForBarberAsync(int barberId);
}
