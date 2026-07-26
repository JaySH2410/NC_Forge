using System.Security.Cryptography;
using System.Text;
using Forge.Shared.Contracts;

namespace Forge.Shared.Services;

public class TokenHasher : ITokenHasher
{
    public string Hash(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }

    public bool Verify(string token, string hash)
    {
        return Hash(token) == hash;
    }
}