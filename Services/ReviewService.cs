using BarberShopBooking.Models;
using BarberShopBooking.Repositories;
using BarberShopBooking.ViewModels;

namespace BarberShopBooking.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public ReviewService(
        IReviewRepository reviewRepository,
        IAppointmentRepository appointmentRepository)
    {
        _reviewRepository = reviewRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<List<BarberReview>> GetPublicReviewsAsync()
    {
        return await _reviewRepository.GetAllPublicReviewsAsync();
    }

    public async Task<ReviewCreateViewModel> PrepareReviewViewModelAsync(int userId, int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdWithDetailsAsync(appointmentId)
            ?? throw new InvalidOperationException("Lịch hẹn không tồn tại.");

        if (appointment.UserId != userId)
        {
            throw new UnauthorizedAccessException("Bạn chỉ được đánh giá lịch hẹn của chính mình.");
        }

        if (appointment.Status != AppointmentStatus.Completed)
        {
            throw new InvalidOperationException("Chỉ có thể đánh giá sau khi lịch hẹn đã hoàn thành.");
        }

        if (await _reviewRepository.HasReviewForAppointmentAsync(appointmentId))
        {
            throw new InvalidOperationException("Lịch hẹn này đã được đánh giá trước đó.");
        }

        return new ReviewCreateViewModel
        {
            AppointmentId = appointment.AppointmentId,
            BarberId = appointment.BarberId,
            BarberName = appointment.Barber?.FullName ?? "N/A",
            ServiceName = appointment.Service?.ServiceName ?? "N/A",
            AppointmentDate = appointment.AppointmentDate,
            Rating = 5
        };
    }

    public async Task CreateReviewAsync(int userId, ReviewCreateViewModel model)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(model.AppointmentId)
            ?? throw new InvalidOperationException("Lịch hẹn không tồn tại.");

        if (appointment.UserId != userId)
        {
            throw new UnauthorizedAccessException("Bạn chỉ được đánh giá lịch hẹn của chính mình.");
        }

        if (appointment.Status != AppointmentStatus.Completed)
        {
            throw new InvalidOperationException("Chỉ có thể đánh giá sau khi lịch hẹn đã hoàn thành.");
        }

        if (await _reviewRepository.HasReviewForAppointmentAsync(model.AppointmentId))
        {
            throw new InvalidOperationException("Lịch hẹn này đã được đánh giá trước đó.");
        }

        var review = new BarberReview
        {
            AppointmentId = model.AppointmentId,
            UserId = userId,
            BarberId = appointment.BarberId,
            Rating = Math.Clamp(model.Rating, 1, 5),
            Comment = model.Comment?.Trim(),
            CreatedAt = DateTime.Now
        };

        await _reviewRepository.AddAsync(review);
        await _reviewRepository.SaveChangesAsync();
    }

    public async Task DeleteReviewAsync(int reviewId)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId)
            ?? throw new InvalidOperationException("Không tìm thấy đánh giá.");

        _reviewRepository.Delete(review);
        await _reviewRepository.SaveChangesAsync();
    }

    public async Task<BarberReview?> GetByIdAsync(int reviewId)
    {
        return await _reviewRepository.GetByIdAsync(reviewId);
    }
}
