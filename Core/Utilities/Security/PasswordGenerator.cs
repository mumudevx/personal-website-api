using PasswordGenerator;

namespace Core.Utilities.Security;

public static class PasswordGenerator
{
    public static PasswordResult GenerateAndHash(int length = 20)
    {
        var passwordGenerator = new Password(length);
        var password = passwordGenerator.Next();

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        return new PasswordResult
        {
            Hash = hashedPassword,
            Password = password
        };
    }

    public static string Hash(string password)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        return hashedPassword;
    }
}