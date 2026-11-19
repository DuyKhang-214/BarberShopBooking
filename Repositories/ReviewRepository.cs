using BarberShopBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopBooking.Repositories;

public class ReviewRepository : Repository<BarberReview>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<BarberReview>> GetAllPublicReviewsAsync() =>
        await Context.BarberReviews
            .Include(r => r.User)
            .Include(r => r.Barber)
            .Include(r => r.Appointment)
            .ThenInclude(a => a!.Service)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

    public async Task<BarberReview?> GetByAppointmentIdAsync(int appointmentId) =>
        await Context.BarberReviews
            .Include(r => r.User)
            .Include(r => r.Barber)
            .FirstOrDefaultAsync(r => r.AppointmentId == appointmentId);

    public async Task<bool> HasReviewForAppointmentAsync(int appointmentId) =>
        await Context.BarberReviews.AnyAsync(r => r.AppointmentId == appointmentId);

    public async Task<List<BarberReview>> GetByBarberIdAsync(int barberId) =>
        await Context.BarberReviews
            .Include(r => r.User)
            .Where(r => r.BarberId == barberId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
}
