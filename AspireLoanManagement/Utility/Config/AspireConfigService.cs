using Microsoft.Extensions.Options;

namespace AspireLoanManagement.Utility.Config
{
    public class AspireConfigService : IAspireConfigService
    {
        private readonly AspireConfigModel config;
        public AspireConfigService(IOptions<AspireConfigModel> _config)
        {
            config = _config.Value;
        }

        public string GetVersion()
        {
            return config.Version;
        }
        public string GetDB()
        {
            return config.Settings.Database;
        }
        public string GetCacheType()
        {
            return config.PreferredCache;
        }
        public string GetRedisConnectionString()
        {
            return config.Settings.RedisConnectionString;
        }

        public AspireLogger GetAspireLoggerConfig()
        {
            return config.LogDetails;
        }
    }
}
