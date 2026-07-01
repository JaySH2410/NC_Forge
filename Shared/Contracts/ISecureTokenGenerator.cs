namespace test.Shared.Contracts;

public interface ISecureTokenGenerator
{
    string Generate(int bytes = 64);
}