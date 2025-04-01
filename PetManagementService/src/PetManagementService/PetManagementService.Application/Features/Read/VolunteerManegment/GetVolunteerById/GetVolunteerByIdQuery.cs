using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Read.VolunteerManegment.GetVolunteerById;
public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;
