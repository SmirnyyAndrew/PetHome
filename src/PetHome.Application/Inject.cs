using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Background;
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
