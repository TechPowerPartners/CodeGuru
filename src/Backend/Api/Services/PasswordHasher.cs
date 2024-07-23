using Crypt = BCrypt.Net.BCrypt;

namespace Api.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
        => Crypt.HashPassword(password);

    public bool Verify(string passwordHash, string providedPassword)
        => Crypt.Verify(providedPassword, passwordHash);
}
