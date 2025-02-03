using CSharpFunctionalExtensions;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Volunteers.Contracts.CreateVolunteerContract;

namespace PetHome.Volunteers.Contracts;
public interface ICreateVolunteerContract
{
    public Task<Result<Guid, ErrorList>> Execute(
         CreateVolunteerCommand createVolunteerCommand, CancellationToken ct);
}
