using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.Volunteers.VolunteerManegment.SoftDeleteRestoreVolunteer;
public class SoftDeleteVolunteerUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<SoftDeleteVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public SoftDeleteVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<SoftDeleteVolunteerUseCase> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Execute(
        Guid id, CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            Volunteer volunteer = _volunteerRepository.GetById(id, ct).Result.Value;
            volunteer.SoftDelete();
            await _volunteerRepository.Update(volunteer, ct);

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            _logger.LogInformation("Волонтёр с id = {0} и его сущности soft deleted", id);
            return id;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось удалить (soft) волонтёра с id = {0}", id);
            return (ErrorList)Errors.Failure("Database.is.failed");
        }
    }
}
