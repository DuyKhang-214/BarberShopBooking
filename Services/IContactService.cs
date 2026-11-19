using BarberShopBooking.Models;

namespace BarberShopBooking.Services;

public interface IContactService
{
    Task SubmitContactAsync(int? userId, string fullName, string email, string? phone, string message);
    Task<List<Contact>> GetAllContactsAsync();
    Task<List<Contact>> GetMyContactsAsync(int userId);
    Task ReplyContactAsync(int contactId, string replyMessage);
    Task<Contact?> GetByIdAsync(int contactId);
}
