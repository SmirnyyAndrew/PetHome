using PetHome.Core.ValueObjects.PetManagment.Pet;

namespace PetHome.Volunteers.IntegrationTests.Seeds.Species;
public partial class SeedManager
{
    public async Task<IReadOnlyList<PetShelter>> SeedShelters(int shelterCountToSeed)
    {
        List<PetShelter> shelters = new List<PetShelter>(shelterCountToSeed);
        for (int i = 0; i < shelterCountToSeed; i++)
        {
            PetShelter shelter = PetShelter.Create("Приют №" + i).Value;
            shelters.Add(shelter);
        }
        return shelters;
    }

}
