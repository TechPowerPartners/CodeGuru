namespace Api.Services;

public interface IPasswordHasher
{
    string HashPassword(string password);

    bool Verify(string passwordHash, string providedPassword);
}
