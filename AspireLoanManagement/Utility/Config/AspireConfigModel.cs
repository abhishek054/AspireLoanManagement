namespace AspireLoanManagement.Utility.Config
{
    public class AspireConfigModel
    {
        public string Version { get; set; } = string.Empty;
        public AspireSetting Settings { get; set; } = new AspireSetting();
        public string PreferredCache { get; set; } = string.Empty;
        public AspireLogger LogDetails { get; set; } = new AspireLogger();

    }
    public class AspireSetting
    {
        public string Database { get; set; } = string.Empty;
        public string RedisConnectionString { get; set; } = string.Empty;
    }
    public class AspireLogger
    {
        public List<LogLevel> AllowedLevels { get; set;}
        public string Type { get; set; } = string.Empty;
        public AspireLogger()
        {
            AllowedLevels = new List<LogLevel>() { };
        }
    }
}
