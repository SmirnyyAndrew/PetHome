using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.VolunteerRequest;
public record RequestComment
{
    public string Value { get; }
    private RequestComment(string value)
    {
        Value = value;
    }

    public static Result<RequestComment, Error> Create(string value) => new RequestComment(value);

    public static implicit operator string(RequestComment id) => id.Value;
}
