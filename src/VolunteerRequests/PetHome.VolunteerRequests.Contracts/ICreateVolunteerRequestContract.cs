using PetHome.Core.ValueObjects.VolunteerRequest;

namespace PetHome.VolunteerRequests.Contracts;
public interface ICreateVolunteerRequestContract
{
    public Task<VolunteerRequestId> Execute(CancellationToken ct);
}
