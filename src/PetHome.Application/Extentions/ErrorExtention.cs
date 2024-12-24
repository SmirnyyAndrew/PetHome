using PetHome.Application.Validator;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Extentions;
public static class ErrorExtention
{
    public static ErrorList ToErrorList(this Error error)
    {
        return new ErrorList([error]);
    }
}
