using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Read.PetManegment.Pet.GetPetById;
public record GetPetByIdQuery(
  Guid PetId  ) : IQuery;
