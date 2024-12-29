namespace PetHome.Framework.Database.Read;
public interface IReadDBContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
    IQueryable<SpeciesDto> Species { get; }
}
