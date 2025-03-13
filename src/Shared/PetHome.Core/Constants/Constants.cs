namespace PetHome.Core.Constants;
public static class Constants
{
    public static int MAX_NAME_LENGHT = 20;
    public static int MAX_DESC_LENGHT = 1000;
    public static int MAX_WEIGHT = 500;
    public const string DATABASE = "PetHomeConnectionString";
    
    public const string VOLUNTEER_UNIT_OF_WORK_KEY = "VolunteerUnitOfWorkKey";
    public const string SPECIES_UNIT_OF_WORK_KEY = "SpeciesUnitOfWorkKey";
    public const string ACCOUNT_UNIT_OF_WORK_KEY = "AccountUnitOfWorkKey";
    public const string DISCUSSION_UNIT_OF_WORK_KEY = "DiscussionUnitOfWorkKey";
    public const string VOLUNTEER_REQUEST_UNIT_OF_WORK_KEY = "VolunteerRequestUnitOfWorkKey";

    public static class Redis
    {
        public static string USER(Guid id) => $"user_{id}";
        public const string USERS = "users";
        public static string PAGED_USERS(int pageSize, int pageNum) => $"paged_users_size_{pageSize}_num_{pageNum}";

        public static string DISCUSSION(Guid id) => $"discussion_{id}";
        public static string MESSAGE(Guid id) => $"message_{id}";
         
        public static string SPECIES(Guid id) => $"species_{id}";
        public static string BREED(Guid id) => $"breed_{id}";
         
        public static string VOLUNTEER(Guid id) => $"volunteer_{id}";
        public static string PET(Guid id) => $"pet_{id}";

        public static string VOLUNTEER_REQUEST(Guid id) => $"volunteer_request_{id}";
         
        public const string REFRESH_TOKEN = "refresh_token";
    }
}
