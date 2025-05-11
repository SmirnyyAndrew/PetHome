using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
public record HardDeleteVolunteerCommand(VolunteerId VolunteerId) : ICommand;
