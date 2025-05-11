using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.VolunteerRequest;
public record VolunteerInfo
{
    public string Value { get; }
    public VolunteerInfo(string value)
    {
        Value = value;
    }

    public static Result<VolunteerInfo, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Данные будущего волонтёра");

        return new VolunteerInfo(value);
    }

    public static implicit operator string(VolunteerInfo id) => id.Value;
}
