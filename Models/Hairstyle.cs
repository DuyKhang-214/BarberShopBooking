using System.ComponentModel.DataAnnotations;

namespace BarberShopBooking.Models;

public class Hairstyle
{
    [Key]
    public int HairstyleId { get; set; }

    public string HairstyleName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    
    public string Category { get; set; } = "Kiểu tóc";
}