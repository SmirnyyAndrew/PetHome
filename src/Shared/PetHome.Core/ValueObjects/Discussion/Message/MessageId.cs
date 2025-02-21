using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.Discussion.Message;
public record MessageId : IComparable<MessageId>
{
    public Guid Value { get; }
    public MessageId(Guid value)
    {
        Value = value;
    }

    public static Result<MessageId, Error> Create(Guid value) => new MessageId(value);

    public static Result<MessageId, Error> Create() => new MessageId(Guid.NewGuid());

    public int CompareTo(MessageId? other) => 0;

    public static implicit operator Guid(MessageId id) => id.Value;
}
