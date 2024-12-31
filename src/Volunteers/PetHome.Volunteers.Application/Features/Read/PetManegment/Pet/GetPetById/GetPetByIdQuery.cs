using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Read.PetManegment.Pet.GetPetById;
public record GetPetByIdQuery(
  Guid PetId  ) : IQuery;
