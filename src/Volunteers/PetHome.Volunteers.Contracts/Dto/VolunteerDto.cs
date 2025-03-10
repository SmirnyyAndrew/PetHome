namespace PetHome.Volunteers.Contracts.Dto;
public record VolunteerDto(
    Guid Id, 
    Guid UserId, 
    string FirstName, 
    string LastName, 
    string? Email, 
    string Description, 
    DateTime StartVolunteeringDate); 
