namespace AspireLoanManagement.Utility.Config
{
    public class AspireConfigModel
    {
        public string Version { get; set; } = string.Empty;
        public AspireSetting Settings { get; set; } = new AspireSetting();

    }
    public class AspireSetting
    {
        public string Database { get; set; } = string.Empty;
    }
}
