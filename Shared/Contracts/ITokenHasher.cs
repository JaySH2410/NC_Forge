namespace test.Shared.Contracts;

public interface ITokenHasher
{
    string Hash(string token);
    bool Verify(string token,string hash);
}
