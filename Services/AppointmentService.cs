using BarberShopBooking.Models;
using BarberShopBooking.Repositories;
using BarberShopBooking.ViewModels;

namespace BarberShopBooking.Services;

/// <summary>
/// Service xử lý Logic nghiệp vụ Đặt Lịch Cắt Tóc (Appointment Business Logic).
/// Bao gồm: Tính toán Khung Giờ Trống (Slot), Kiểm tra trùng lịch thợ, Ràng buộc Hủy lịch trước 2h.
/// </summary>
public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IBarberRepository _barberRepository;
    private readonly IBarberScheduleRepository _scheduleRepository;
    private readonly IUserRepository _userRepository;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IServiceRepository serviceRepository,
        IBarberRepository barberRepository,
        IBarberScheduleRepository scheduleRepository,
        IUserRepository userRepository)
    {
        _appointmentRepository = appointmentRepository;
        _serviceRepository = serviceRepository;
        _barberRepository = barberRepository;
        _scheduleRepository = scheduleRepository;
        _userRepository = userRepository;
    }

    public async Task<List<AvailableSlotViewModel>> GetAvailableSlotsAsync(int barberId, int serviceId, DateTime date)
    {
        var slots = new List<AvailableSlotViewModel>();

        var service = await _serviceRepository.GetByIdAsync(serviceId);
        if (service == null || !service.IsActive)
        {
            return slots;
        }

        var barber = await _barberRepository.GetByIdAsync(barberId);
        if (barber == null || !barber.IsActive)
        {
            return slots;
        }

        // Get Barber schedule for this day of week
        var daySchedule = await _scheduleRepository.GetByBarberAndDayAsync(barberId, date.DayOfWeek);
        if (daySchedule == null || !daySchedule.IsWorkingDay)
        {
            return slots; // Barber does not work on this day
        }

        int duration = service.DurationMinutes <= 0 ? 30 : service.DurationMinutes;
        var durationSpan = TimeSpan.FromMinutes(duration);

        // Existing non-cancelled appointments for this barber on this date
        var existingAppointments = await _appointmentRepository.GetBookedAppointmentsAsync(barberId, date);

        var currentTime = daySchedule.StartTime;
        var now = DateTime.Now;

        // Step by 30-minute intervals
        while (currentTime + durationSpan <= daySchedule.EndTime)
        {
            var slotStart = currentTime;
            var slotEnd = slotStart + durationSpan;
            var slotStartDateTime = date.Date + slotStart;

            bool isAvailable = true;
            string? reason = null;

            // 1. Past time check
            if (slotStartDateTime <= now.AddMinutes(5))
            {
                isAvailable = false;
                reason = "Thời gian đã qua";
            }
            else
            {
                // 2. Overlap check with existing appointments
                foreach (var appt in existingAppointments)
                {
                    int apptDuration = appt.Service?.DurationMinutes ?? 30;
                    var apptStart = appt.AppointmentTime;
                    var apptEnd = apptStart + TimeSpan.FromMinutes(apptDuration);

                    // Overlap formula: newStart < existingEnd && newEnd > existingStart
                    if (slotStart < apptEnd && slotEnd > apptStart)
                    {
                        isAvailable = false;
                        reason = "Đã có lịch hẹn trùng giờ";
                        break;
                    }
                }
            }

            slots.Add(new AvailableSlotViewModel
            {
                Time = slotStart,
                DisplayTime = $"{slotStart.Hours:D2}:{slotStart.Minutes:D2} - {slotEnd.Hours:D2}:{slotEnd.Minutes:D2}",
                IsAvailable = isAvailable,
                Reason = reason
            });

            currentTime = currentTime.Add(TimeSpan.FromMinutes(30));
        }

        return slots;
    }

    public async Task<Appointment> CreateAppointmentAsync(int userId, AppointmentCreateViewModel model)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || !user.IsActive)
        {
            throw new InvalidOperationException("Phiên đăng nhập của bạn đã hết hạn hoặc tài khoản không hợp lệ. Vui lòng đăng nhập lại.");
        }

        var service = await _serviceRepository.GetByIdAsync(model.ServiceId)
            ?? throw new InvalidOperationException("Dịch vụ được chọn không tồn tại hoặc đã ngừng hoạt động.");

        var barber = await _barberRepository.GetByIdAsync(model.BarberId)
            ?? throw new InvalidOperationException("Barber được chọn không tồn tại hoặc đã ngừng hoạt động.");

        DateTime startDateTime = model.AppointmentDate.Date + model.AppointmentTime;
        if (startDateTime <= DateTime.Now)
        {
            throw new InvalidOperationException("Thời gian đặt lịch phải ở trong tương lai.");
        }

        // Validate Schedule
        var daySchedule = await _scheduleRepository.GetByBarberAndDayAsync(model.BarberId, model.AppointmentDate.DayOfWeek);
        if (daySchedule == null || !daySchedule.IsWorkingDay)
        {
            throw new InvalidOperationException("Barber không làm việc vào ngày này.");
        }

        TimeSpan endSlot = model.AppointmentTime + TimeSpan.FromMinutes(service.DurationMinutes);
        if (model.AppointmentTime < daySchedule.StartTime || endSlot > daySchedule.EndTime)
        {
            throw new InvalidOperationException("Khung giờ nằm ngoài giờ làm việc của thợ cắt tóc.");
        }

        // Double-check Collision Server-side
        var existingAppointments = await _appointmentRepository.GetBookedAppointmentsAsync(model.BarberId, model.AppointmentDate);
        foreach (var appt in existingAppointments)
        {
            int apptDuration = appt.Service?.DurationMinutes ?? 30;
            var apptStart = appt.AppointmentTime;
            var apptEnd = apptStart + TimeSpan.FromMinutes(apptDuration);

            if (model.AppointmentTime < apptEnd && endSlot > apptStart)
            {
                throw new InvalidOperationException("Khung giờ này đã có người đặt trước. Vui lòng chọn khung giờ khác.");
            }
        }

        var appointment = new Appointment
        {
            UserId = userId,
            ServiceId = service.ServiceId,
            BarberId = barber.BarberId,
            AppointmentDate = model.AppointmentDate.Date,
            AppointmentTime = model.AppointmentTime,
            Note = model.Note?.Trim(),
            PriceAtBooking = service.Price,
            CreatedAt = DateTime.Now
        };

        await _appointmentRepository.AddAsync(appointment);
        await _appointmentRepository.SaveChangesAsync();

        return appointment;
    }

    public async Task<List<Appointment>> GetMyAppointmentsAsync(int userId)
    {
        return await _appointmentRepository.GetByUserAsync(userId);
    }

    public async Task<List<Appointment>> GetAllForAdminAsync()
    {
        return await _appointmentRepository.GetAllWithDetailsAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int appointmentId)
    {
        return await _appointmentRepository.GetByIdWithDetailsAsync(appointmentId);
    }

    public async Task ChangeStatusAsync(int appointmentId, AppointmentStatus status)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId)
            ?? throw new InvalidOperationException("Không tìm thấy lịch hẹn.");

        appointment.ChangeStatus(status);
        await _appointmentRepository.SaveChangesAsync();
    }

    public async Task CancelByCustomerAsync(int userId, int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId)
            ?? throw new InvalidOperationException("Không tìm thấy lịch hẹn.");

        if (appointment.UserId != userId)
        {
            throw new UnauthorizedAccessException("Bạn không có quyền hủy lịch hẹn của người khác.");
        }

        if (appointment.Status != AppointmentStatus.Pending && appointment.Status != AppointmentStatus.Confirmed)
        {
            throw new InvalidOperationException("Chỉ có thể hủy lịch hẹn đang ở trạng thái 'Chờ xác nhận' hoặc 'Đã xác nhận'.");
        }

        DateTime appointmentDateTime = appointment.AppointmentDate.Date + appointment.AppointmentTime;
        TimeSpan timeDifference = appointmentDateTime - DateTime.Now;

        if (timeDifference < TimeSpan.FromHours(2))
        {
            throw new InvalidOperationException("Bạn chỉ được hủy lịch trước thời gian hẹn ít nhất 2 giờ.");
        }

        appointment.Cancel();
        await _appointmentRepository.SaveChangesAsync();
    }

    public async Task<List<Service>> GetActiveServicesAsync()
    {
        return await _serviceRepository.GetActiveAsync();
    }

    public async Task<List<Barber>> GetActiveBarbersAsync()
    {
        return await _barberRepository.GetActiveAsync();
    }

    public async Task<bool> HasActiveAppointmentForServiceAsync(int serviceId)
    {
        return await _appointmentRepository.HasActiveAppointmentForServiceAsync(serviceId);
    }

    public async Task<bool> HasActiveAppointmentForBarberAsync(int barberId)
    {
        return await _appointmentRepository.HasActiveAppointmentForBarberAsync(barberId);
    }
}
