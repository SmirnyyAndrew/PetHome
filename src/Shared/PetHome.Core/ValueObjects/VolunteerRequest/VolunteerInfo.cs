using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.VolunteerRequest;
public record VolunteerInfo
{
    public string Value { get; }
    public VolunteerInfo(string value)
    {
        Value = value;
    }

    public static Result<VolunteerInfo, Error> Create(string value)
    {
        if (value.Trim().Length <= 10)
            return Errors.Validation("Данные будущего волонтёра");

        return new VolunteerInfo(value);
    }

    public static implicit operator string(VolunteerInfo id) => id.Value;
}
