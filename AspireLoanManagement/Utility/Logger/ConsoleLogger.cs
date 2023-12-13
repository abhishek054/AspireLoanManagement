namespace AspireLoanManagement.Utility.Logger
{
    public class ConsoleLogger : IAspireLogger
    {
        public void Log(LogLevel level, string message)
        {
            Console.WriteLine($"{DateTime.Now} [{level}] - {message}");
        }
    }
}
