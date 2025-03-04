namespace PetHome.Accounts.Contracts.Dto;
public record UserDto(
    Guid Id, 
    string UserName, 
    string Email, 
    string RoleName,
    DateTime DateTime);