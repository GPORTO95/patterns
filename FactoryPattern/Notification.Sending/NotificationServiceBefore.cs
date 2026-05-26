using Microsoft.Extensions.Configuration;
using SlackNet;
using System.Net;
using System.Net.Mail;

namespace Notification.Sending;

public class NotificationServiceBefore
{
    private readonly IConfiguration _config;

    public NotificationServiceBefore(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendAsync(string channel, string recipient, string subject, string body)
    {
        if (channel == "Email")
        {
            using var smtp = new SmtpClient(
                _config["Smtp:Host"],
                int.Parse(_config["Smtp:Port"]!));

            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(
                _config["Smtp:Username"],
                _config["Smtp:Password"]);

            var message = new MailMessage(
                from: _config["Smtp:FromAddress"]!,
                to: recipient,
                subject: subject,
                body: body);
            message.IsBodyHtml = true;

            await smtp.SendMailAsync(message);

        }
        else if (channel == "Slack")
        {
            var slackClient = new SlackApiClient(_config["Slack:BotToken"]);
            await slackClient.Chat.PostMessage(new SlackNet.WebApi.Message
            {
                Channel = recipient,
                Text = body
            });
        }
        else
        {
            throw new ArgumentException($"Unsupported channel: {channel}");
        }
    }


    public async Task SendBuilkAsync(IEnumerable<NotificationRequestBefore> requests)
    {
        foreach (var request in requests)
        {
            await SendAsync(
                request.Channel,
                request.Recipient,
                request.Subject ?? string.Empty,
                request.Body);
        }
    }

    public sealed class NotificationRequestBefore
    {
        public string Channel { get; set; } = string.Empty;

        public string Recipient { get; set; } = string.Empty;  

        public string? Subject { get; set; }

        public string? Body { get; set; }
    }
}