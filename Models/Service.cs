namespace BarberShopBooking.Models;

public class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = "";

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int DurationMinutes { get; set; }

    public bool IsActive { get; set; } = true;
    
    public bool IsFeatured { get; set; }
    
    public int DisplayOrder { get; set; }

    public string? ImageUrl { get; set; }

    // Navigation properties
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}