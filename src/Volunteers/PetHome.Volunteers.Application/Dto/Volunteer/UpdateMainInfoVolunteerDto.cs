public record UpdateMainInfoVolunteerDto(
    FullNameDto FullNameDto,
    string Description,
    IEnumerable<string> PhoneNumbers,
    string Email);
