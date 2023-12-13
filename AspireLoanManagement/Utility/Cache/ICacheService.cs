namespace AspireLoanManagement.Utility.Cache
{
    public interface IAspireCacheService
    {
        void Set<T>(string key, T value, TimeSpan expiration = default);
        T Get<T>(string key);
        void Remove(string key);
    }
}
