namespace DesignPartners.Creational;

/// <summary>
/// Lets subclasses decide which notification channel to instantiate while the
/// workflow remains expressed against a shared creator contract.
/// </summary>
public interface INotificationChannel
{
    string Name { get; }
    string Deliver(string recipient, string message);
}

public abstract class NotificationCreator
{
    public string Notify(string recipient, string message)
    {
        var channel = CreateChannel();
        return channel.Deliver(recipient, message);
    }

    protected abstract INotificationChannel CreateChannel();
}

public sealed class EmailNotificationCreator : NotificationCreator
{
    protected override INotificationChannel CreateChannel() => new EmailChannel();
}

public sealed class SmsNotificationCreator : NotificationCreator
{
    protected override INotificationChannel CreateChannel() => new SmsChannel();
}

file sealed class EmailChannel : INotificationChannel
{
    public string Name => "email";

    public string Deliver(string recipient, string message) =>
        $"email:{recipient}:{message}";
}

file sealed class SmsChannel : INotificationChannel
{
    public string Name => "sms";

    public string Deliver(string recipient, string message) =>
        $"sms:{recipient}:{message}";
}
