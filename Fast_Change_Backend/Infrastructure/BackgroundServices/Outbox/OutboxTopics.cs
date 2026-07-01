namespace Infrastructure.BackgroundServices.Outbox;

public static class OutboxTopics
{
    public const string Wallet = "wallet-events";

    public const string Exchange = "exchange-events";

    public const string User = "user-events";

    public const string Notification = "notification-events";
}
