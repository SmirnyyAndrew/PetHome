using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.Discussion.Message;
public record MessageText
{ 
    public string Value { get; }
    public static int MinMessageSize = 2;
    public MessageText(string value)
    {
        Value = value;
    }

    public static Result<MessageText, Error> Create(string value)
    {
        if (value.Trim().Count() < MinMessageSize)
            return Errors.Validation("Message text");
        return new MessageText(value);
    } 

    public static implicit operator string(MessageText text) => text.Value;
}
