namespace PetHome.Application.Features.Dtos;
public record PetInfoDto(PetMainInfoDto PetMainInfoDto, List<PhotoDto> PhotoDetailsDto);
