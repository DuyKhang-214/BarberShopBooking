namespace BarberShopBooking.Models;

public class Contact
{
    public int ContactId { get; set; }

    public int? UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string Message { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string? ReplyMessage { get; set; }

    public DateTime? RepliedAt { get; set; }

    public User? User { get; set; }
}