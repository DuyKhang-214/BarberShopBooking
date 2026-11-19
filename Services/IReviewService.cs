using BarberShopBooking.Models;
using BarberShopBooking.ViewModels;

namespace BarberShopBooking.Services;

public interface IReviewService
{
    Task<List<BarberReview>> GetPublicReviewsAsync();
    Task<ReviewCreateViewModel> PrepareReviewViewModelAsync(int userId, int appointmentId);
    Task CreateReviewAsync(int userId, ReviewCreateViewModel model);
    Task DeleteReviewAsync(int reviewId);
    Task<BarberReview?> GetByIdAsync(int reviewId);
}
