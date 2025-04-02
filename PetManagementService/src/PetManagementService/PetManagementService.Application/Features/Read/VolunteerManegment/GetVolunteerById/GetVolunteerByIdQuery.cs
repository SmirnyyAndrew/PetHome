using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Read.VolunteerManegment.GetVolunteerById;
public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;
