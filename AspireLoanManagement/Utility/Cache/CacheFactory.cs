namespace AspireLoanManagement.Utility.Cache
{
    public static class CacheFactory
    {
        public static IAspireCacheService CreateCache(string cacheType, string connectionString = null)
        {
            switch (cacheType.ToLower())
            {
                case "in-memory":
                    return new InMemoryCacheService();
                case "redis":
                    if (string.IsNullOrEmpty(connectionString))
                        throw new ArgumentException("Redis connection string is required for Redis cache.");
                    return new RedisCacheService(connectionString);
                default:
                    throw new ArgumentException("Invalid cache type specified in configuration.");
            }
        }
    }
}
