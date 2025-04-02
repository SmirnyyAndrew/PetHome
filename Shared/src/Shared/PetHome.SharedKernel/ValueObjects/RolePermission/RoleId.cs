using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.RolePermission;
public record RoleId
{
    public Guid Value { get; }

    private RoleId() { }
    private RoleId(Guid value)
    {
        Value = value;
    }

    public static Result<RoleId, Error> Create(Guid value) => new RoleId(value);

    public static implicit operator Guid(RoleId roleId) => roleId.Value;
}
