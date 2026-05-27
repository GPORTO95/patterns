using Microsoft.Extensions.Options;
using SlackNet;

namespace Notification.Sending.Factory;

internal sealed class SlackNotificationSender(IOptions<SlackSettings> slackSettings) : INotificationSender
{
    private readonly SlackSettings _slackSettings = slackSettings.Value;

    public NotificationChannel Channel => NotificationChannel.Slack;

    public async Task SendAsync(NotificationMessage message)
    {
        //var slackClient = new SlackApiClient(_slackSettings.BotToken);
        //await slackClient.Chat.PostMessage(new SlackNet.WebApi.Message
        //{
        //    Channel = message.Recipient,
        //    Text = message.Body
        //});
        Console.WriteLine("Envio feito por slack");
    }
}

public sealed class SlackSettings
{
    public string BotToken { get; set; }
}
