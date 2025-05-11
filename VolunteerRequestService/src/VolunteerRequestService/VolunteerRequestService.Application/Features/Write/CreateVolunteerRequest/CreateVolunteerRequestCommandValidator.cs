using FluentValidation; 
using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Contracts.Messaging;
using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.User;
using PetHome.SharedKernel.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Contracts.Messaging;
using PetHome.VolunteerRequests.Domain;
using PetHome.VolunteerRequests.Domain;
using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.User;
using PetHome.SharedKernel.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Contracts.Messaging;
using PetHome.VolunteerRequests.Domain;
using PetHome.SharedKernel.ValueObjects.Discussion;
using PetHome.Core.Application.Validation.Validator;

namespace PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequest;
public class CreateVolunteerRequestCommandValidator
    : AbstractValidator<CreateVolunteerRequestCommand>
{
    public CreateVolunteerRequestCommandValidator()
    {
        RuleFor(r => r.VolunteerInfo).MustBeValueObject(VolunteerInfo.Create);
    }
}
