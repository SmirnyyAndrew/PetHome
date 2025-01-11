namespace PetHome.Accounts.Domain.Constants;
public static class Permissions
{
    public static class Pet
    {
        public static string GET = "get.pet";
        public static string CREATE = "create.pet";
        public static string UDPATE = "update.pet";
        public static string DELETE = "delete.pet";
    }

    public static class Species
    {
        public static string GET = "get.species";
        public static string CREATE = "create.species";
        public static string UDPATE = "update.species";
        public static string DELETE = "delete.species";
    }

    public static class Volunteer
    {
        public static string GET = "get.volunteer";
        public static string CREATE = "create.volunteer";
        public static string UDPATE = "update.volunteer";
        public static string DELETE = "delete.volunteer";
    }

    public static class MinioFiles
    { 
        public static string GET = "get.miniofiles";
        public static string CREATE = "create.miniofiles";
        public static string UDPATE = "update.miniofiles";
        public static string DELETE = "delete.miniofiles";
    }
}
