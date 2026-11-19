using BarberShopBooking.Models;

namespace BarberShopBooking.ViewModels;

public class DashboardViewModel
{
    public int TotalUsers { get; set; }
    public int TotalAppointments { get; set; }
    public int PendingAppointments { get; set; }
    public int ConfirmedAppointments { get; set; }
    public int CompletedAppointments { get; set; }
    public int CancelledAppointments { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalServices { get; set; }
    public int TotalBarbers { get; set; }
    public int TotalReviews { get; set; }
    public int PendingContacts { get; set; }

    public List<Appointment> RecentAppointments { get; set; } = new();
}
