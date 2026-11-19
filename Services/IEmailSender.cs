namespace BarberShopBooking.Services;

public interface IEmailSender
{
    /// <summary>Gửi email HTML. Trả về true nếu gửi thành công.</summary>
    Task<bool> SendAsync(string toEmail, string subject, string htmlBody);
}