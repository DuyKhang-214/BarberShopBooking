using System.ComponentModel.DataAnnotations;

namespace BarberShopBooking.Models;

public class BarberReview
{
    [Key]
    public int ReviewId { get; set; }

    public int AppointmentId { get; set; }
    public int UserId { get; set; }
    public int BarberId { get; set; }

    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    public Appointment? Appointment { get; set; }
    public User? User { get; set; }
    public Barber? Barber { get; set; }
}