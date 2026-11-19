namespace BarberShopBooking.Models;

public class Barber
{
    public int BarberId { get; set; }

    public string FullName { get; set; } = "";

    public string? Phone { get; set; }

    public string? SkillDescription { get; set; }

    public string? ImageUrl { get; set; }

    public int ExperienceYears { get; set; } = 3;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<BarberReview> BarberReviews { get; set; } = new List<BarberReview>();
    public ICollection<BarberSchedule> BarberSchedules { get; set; } = new List<BarberSchedule>();
}