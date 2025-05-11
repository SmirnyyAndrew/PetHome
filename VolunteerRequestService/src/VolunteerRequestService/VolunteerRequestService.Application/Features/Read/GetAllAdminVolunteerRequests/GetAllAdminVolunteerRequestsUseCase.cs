using CSharpFunctionalExtensions;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Domain.Models;
using PetHome.Core.Web.Extentions.Collection;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllAdminVolunteerRequests;
public class GetAllAdminVolunteerRequestsUseCase
    : IQueryHandler<PagedList<VolunteerRequest>, GetAllAdminVolunteerRequestsQuery>
{
    private readonly IVolunteerRequestRepository _repository;

    public GetAllAdminVolunteerRequestsUseCase(
        IVolunteerRequestRepository repository)
    {
        _repository = repository;
    }


    public async Task<Result<PagedList<VolunteerRequest>, ErrorList>> Execute(
        GetAllAdminVolunteerRequestsQuery query, CancellationToken ct)
    {
        var volunteerRequests = await _repository.GetByAdminId(query.AdminId, ct);

        if (query.Status is not null)
            volunteerRequests = volunteerRequests.Where(d => d.Status == query.Status).ToList();

        PagedList<VolunteerRequest> volunteerRequestsPagedList = volunteerRequests
            .ToPagedList(query.PageNum, query.PageSize);

        return volunteerRequestsPagedList;
    }
}
