using StackExchange.Redis;
using System.Text.Json;

namespace AspireLoanManagement.Utility.Cache
{
    public class RedisCacheService : IAspireCacheService
    {
        private readonly ConnectionMultiplexer _redisConnection;

        public RedisCacheService(string connectionString)
        {
            _redisConnection = ConnectionMultiplexer.Connect(connectionString);
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            var db = _redisConnection.GetDatabase();
            db.StringSet(key, JsonSerializer.Serialize(value), expiration);
        }

        public T Get<T>(string key)
        {
            var db = _redisConnection.GetDatabase();
            var value = db.StringGet(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public void Remove(string key)
        {
            var db = _redisConnection.GetDatabase();
            db.KeyDelete(key);
        }
    }
}
