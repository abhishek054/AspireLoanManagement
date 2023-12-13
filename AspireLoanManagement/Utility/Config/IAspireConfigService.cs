namespace AspireLoanManagement.Utility.Config
{
    public interface IAspireConfigService
    {
        string GetVersion();
        string GetDB();
        string GetCacheType();
        string GetRedisConnectionString();
        AspireLogger GetAspireLoggerConfig();
    }
}
