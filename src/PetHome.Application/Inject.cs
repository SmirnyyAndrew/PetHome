using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Background;
using PetHome.Application.Features.Write.PetManegment.ChangeSerialNumber;
using PetHome.Application.Features.Write.PetManegment.CreateBreed;
using PetHome.Application.Features.Write.PetManegment.CreatePet;
using PetHome.Application.Features.Write.PetManegment.CreateSpecies;
using PetHome.Application.Features.Write.PetManegment.DeletePetMediaFiles;
using PetHome.Application.Features.Write.PetManegment.UploadPetMediaFiles;
using PetHome.Application.Features.Write.VolunteerManegment.CreateVolunteer;
using PetHome.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
using PetHome.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
using PetHome.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;

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
