namespace AspireLoanManagement.Utility.Config
{
    public interface IAspireConfigService
    {
        string GetCacheType();
        string GetRedisConnectionString();
        AspireLogger GetAspireLoggerConfig();
    }
}
