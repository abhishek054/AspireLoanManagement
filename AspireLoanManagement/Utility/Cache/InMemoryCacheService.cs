using Microsoft.Extensions.Caching.Memory;

namespace AspireLoanManagement.Utility.Cache
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            _cache.Set(key, value, expiration);
        }

        public T Get<T>(string key)
        {
            return _cache.TryGetValue(key, out T value) ? value : default;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
