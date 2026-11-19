using System.Net;
using System.Net.Mail;

namespace BarberShopBooking.Services;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _config;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(IConfiguration config, ILogger<SmtpEmailSender> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<bool> SendAsync(string toEmail, string subject, string htmlBody)
    {
        var host = _config["Smtp:Host"];
        if (string.IsNullOrWhiteSpace(host))
        {
            _logger.LogWarning("Chưa cấu hình Smtp:Host nên không gửi được email tới {Email}.", toEmail);
            return false;
        }

        try
        {
            var port = int.TryParse(_config["Smtp:Port"], out var p) ? p : 587;
            var user = _config["Smtp:User"];
            var password = _config["Smtp:Password"];
            var from = _config["Smtp:From"] ?? user ?? "no-reply@barbershop.local";
            var enableSsl = !bool.TryParse(_config["Smtp:EnableSsl"], out var ssl) || ssl;

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                Credentials = string.IsNullOrEmpty(user) ? null : new NetworkCredential(user, password)
            };

            using var message = new MailMessage(new MailAddress(from, "BarberShop"), new MailAddress(toEmail))
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            await client.SendMailAsync(message);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gửi email tới {Email} thất bại.", toEmail);
            return false;
        }
    }
}