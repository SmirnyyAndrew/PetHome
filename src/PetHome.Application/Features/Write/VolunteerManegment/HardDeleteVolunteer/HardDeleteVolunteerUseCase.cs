using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
public class HardDeleteVolunteerUseCase
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
        VolunteerId id, CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var result = _volunteerRepository.RemoveById(id, ct).Result;

            if (result.IsFailure)
                return (ErrorList)result.Error;

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            _logger.LogInformation("Волонтёр с id = {0} навсегда удалён", id.Value);
            return result.Value;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось удалить навсегда волонтёра");
            return (ErrorList)Errors.Failure("Database.is.failed");
        }
    }


}
