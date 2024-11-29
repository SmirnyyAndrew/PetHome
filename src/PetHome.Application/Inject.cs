using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Volunteers.CreateVolunteer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHome.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateVolunteerUseCase>(); 
            return services;
        }
    }
}
