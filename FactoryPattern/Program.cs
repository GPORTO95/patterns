using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notification.Sending;
using Notification.Sending.Factory;
using static Notification.Sending.NotificationService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Slack"));

builder.Services.AddSingleton<INotificationSender, EmailNotificationSender>();
builder.Services.AddSingleton<INotificationSender, SlackNotificationSender>();

builder.Services.AddSingleton<INotificationSenderFactory, NotificationSenderFactory>();

// KEYED
//builder.Services.AddKeyedSingleton<INotificationSender, EmailNotificationSender>(NotificationChannel.Email);
//builder.Services.AddKeyedSingleton<INotificationSender, SlackNotificationSender>(NotificationChannel.Slack);

//builder.Services.AddSingleton<INotificationSenderFactory, KeyedNotificationSenderFactory>();

builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var notificationService = scope.ServiceProvider.GetService<NotificationService>();

var requests = new List<NotificationRequest>
{
    new NotificationRequest
    {
        Channel = NotificationChannel.Email,
        Body = "Body email",
        Recipient = "Recipient",
        Subject = "Subject"
    },
    new NotificationRequest
    {
        Channel = NotificationChannel.Slack,
        Body = "Body Slacl"
    }
};

foreach (var request in requests)
{
    Console.WriteLine(" Sending");

    try
    {
        await notificationService.SendAsync(
            request.Channel,
            request.Recipient,
            request.Subject,
            request.Body);
    }
    catch(Exception ex)
    {
        Console.WriteLine($"x {ex.GetType().Name}: {ex.Message}");
    }

}