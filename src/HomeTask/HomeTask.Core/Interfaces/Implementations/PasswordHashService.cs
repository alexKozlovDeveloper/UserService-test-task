using Microsoft.AspNetCore.Identity;

namespace HomeTask.Core.Interfaces.Implementations;

public class PasswordHashService : IPasswordHashService
{
    private readonly PasswordHasher<object> _hasher = new ();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null, password);
    }

    public bool VerifyPassword(string hash, string password)
    {
        var result = _hasher.VerifyHashedPassword(null, hash, password);

        return result == PasswordVerificationResult.Success;
    }
}
