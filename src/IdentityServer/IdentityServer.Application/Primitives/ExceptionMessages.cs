namespace IdentityServer.Application.Primitives;

public static class ExceptionMessages
{
    public const string UserAlreadyExist = "Пользователь с таким именем {0} уже существует";
    public const string InvalidCredentials = "Некорректный логин или пароль";
    public const string ErrorWhileGettingEmailAddress = "Произошла ошибка при попытке получить email пользователя";
    public const string ErrorWhileGetAccessToken = "Произошла ошибка при попытке получить токен";
}