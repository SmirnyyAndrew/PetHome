using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.VolunteerRequest;
public record RequestComment
{
    public string Value { get; }
    public RequestComment(string value)
    {
        Value = value;
    }

    public static Result<RequestComment, Error> Create(string value)
    {
        return new RequestComment(value);
    } 

    public static implicit operator string(RequestComment id) => id.Value;
}
