using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Read.VolunteerManegment.GetVolunteerById;
public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;
