using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Write.FilesService.GetFilesDataByIds;
public record GetFilesDataByIdsCommand(IEnumerable<Guid> Ids) : ICommand;