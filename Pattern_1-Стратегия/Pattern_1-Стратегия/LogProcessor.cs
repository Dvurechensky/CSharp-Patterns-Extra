namespace Pattern_1_Стратегия
{
    public class LogProcessor
    {
        private readonly Func<List<LogEntry>> _logimporter;

        public LogProcessor(Func<List<LogEntry>> logImporter)
        {
            _logimporter = logImporter;
        }

        public void ProcessLogs()
        {
            foreach(var logEntry in _logimporter.Invoke())
            {
                Console.WriteLine(logEntry.DateTime);
                Console.WriteLine(logEntry.Severity);
                Console.WriteLine(logEntry.Message);
            }
        }
    }
}
