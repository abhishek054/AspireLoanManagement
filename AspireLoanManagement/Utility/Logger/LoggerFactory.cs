using AspireLoanManagement.Utility.Config;

namespace AspireLoanManagement.Utility.Logger
{
    public static class LoggerFactory
    {
        public static IAspireLogger CreateLogger(AspireLogger aspireLogger)
        {
            switch(aspireLogger?.Type?.ToLower())
            {
                case "console":
                    return new ConsoleLogger();
                case "serilog":
                    throw new ArgumentException("Serilog logger implementation not done");
                case "aws-cloudwatch":
                    throw new ArgumentException("Cloudwatch logger implementation not done");
                default:
                    throw new ArgumentException("Invalid logger type");
            }
        }
    }
}
