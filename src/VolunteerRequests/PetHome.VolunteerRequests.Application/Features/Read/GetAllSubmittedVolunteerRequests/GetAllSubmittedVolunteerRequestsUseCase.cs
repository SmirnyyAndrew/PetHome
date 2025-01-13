using Microsoft.EntityFrameworkCore;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.VolunteerRequests.Application.Database.Dto;
using PetHome.VolunteerRequests.Application.Database.Interfaces;

namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllSubmittedVolunteerRequests;
public class GetAllSubmittedVolunteerRequestsUseCase
    : IQueryHandler<IReadOnlyList<VolunteerRequestDto>>
{  
    private readonly IVolunteerRequestReadDbContext _readDbContext;

    public GetAllSubmittedVolunteerRequestsUseCase(
        IVolunteerRequestReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
      
    public async Task<IReadOnlyList<VolunteerRequestDto>> Execute(CancellationToken ct)
    {
        IReadOnlyList<VolunteerRequestDto> volunteerRequests = await _readDbContext.VolunteerRequests.ToListAsync(ct);
        return volunteerRequests;
    }
}
