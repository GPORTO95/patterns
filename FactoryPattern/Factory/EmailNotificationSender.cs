using System.Net.Mail;
using System.Net;
using System.Reactive.Subjects;
using Microsoft.Extensions.Options;

namespace Notification.Sending.Factory;

internal sealed class EmailNotificationSender(IOptions<SmtpSettings> smptSettings) : INotificationSender
{
    private readonly SmtpSettings _smtpSettings = smptSettings.Value;

    public NotificationChannel Channel => NotificationChannel.Email;

    public async Task SendAsync(NotificationMessage message)
    {
        //using var smtp = new SmtpClient(
        //        _smtpSettings.Host,
        //        _smtpSettings.Port);

        //smtp.EnableSsl = true;
        //smtp.Credentials = new NetworkCredential(
        //    _smtpSettings.Username,
        //    _smtpSettings.Password);

        //var mailMessage = new MailMessage(
        //    from: _smtpSettings.FromAddress,
        //    to: message.Recipient,
        //    subject: message.Subject,
        //    body: message.Body);
        //mailMessage.IsBodyHtml = true;

        //await smtp.SendMailAsync(mailMessage);


        Console.WriteLine("Envio feito por email");
    }
}

public sealed class SmtpSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string FromAddress { get; set; }
}
