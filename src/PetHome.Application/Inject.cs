using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Background;
using PetHome.Application.Features.Volunteers.CreateBreedVolunteer;
using PetHome.Application.Features.Volunteers.CreatePetVolunteer;
using PetHome.Application.Features.Volunteers.CreateSpeciesVolunteer;
using PetHome.Application.Features.Volunteers.CreateVolunteer;
using PetHome.Application.Features.Volunteers.HardDeleteVolunteer;
using PetHome.Application.Features.Volunteers.SoftDeleteRestoreVolunteer;
using PetHome.Application.Features.Volunteers.SoftDeleteVolunteer;
using PetHome.Application.Features.Volunteers.UpdateMainInfoVolunteer;
using PetHome.Application.Features.Volunteers.UploadPetMediaFilesVolunteer;

namespace PetHome.Application;
public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerUseCase>();
        services.AddScoped<UpdateMainInfoVolunteerUseCase>();
        services.AddScoped<HardDeleteVolunteerUseCase>();
        services.AddScoped<SoftDeleteVolunteerUseCase>();
        services.AddScoped<SoftRestoreVolunteerUseCase>();
        services.AddScoped<SoftDeletedEntitiesToHardDeleteUseCase>();
        services.AddScoped<VolunteerCreatePetUseCase>();
        services.AddScoped<VolunteerCreateSpeciesUseCase>();
        services.AddScoped<VolunteerCreateBreedUseCase>();
        services.AddScoped<UploadPetMediaFilesVolunteerUseCase>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}
