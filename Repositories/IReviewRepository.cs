using BarberShopBooking.Models;

namespace BarberShopBooking.Repositories;

public interface IReviewRepository : IRepository<BarberReview>
{
    Task<List<BarberReview>> GetAllPublicReviewsAsync();
    Task<BarberReview?> GetByAppointmentIdAsync(int appointmentId);
    Task<bool> HasReviewForAppointmentAsync(int appointmentId);
    Task<List<BarberReview>> GetByBarberIdAsync(int barberId);
}
