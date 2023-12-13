using AspireLoanManagement.Utility.Config;

namespace AspireLoanManagement.Utility.Logger
{
    public class AspireLoggerManager : IAspireLogger
    {
        private readonly IAspireConfigService _config;
        private readonly IAspireLogger _logger;

        public AspireLoggerManager(IAspireConfigService config)
        {
             _config = config;
            _logger = LoggerFactory.CreateLogger(config.GetAspireLoggerConfig());
        }

        public void Log(LogLevel level, string message)
        {
            if(IsLogLevelAllowed(level))
            {
                _logger.Log(level, message);
            }
        }

        private bool IsLogLevelAllowed(LogLevel level)
        {
            return _config.GetAspireLoggerConfig().AllowedLevels.Contains(level);
        }
    }
}
