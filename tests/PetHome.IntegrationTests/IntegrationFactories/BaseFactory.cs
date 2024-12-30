using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.ValueObjects;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;
using PetHome.Species.Domain.SpeciesManagment.SpeciesEntity;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;
using Xunit;
using PetHome.Species.Application.Database;
using PetHome.Species.Infrastructure.Database.Write.DBContext;

namespace PetHome.IntegrationTests.IntegrationFactories;
public class BaseFactory
    : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestFactory _factory;
    protected readonly Fixture _fixture;
    protected readonly IServiceScope _scope; 

    public BaseFactory(IntegrationTestFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();  
    }


    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }


    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    } 
}
