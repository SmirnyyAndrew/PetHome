using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Database.Repositories;
using PetHome.Accounts.Domain.Accounts;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.User;
using PetHome.Framework.Database;

namespace PetHome.Accounts.Application.Features.Write.CreateVolunteer;
public class CreateVolunteerAccountUseCase
    : ICommandHandler<UserId, CreateVolunteerAccountCommand>
{
    private readonly IAuthenticationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateVolunteerAccountCommand> _validator;

    public CreateVolunteerAccountUseCase(
        IAuthenticationRepository repository,
        IValidator<CreateVolunteerAccountCommand> validator,
        [FromKeyedServices(Constants.ACCOUNT_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<UserId, ErrorList>> Execute(
        CreateVolunteerAccountCommand command,
        CancellationToken ct)
    {
        var validationResult =await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid is not true)
            return validationResult.Errors.ToErrorList();

        var geRoleResult = await _repository.GetRole(VolunteerAccount.ROLE);
        if (geRoleResult.IsFailure)
            return geRoleResult.Error.ToErrorList();

        Role role = geRoleResult.Value;
        Email email = Email.Create(command.Email).Value;
        UserName userName = UserName.Create(command.UserName).Value;
        User user = User.Create(email, userName, role).Value;
        List<Requisites> requisites = command.Requisites.Select(r => Requisites.Create(r.Name, r.Desc, r.PaymentMethod).Value).ToList();
        List<Certificate> certificates = command.Certificates.Select(r => Certificate.Create(r.Name, r.Value).Value).ToList();
        Date startVolunteeringDate = Date.Create(command.StartVolunteeringDate).Value;
       
        VolunteerAccount volunteer = VolunteerAccount.Create(
            user,
            startVolunteeringDate,
            requisites,
            certificates).Value;


        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.AddUser(user, ct);
        await _repository.AddVolunteer(volunteer, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        UserId userId = UserId.Create(user.Id).Value;
        return userId;
    }
}
