namespace PetHome.Accounts.Domain.Constants;
public static class Permissions
{
    public static class Pet
    {
        public const string GET = "get.pet";
        public const string CREATE = "create.pet";
        public const string UDPATE = "update.pet";
        public const string DELETE = "delete.pet";
    }

    public static class Species
    {
        public const string GET = "get.species";
        public const string CREATE = "create.species";
        public const string UDPATE = "update.species";
        public const string DELETE = "delete.species";
    }

    public static class User
    {
        public const string GET = "get.user";
        public const string CREATE = "create.user";
        public const string UDPATE = "update.user";
        public const string DELETE = "delete.user";
    }

    public static class Volunteer
    {
        public const string GET = "get.volunteer";
        public const string CREATE = "create.volunteer";
        public const string UDPATE = "update.volunteer";
        public const string DELETE = "delete.volunteer";
    }

    public static class Participant
    {
        public const string GET = "get.participant";
        public const string CREATE = "create.participant";
        public const string UDPATE = "update.participant";
        public const string DELETE = "delete.participant";
    }

    public static class Admin
    {
        public const string GET = "get.admin";
        public const string CREATE = "create.admin";
        public const string UDPATE = "update.admin";
        public const string DELETE = "delete.admin";
    }


    public static class MinioFiles
    { 
        public const string GET = "get.miniofiles";
        public const string CREATE = "create.miniofiles";
        public const string UDPATE = "update.miniofiles";
        public const string DELETE = "delete.miniofiles";
    }
}
