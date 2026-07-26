using System.Security.Cryptography;
using Forge.Shared.Contracts;

namespace Forge.Shared.Services;

public class SecureTokenGenerator
    : ISecureTokenGenerator
{
    public string Generate(int bytes = 64)
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(bytes));
    }
}