using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
public class UpdateMainInfoVolunteerUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateMainInfoVolunteerCommand> _validator;

    public UpdateMainInfoVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoVolunteerUseCase> logger,
        IUnitOfWork unitOfWork,
        IValidator<UpdateMainInfoVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Execute(
        UpdateMainInfoVolunteerCommand command,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid is false)
            return (ErrorList)validationResult.Errors;

        UpdateMainInfoVolunteerDto updateInfoDto = command.UpdateMainInfoDto;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            Volunteer volunteer = _volunteerRepository.GetById(command.Id, ct).Result.Value;

            FullName fullName = FullName.Create(
                updateInfoDto.FullNameDto.FirstName,
                updateInfoDto.FullNameDto.LastName).Value;

            Description description = Description.Create(updateInfoDto.Description).Value;

            List<PhoneNumber> phoneNumbers = updateInfoDto.PhoneNumbers
                .Select(p => PhoneNumber.Create(p).Value)
                .ToList();

            Email email = Email.Create(updateInfoDto.Email).Value;

            volunteer.UpdateMainInfo(
                fullName,
                description,
                phoneNumbers,
                email);

            await _volunteerRepository.Update(volunteer, ct);

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            _logger.LogInformation("Обновлена информация волонтёра с id = {0}", command.Id);
            return command.Id;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось обнавить информацию волонтёра с id = {0}", command.Id);
            return (ErrorList)Errors.Failure("Database.is.failed");
        }
    }
}
