using Microsoft.Extensions.DependencyInjection;

namespace Notification.Sending.Factory;

internal sealed class NotificationSenderFactory : INotificationSenderFactory
{
    private readonly Dictionary<NotificationChannel, INotificationSender> _senders;

    public NotificationSenderFactory(IEnumerable<INotificationSender> senders)
    {
        _senders = senders.ToDictionary(s => s.Channel, s => s);
    }

    public INotificationSender CreateSender(NotificationChannel channel)
    {
        if (!_senders.TryGetValue(channel, out var sender))
        {
            throw new ArgumentOutOfRangeException(
                nameof(channel),
                $"No sender registered for channel: {channel}");
        }

        return sender;
    }
}

internal sealed class KeyedNotificationSenderFactory(IServiceProvider serviceProvider) : INotificationSenderFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public INotificationSender CreateSender(NotificationChannel channel)
    {
        var sender = _serviceProvider.GetRequiredKeyedService<INotificationSender>(channel);
        return sender;
    }
}
