using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.VolunteerRequest;

namespace PetHome.VolunteerRequests.Contracts;
public interface ICreateVolunteerRequestContract
{
    public Task<Result<VolunteerRequestId, Error>>  Execute(CancellationToken ct);
}
