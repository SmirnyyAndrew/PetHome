using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Write.FilesService.GetFilesDataByIds;
public record GetFilesDataByIdsCommand(IEnumerable<Guid> Ids) : ICommand;