using CSharpFunctionalExtensions;

namespace NotificationService.Core.VO;

public class TelegramSettings : ComparableValueObject
{
    public string UserId { get; }
    public long ChatId { get; }

    private TelegramSettings() { }
    public TelegramSettings(string userId, long chatId)
    {
        UserId = userId;
        ChatId = chatId;
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return UserId;
        yield return ChatId;
    }

    public static implicit operator long(TelegramSettings settings) => settings.ChatId;
}
