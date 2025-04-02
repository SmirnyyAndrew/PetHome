using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.Constants;
public static class TemplateErrors
{
    public static class Validation
    {
        public static Error Name(string name) => Errors.Validation($"Имя: \"{name}\"");
        public static Error Email(string email) => Errors.Validation($"Email: \"{email}\"");
        public static Error PhoneNumber(string number) => Errors.Validation($"Номер телефона: \"{number}\"");
        public static Error BirthDate(DateTime birthDate) => Errors.Validation($"Дата: \"{birthDate}\"");
        public static Error Description(string description) => Errors.Validation($"Описание: \"{description}\"");
    }


    public static class Conflict
    {
        public static Error User(Guid id) => Errors.Conflict($"Пользователь с id: \"{id}\"");
        public static Error Email(string email) => Errors.Conflict($"Email: \"{email}\"");
        public static Error Breed(string name) => Errors.Conflict($"Порода с названием: \"{name}\"");
        public static Error Species(string name) => Errors.Conflict($"Вид с названием: \"{name}\"");
    }


    public static class NotFound
    {
        public static class PetManagement
        {
            public static Error Pet(string name) => Errors.NotFound($"Питомец с именем: \"{name}\"");
            public static Error Species(string name) => Errors.NotFound($"Вид с именем: \"{name}\"");
            public static Error Breed(string name) => Errors.NotFound($"Порода с именем: \"{name}\"");
            public static Error Pet(Guid id) => Errors.NotFound($"Питомец с id: \"{id}\"");
        }

        public static class Accounts
        {
            public static Error User(string email) => Errors.NotFound($"Пользователь с email: \"{email}\"");
            public static Error User(Guid id) => Errors.NotFound($"Пользователь с id: \"{id}\"");
            public static Error Role(Guid id) => Errors.NotFound($"Роль с id: \"{id}\"");
            public static Error Role(string name) => Errors.NotFound($"Роль с названием: \"{name}\"");
            public static Error RefreshSession(Guid refreshToken) => Errors.NotFound($"Refresh session с refresh token: \"{refreshToken}\"");
        }

        public static class Files
        {
            public static Error File() => Errors.NotFound($"Файлы");
            public static Error File(Guid id) => Errors.NotFound($"Файл с id: \"{id}\"");
            public static Error Bucket(string name) => Errors.NotFound($"Bucket с именем: \"{name}\"");
        }

        public static class Discussions
        {
            public static Error Discussion(Guid id) => Errors.NotFound($"Обсуждение с id: \"{id}\"");
            public static Error Message(Guid id) => Errors.NotFound($"Сообщение с id: \"{id}\"");

        }
        public static class VolunteerRequests
        {
            public static Error VolunteerRequest(Guid id) => Errors.NotFound($"Запрос на волонтёрство с id: \"{id}\"");
        }
    }
}
