using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.Discussion.Message;
public class MessageId : ComparableValueObject
{
    public Guid Value { get; }
    public MessageId(Guid value)
    {
        Value = value;
    }

    public static Result<MessageId, Error> Create(Guid value) => new MessageId(value);

    public static Result<MessageId, Error> Create() => new MessageId(Guid.NewGuid());

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(MessageId id) => id.Value;
}
