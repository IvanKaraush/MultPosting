using System.Text;

namespace IdentityServer.Application.Helpers;

public static class PasswordGenerator
{
    public static string Generate(int length = 16)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
        var random = new Random();
        var sb = new StringBuilder();

        for (var i = 0; i < length; i++)
        {
            var index = random.Next(chars.Length);
            sb.Append(chars[index]);
        }

        return sb.ToString();
    }
}