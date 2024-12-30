using PetHome.IntegrationTests.IntegrationFactories;

namespace PetHome.IntegrationTests.Seeds;
public partial class SeedManager(IntegrationTestFactory factory) : SpeciesFactory(factory);
