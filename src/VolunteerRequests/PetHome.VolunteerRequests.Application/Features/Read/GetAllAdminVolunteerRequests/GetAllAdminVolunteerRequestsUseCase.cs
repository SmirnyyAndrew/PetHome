using CSharpFunctionalExtensions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllAdminVolunteerRequests;
public class GetAllAdminVolunteerRequestsUseCase
    : IQueryHandler<IReadOnlyList<VolunteerRequest>, GetAllAdminVolunteerRequestsQuery>
{
    private readonly IVolunteerRequestRepository _repository;

    public GetAllAdminVolunteerRequestsUseCase(
        IVolunteerRequestRepository repository)
    {
        _repository = repository;
    }


    public async Task<Result<IReadOnlyList<VolunteerRequest>, ErrorList>> Execute(
        GetAllAdminVolunteerRequestsQuery query, CancellationToken ct)
    {
        IReadOnlyList<VolunteerRequest> volunteerRequests = await _repository.GetByAdminId(query.AdminId, ct);
        return volunteerRequests.ToList();
    }
}
