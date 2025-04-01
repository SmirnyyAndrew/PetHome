using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Read.PetManegment.Pet.GetPetById;
public record GetPetByIdQuery(
  Guid PetId  ) : IQuery;
