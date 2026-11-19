using BarberShopBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopBooking.Repositories;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(AppDbContext context) : base(context)
    {
    }

    public List<Appointment> GetByUser(int userId)
    {
        var list = Context.Appointments
            .Include(x => x.Service)
            .Include(x => x.Barber)
            .Include(x => x.BarberReview)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.AppointmentDate)
            .ToList();

        return list.OrderByDescending(x => x.AppointmentDate).ThenByDescending(x => x.AppointmentTime).ToList();
    }

    public async Task<List<Appointment>> GetByUserAsync(int userId)
    {
        var list = await Context.Appointments
            .Include(x => x.Service)
            .Include(x => x.Barber)
            .Include(x => x.BarberReview)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.AppointmentDate)
            .ToListAsync();

        return list.OrderByDescending(x => x.AppointmentDate).ThenByDescending(x => x.AppointmentTime).ToList();
    }

    public List<Appointment> GetAllWithDetails()
    {
        var list = Context.Appointments
            .Include(x => x.User)
            .Include(x => x.Service)
            .Include(x => x.Barber)
            .Include(x => x.BarberReview)
            .OrderByDescending(x => x.AppointmentDate)
            .ToList();

        return list.OrderByDescending(x => x.AppointmentDate).ThenByDescending(x => x.AppointmentTime).ToList();
    }

    public async Task<List<Appointment>> GetAllWithDetailsAsync()
    {
        var list = await Context.Appointments
            .Include(x => x.User)
            .Include(x => x.Service)
            .Include(x => x.Barber)
            .Include(x => x.BarberReview)
            .OrderByDescending(x => x.AppointmentDate)
            .ToListAsync();

        return list.OrderByDescending(x => x.AppointmentDate).ThenByDescending(x => x.AppointmentTime).ToList();
    }

    public async Task<Appointment?> GetByIdWithDetailsAsync(int appointmentId) =>
        await Context.Appointments
            .Include(x => x.User)
            .Include(x => x.Service)
            .Include(x => x.Barber)
            .Include(x => x.BarberReview)
            .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);

    public async Task<List<Appointment>> GetBookedAppointmentsAsync(int barberId, DateTime date) =>
        await Context.Appointments
            .Include(x => x.Service)
            .Where(x => x.BarberId == barberId 
                        && x.AppointmentDate.Date == date.Date 
                        && x.Status != AppointmentStatus.Cancelled)
            .ToListAsync();

    public bool HasActiveAppointmentForService(int serviceId) =>
        Context.Appointments
            .Any(x => x.ServiceId == serviceId && x.Status != AppointmentStatus.Cancelled);

    public async Task<bool> HasActiveAppointmentForServiceAsync(int serviceId) =>
        await Context.Appointments
            .AnyAsync(x => x.ServiceId == serviceId && x.Status != AppointmentStatus.Cancelled);

    public async Task<bool> HasActiveAppointmentForBarberAsync(int barberId) =>
        await Context.Appointments
            .AnyAsync(x => x.BarberId == barberId && x.Status != AppointmentStatus.Cancelled);
}
