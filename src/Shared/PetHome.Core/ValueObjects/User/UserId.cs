using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.User;
public record UserId : IComparable<UserId>
{
    public Guid Value { get; }
    
    private UserId() {}
    public UserId(Guid value)
    {
        Value = value;
    }

    public static Result<UserId, Error> Create(Guid value)
    {
        return new UserId(value);
    }
    public static Result<UserId, Error> Create()
    {
        return new UserId(Guid.NewGuid());
    }

    public int CompareTo(UserId? other) => 0;

    public static implicit operator Guid(UserId id) => id.Value;
}
