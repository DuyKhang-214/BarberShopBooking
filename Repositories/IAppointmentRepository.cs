using BarberShopBooking.Models;

namespace BarberShopBooking.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    List<Appointment> GetByUser(int userId);
    Task<List<Appointment>> GetByUserAsync(int userId);
    List<Appointment> GetAllWithDetails();
    Task<List<Appointment>> GetAllWithDetailsAsync();
    Task<Appointment?> GetByIdWithDetailsAsync(int appointmentId);
    Task<List<Appointment>> GetBookedAppointmentsAsync(int barberId, DateTime date);
    bool HasActiveAppointmentForService(int serviceId);
    Task<bool> HasActiveAppointmentForServiceAsync(int serviceId);
    Task<bool> HasActiveAppointmentForBarberAsync(int barberId);
}
