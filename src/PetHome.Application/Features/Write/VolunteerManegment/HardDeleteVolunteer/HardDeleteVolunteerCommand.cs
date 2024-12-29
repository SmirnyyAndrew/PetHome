using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
public record HardDeleteVolunteerCommand(VolunteerId VolunteerId) : ICommand;
