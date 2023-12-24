namespace Application.Interfaces
{
    public interface ICacheKeyService
    {
        string GenerateHashedKey(string key);
    }
}
