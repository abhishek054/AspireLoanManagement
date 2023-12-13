using AspireLoanManagement.Utility.Config;

namespace AspireLoanManagement.Utility.Cache
{
    public class AspireCacheManager: IAspireCacheService
    {
        private readonly IAspireCacheService _cacheService;

        public AspireCacheManager(IAspireConfigService configuration)
        {
            string preferredCacheType = configuration.GetCacheType();
            string connectionString = configuration.GetRedisConnectionString();
            _cacheService = CacheFactory.CreateCache(preferredCacheType, connectionString);
        }

        public T Get<T>(string key)
        {
            return _cacheService.Get<T>(key);
        }

        public void Remove(string key)
        {
            _cacheService.Remove(key);
        }

        public void Set<T>(string key, T value, TimeSpan timespan = default)
        {
            _cacheService.Set<T>(key, value, timespan != default ? timespan : TimeSpan.FromMinutes(10));
        }
    }
}
