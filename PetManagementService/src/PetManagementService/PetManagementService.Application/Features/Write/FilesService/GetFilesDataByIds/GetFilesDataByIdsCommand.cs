using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.FilesService.GetFilesDataByIds;
public record GetFilesDataByIdsCommand(IEnumerable<Guid> Ids) : ICommand;