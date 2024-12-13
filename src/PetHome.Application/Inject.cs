using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Background;
using PetHome.Application.Features.Volunteers.PetManegment.ChangeSerialNumber;
using PetHome.Application.Features.Volunteers.PetManegment.CreateBreedVolunteer;
using PetHome.Application.Features.Volunteers.PetManegment.CreatePetVolunteer;
using PetHome.Application.Features.Volunteers.PetManegment.CreateSpeciesVolunteer;
using PetHome.Application.Features.Volunteers.PetManegment.DeletePetMediaFiles;
using PetHome.Application.Features.Volunteers.PetManegment.UploadPetMediaFilesVolunteer;
using PetHome.Application.Features.Volunteers.VolunteerManegment.CreateVolunteer;
using PetHome.Application.Features.Volunteers.VolunteerManegment.HardDeleteVolunteer;
using PetHome.Application.Features.Volunteers.VolunteerManegment.SoftDeleteRestoreVolunteer;
using PetHome.Application.Features.Volunteers.VolunteerManegment.UpdateMainInfoVolunteer;

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
        services.AddScoped<CreatePetUseCase>();
        services.AddScoped<CreateSpeciesUseCase>();
        services.AddScoped<CreateBreedUseCase>();
        services.AddScoped<UploadPetMediaFilesUseCase>();
        services.AddScoped<DeletePetMediaFilesUseCase>();
        services.AddScoped<ChangePetSerialNumberUseCase>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}
