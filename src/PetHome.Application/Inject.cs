using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Background;
using PetHome.Application.Features.Read.PetManegment.Breeds.GetAllBreedDtoBySpeciesId;
using PetHome.Application.Features.Read.PetManegment.Species.GetAllSpecies;
using PetHome.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;
using PetHome.Application.Features.Read.VolunteerManegment.GetVolunteerById;
using PetHome.Application.Features.Write.PetManegment.ChangePetInfo;
using PetHome.Application.Features.Write.PetManegment.ChangePetStatus;
using PetHome.Application.Features.Write.PetManegment.ChangeSerialNumber;
using PetHome.Application.Features.Write.PetManegment.CreateBreed;
using PetHome.Application.Features.Write.PetManegment.CreatePet;
using PetHome.Application.Features.Write.PetManegment.CreateSpecies;
using PetHome.Application.Features.Write.PetManegment.DeletePetMediaFiles;
using PetHome.Application.Features.Write.PetManegment.DeleteSpeciesById;
using PetHome.Application.Features.Write.PetManegment.HardDelete;
using PetHome.Application.Features.Write.PetManegment.SetMainPhoto;
using PetHome.Application.Features.Write.PetManegment.SoftDeleteRestore;
using PetHome.Application.Features.Write.PetManegment.UploadPetMediaFiles;
using PetHome.Application.Features.Write.VolunteerManegment.CreateVolunteer;
using PetHome.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
using PetHome.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
using PetHome.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application;
public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
        .AddClasses(classes => classes
            .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
        .AsSelfWithInterfaces()
        .WithScopedLifetime());

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        /*
        //Volunteer manegment
        services.AddScoped<CreateVolunteerUseCase>();
        services.AddScoped<UpdateMainInfoVolunteerUseCase>();
        services.AddScoped<HardDeleteVolunteerUseCase>();
        services.AddScoped<SoftDeleteVolunteerUseCase>();
        services.AddScoped<SoftRestoreVolunteerUseCase>();
        services.AddScoped<GetAllVolunteersWithPaginationUseCase>();
        services.AddScoped<GetVolunteerByIdUseCase>();

        //Pet manegment
        services.AddScoped<SoftDeletedEntitiesToHardDeleteUseCase>();
        services.AddScoped<CreatePetUseCase>();
        services.AddScoped<CreateSpeciesUseCase>();
        services.AddScoped<CreateBreedUseCase>();
        services.AddScoped<UploadPetMediaFilesUseCase>();
        services.AddScoped<DeletePetMediaFilesUseCase>();
        services.AddScoped<ChangePetSerialNumberUseCase>();
        services.AddScoped<ChangePetInfoUseCase>();
        services.AddScoped<ChangePetStatusUseCase>();
        services.AddScoped<HardDeletePetUseCase>();
        services.AddScoped<SoftDeleteRestorePetUseCase>();
        services.AddScoped<SetPetMainPhotoUseCase>();

        //Species manegment
        services.AddScoped<GetAllSpeciesUseCase>();

        //Breed manegment
        services.AddScoped<GetAllBreedDtoBySpeciesIdUseCase>();
        services.AddScoped<DeleteSpeciesByIdUseCase>();*/
        return services;
    }
}
