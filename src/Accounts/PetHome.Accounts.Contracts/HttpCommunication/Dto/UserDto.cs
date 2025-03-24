namespace PetHome.Accounts.Contracts.HttpCommunication.Dto;
public record UserDto(
    Guid Id, 
    string UserName, 
    string Email, 
    string RoleName,
    DateTime BirthDate);