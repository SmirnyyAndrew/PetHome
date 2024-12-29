using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Domain.Shared.Error;
using PetHome.Volunteers.Application.Database.RepositoryInterfaces;

namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
public class HardDeleteVolunteerUseCase
    : ICommandHandler<bool, HardDeleteVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<HardDeleteVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HardDeleteVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<HardDeleteVolunteerUseCase> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool, ErrorList>> Execute(
        HardDeleteVolunteerCommand command, 
        CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var result = _volunteerRepository.RemoveById(command.VolunteerId, ct).Result;

            if (result.IsFailure)
                return result.Error.ToErrorList();

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            _logger.LogInformation("Волонтёр с id = {0} навсегда удалён", command.VolunteerId.Value);
            return result.Value;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось удалить навсегда волонтёра");
            return Errors.Failure("Database.is.failed").ToErrorList();
        }
    }


}
