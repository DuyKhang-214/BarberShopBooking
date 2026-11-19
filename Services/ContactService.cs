using BarberShopBooking.Models;
using BarberShopBooking.Repositories;

namespace BarberShopBooking.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task SubmitContactAsync(int? userId, string fullName, string email, string? phone, string message)
    {
        var contact = new Contact
        {
            UserId = userId,
            FullName = fullName.Trim(),
            Email = email.Trim().ToLower(),
            Phone = phone?.Trim(),
            Message = message.Trim(),
            CreatedAt = DateTime.Now
        };

        await _contactRepository.AddAsync(contact);
        await _contactRepository.SaveChangesAsync();
    }

    public async Task<List<Contact>> GetAllContactsAsync()
    {
        return await _contactRepository.GetAllWithUserAsync();
    }

    public async Task<List<Contact>> GetMyContactsAsync(int userId)
    {
        return await _contactRepository.GetByUserIdAsync(userId);
    }

    public async Task ReplyContactAsync(int contactId, string replyMessage)
    {
        var contact = await _contactRepository.GetByIdAsync(contactId)
            ?? throw new InvalidOperationException("Không tìm thấy liên hệ.");

        contact.ReplyMessage = replyMessage.Trim();
        contact.RepliedAt = DateTime.Now;

        _contactRepository.Update(contact);
        await _contactRepository.SaveChangesAsync();
    }

    public async Task<Contact?> GetByIdAsync(int contactId)
    {
        return await _contactRepository.GetByIdAsync(contactId);
    }
}
