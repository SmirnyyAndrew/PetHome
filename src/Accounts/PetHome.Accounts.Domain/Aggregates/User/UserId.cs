using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Accounts.Domain.Aggregates.User;
public record UserId
{
    private Guid Value { get;}
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
}
