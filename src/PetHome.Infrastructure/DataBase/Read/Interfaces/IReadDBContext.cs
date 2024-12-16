using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PetHome.Infrastructure.DataBase.Read.Interfaces;
public interface IReadDBContext
{
    DbSet<VolunteerDto> Volunteers { get; }
    DbSet<PetDto> Pets { get; }
    DbSet<SpeciesDto> Species { get; }
}
