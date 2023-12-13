namespace AspireLoanManagement.Utility.Cache
{
    public interface ICacheService
    {
        void Set<T>(string key, T value, TimeSpan expiration);
        T Get<T>(string key);
        void Remove(string key);
    }
}
