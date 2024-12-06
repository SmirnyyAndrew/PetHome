using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Volunteers.CreateVolunteer;
using PetHome.Application.Features.Volunteers.UpdateMainInfoVolunteer;

namespace PetHome.Application;
public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerUseCase>();
        services.AddScoped<UpdateMainInfoVolunteerUseCase>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}
