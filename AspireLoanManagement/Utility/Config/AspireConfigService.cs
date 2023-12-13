using Microsoft.Extensions.Options;

namespace AspireLoanManagement.Utility.Config
{
    public interface IAspireConfigService
    {
        string GetVersion();
        string GetDB();
    }
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
    }
}
