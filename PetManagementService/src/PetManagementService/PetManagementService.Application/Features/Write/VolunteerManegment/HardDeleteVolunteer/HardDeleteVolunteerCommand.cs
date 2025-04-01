using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
public record HardDeleteVolunteerCommand(VolunteerId VolunteerId) : ICommand;
